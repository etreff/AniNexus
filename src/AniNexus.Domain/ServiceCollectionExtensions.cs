using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using EntityFrameworkCore.Triggered.Internal;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace AniNexus.Domain;

public static class ServiceCollectionExtensions
{
#pragma warning disable EF1001 // Internal EF Core API usage.
    public static IServiceCollection AddTriggeredPooledDbContextFactoryExtended<TContext>(this IServiceCollection serviceCollection, Action<DbContextOptionsBuilder<TContext>> optionsAction, int poolSize = 1024)
        where TContext : DbContext
    {
        var optionsAction2 = optionsAction;
        return AddTriggeredPooledDbContextFactoryExtended<TContext>(serviceCollection, (_, optionsBuilder) => optionsAction2(optionsBuilder), poolSize);
    }

    public static IServiceCollection AddTriggeredPooledDbContextFactoryExtended<TContext>(this IServiceCollection serviceCollection, Action<IServiceProvider, DbContextOptionsBuilder<TContext>> optionsAction, int poolSize = 1024)
        where TContext : DbContext
    {
        // Source taken from EFCore internals.
        var optionsAction2 = optionsAction;
        AddPoolingOptions<TContext>(serviceCollection, (serviceProvider, optionsBuilder) =>
        {
            optionsAction2(serviceProvider, optionsBuilder);
            optionsBuilder.UseTriggers();
        }, poolSize);

        // Overwrite the context pool to support our context constructor.
        serviceCollection.TryAddSingleton<IDbContextPool<TContext>, DbContextPoolExtended<TContext>>();
        serviceCollection.TryAddSingleton<IDbContextFactory<TContext>>(static (IServiceProvider sp) => new PooledDbContextFactory<TContext>(sp.GetRequiredService<IDbContextPool<TContext>>()));

        // Source taken from EFCore.Triggered internals.
        var serviceDescriptor = serviceCollection.FirstOrDefault(static x => x.ServiceType == typeof(IDbContextFactory<TContext>));
        if (serviceDescriptor?.ImplementationType != null)
        {
            var triggeredFactoryType = typeof(TriggeredDbContextFactory<,>).MakeGenericType(typeof(TContext), serviceDescriptor.ImplementationType);
            serviceCollection.TryAdd(ServiceDescriptor.Describe(serviceDescriptor.ImplementationType, serviceDescriptor.ImplementationType, serviceDescriptor.Lifetime));
            serviceCollection.Replace(ServiceDescriptor.Describe(typeof(IDbContextFactory<TContext>), (IServiceProvider serviceProvider) => ActivatorUtilities.CreateInstance(serviceProvider, triggeredFactoryType, serviceProvider.GetRequiredService(serviceDescriptor.ImplementationType), serviceProvider), ServiceLifetime.Scoped));
        }

        return serviceCollection;
    }

    private static void AddPoolingOptions<TContext>(IServiceCollection serviceCollection, Action<IServiceProvider, DbContextOptionsBuilder<TContext>> optionsAction, int poolSize)
        where TContext : DbContext
    {
        var optionsAction2 = optionsAction;
        AddCoreServices(serviceCollection, (IServiceProvider serviceProvider, DbContextOptionsBuilder<TContext> optionsBuilder) =>
        {
            optionsAction2(serviceProvider, optionsBuilder);
            CoreOptionsExtension extension = (optionsBuilder.Options.FindExtension<CoreOptionsExtension>() ?? new CoreOptionsExtension()).WithMaxPoolSize(poolSize);
            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
        }, ServiceLifetime.Singleton);
    }

    private static void AddCoreServices<TContext>(IServiceCollection serviceCollection, Action<IServiceProvider, DbContextOptionsBuilder<TContext>> optionsAction, ServiceLifetime optionsLifetime)
        where TContext : DbContext
    {
        serviceCollection.TryAdd(new ServiceDescriptor(typeof(DbContextOptions<TContext>), (IServiceProvider p) => CreateDbContextOptions(p, optionsAction), optionsLifetime));
        serviceCollection.Add(new ServiceDescriptor(typeof(DbContextOptions), static p => p.GetRequiredService<DbContextOptions<TContext>>(), optionsLifetime));
    }

    private static DbContextOptions<TContext> CreateDbContextOptions<TContext>(IServiceProvider applicationServiceProvider, Action<IServiceProvider, DbContextOptionsBuilder<TContext>> optionsAction)
        where TContext : DbContext
    {
        var builder = new DbContextOptionsBuilder<TContext>(new DbContextOptions<TContext>(new Dictionary<Type, IDbContextOptionsExtension>()));
        builder.UseApplicationServiceProvider(applicationServiceProvider);
        optionsAction.Invoke(applicationServiceProvider, builder);
        return builder.Options;
    }

    private class DbContextPoolExtended<TContext> : IDbContextPool<TContext>, IDisposable, IAsyncDisposable
        where TContext : DbContext
    {
        public const int DefaultPoolSize = 1024;

        private readonly ConcurrentQueue<IDbContextPoolable> _pool = new ConcurrentQueue<IDbContextPoolable>();

        private readonly Func<DbContext> _activator;

        private int _maxSize;

        private int _count;

        public DbContextPoolExtended(DbContextOptions<TContext> options, IOptions<OperationalStoreOptions> storeOptions)
        {
            _maxSize = options.FindExtension<CoreOptionsExtension>()?.MaxPoolSize ?? 1024;
            if (_maxSize <= 0)
            {
                throw new ArgumentOutOfRangeException("MaxPoolSize", "Invalid pool size.");
            }

            options.Freeze();
            _activator = CreateActivator(options, storeOptions);
        }

        private static Func<DbContext> CreateActivator(DbContextOptions<TContext> contextOptions, IOptions<OperationalStoreOptions> storeOptions)
        {
            var constructors = typeof(TContext).GetTypeInfo().DeclaredConstructors.Where(static c => !c.IsStatic && c.IsPublic).ToArray();
            if (constructors.Length == 1)
            {
                var parameters = constructors[0].GetParameters();
                if (parameters.Length == 1 &&
                    (parameters[0].ParameterType == typeof(DbContextOptions) || parameters[0].ParameterType == typeof(DbContextOptions<TContext>)))
                {
                    return Expression.Lambda<Func<TContext>>(Expression.New(constructors[0], Expression.Constant(contextOptions)), Array.Empty<ParameterExpression>()).Compile();
                }
                else if (parameters.Length == 2 &&
                    (parameters[0].ParameterType == typeof(DbContextOptions) || parameters[0].ParameterType == typeof(DbContextOptions<TContext>)) &&
                    (parameters[1].ParameterType == typeof(IOptions<OperationalStoreOptions>)))
                {
                    return Expression.Lambda<Func<TContext>>(Expression.New(constructors[0], Expression.Constant(contextOptions), Expression.Constant(storeOptions)), Array.Empty<ParameterExpression>()).Compile();
                }
            }

            throw new InvalidOperationException($"Unable to find valid constructor for DbContext {typeof(TContext).ShortDisplayName()} - the class must have a single constructor that " +
                                                $"accepts a single argument of type {typeof(DbContextOptions).ShortDisplayName()} or {typeof(TContext).ShortDisplayName()}, or a single constructor " +
                                                $"that accepts a single argument of type {typeof(DbContextOptions).ShortDisplayName()} or {typeof(TContext).ShortDisplayName()} and a single argument " +
                                                $"of type {typeof(IOptions<OperationalStoreOptions>).ShortDisplayName()}.");
        }


        public void Dispose()
        {
            _maxSize = 0;
            while (_pool.TryDequeue(out var result))
            {
                result.ClearLease();
                result.Dispose();
            }
        }
        public ValueTask DisposeAsync()
        {
            Dispose();
            return default;
        }
        public IDbContextPoolable Rent()
        {
            if (_pool.TryDequeue(out var result))
            {
                Interlocked.Decrement(ref _count);
                return result;
            }

            result = _activator();
            result.SnapshotConfiguration();
            return result;
        }
        public void Return(IDbContextPoolable context)
        {
            if (Interlocked.Increment(ref _count) <= _maxSize)
            {
                context.ResetState();
                _pool.Enqueue(context);
            }
            else
            {
                PooledReturn(context);
            }
        }
        public async ValueTask ReturnAsync(IDbContextPoolable context, CancellationToken cancellationToken = default)
        {
            if (Interlocked.Increment(ref _count) <= _maxSize)
            {
                await context.ResetStateAsync(cancellationToken);
                _pool.Enqueue(context);
            }
            else
            {
                PooledReturn(context);
            }
        }
        private void PooledReturn(IDbContextPoolable context)
        {
            Interlocked.Decrement(ref _count);
            context.ClearLease();
            context.Dispose();
        }

    }
#pragma warning restore EF1001 // Internal EF Core API usage.
}
