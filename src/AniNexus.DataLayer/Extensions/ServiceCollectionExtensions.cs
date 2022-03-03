//using System.Collections.Concurrent;
//using System.Linq.Expressions;
//using System.Reflection;
//using Duende.IdentityServer.EntityFramework.Options;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Infrastructure;
//using Microsoft.EntityFrameworkCore.Internal;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.DependencyInjection.Extensions;
//using Microsoft.Extensions.Options;

//namespace AniNexus.Data;

//public static class ServiceCollectionExtensions
//{
//#pragma warning disable EF1001 // Internal EF Core API usage.
//    public static IServiceCollection AddTriggeredPooledDbContextFactoryExtended<TContext>(this IServiceCollection serviceCollection, Action<DbContextOptionsBuilder> optionsAction, int poolSize = 1024)
//        where TContext : DbContext
//    {
//        return AddTriggeredPooledDbContextFactoryExtended<TContext>(serviceCollection, (_, optionsBuilder) => optionsAction(optionsBuilder), poolSize);
//    }

//    public static IServiceCollection AddTriggeredPooledDbContextFactoryExtended<TContext>(this IServiceCollection serviceCollection, Action<IServiceProvider, DbContextOptionsBuilder> optionsAction, int poolSize = 1024)
//        where TContext : DbContext
//    {
//        serviceCollection.AddPooledDbContextFactory<TContext>(optionsAction);
//        serviceCollection.Replace(ServiceDescriptor.Describe(typeof(IDbContextPool<TContext>), typeof(DbContextPoolExtended<TContext>), ServiceLifetime.Singleton));

//        return serviceCollection;
//    }

//    private class DbContextPoolExtended<TContext> : IDbContextPool<TContext>, IDisposable, IAsyncDisposable
//        where TContext : DbContext
//    {
//        public const int DefaultPoolSize = 1024;

//        private readonly ConcurrentQueue<IDbContextPoolable> Pool = new();
//        private readonly Func<DbContext> Activator;

//        private int MaxSize;
//        private int Count;

//        public DbContextPoolExtended(DbContextOptions<TContext> options, IOptions<OperationalStoreOptions> storeOptions)
//        {
//            MaxSize = options.FindExtension<CoreOptionsExtension>()?.MaxPoolSize ?? 1024;
//            if (MaxSize <= 0)
//            {
//                throw new ArgumentOutOfRangeException("MaxPoolSize", "Invalid pool size.");
//            }

//            options.Freeze();
//            Activator = CreateActivator(options, storeOptions);
//        }

//        private static Func<DbContext> CreateActivator(DbContextOptions<TContext> contextOptions, IOptions<OperationalStoreOptions> storeOptions)
//        {
//            var constructors = typeof(TContext).GetTypeInfo().DeclaredConstructors.Where(static c => !c.IsStatic && c.IsPublic).ToArray();
//            if (constructors.Length == 1)
//            {
//                var parameters = constructors[0].GetParameters();
//                if (parameters.Length == 1 &&
//                    (parameters[0].ParameterType == typeof(DbContextOptions) || parameters[0].ParameterType == typeof(DbContextOptions<TContext>)))
//                {
//                    return Expression.Lambda<Func<TContext>>(Expression.New(constructors[0], Expression.Constant(contextOptions)), Array.Empty<ParameterExpression>()).Compile();
//                }
//                else if (parameters.Length == 2 &&
//                    (parameters[0].ParameterType == typeof(DbContextOptions) || parameters[0].ParameterType == typeof(DbContextOptions<TContext>)) &&
//                    (parameters[1].ParameterType == typeof(IOptions<OperationalStoreOptions>)))
//                {
//                    return Expression.Lambda<Func<TContext>>(Expression.New(constructors[0], Expression.Constant(contextOptions), Expression.Constant(storeOptions)), Array.Empty<ParameterExpression>()).Compile();
//                }
//            }

//            throw new InvalidOperationException($"Unable to find valid constructor for DbContext {typeof(TContext).ShortDisplayName()} - the class must have a single constructor that " +
//                                                $"accepts a single argument of type {typeof(DbContextOptions).ShortDisplayName()} or {typeof(TContext).ShortDisplayName()}, or a single constructor " +
//                                                $"that accepts a single argument of type {typeof(DbContextOptions).ShortDisplayName()} or {typeof(TContext).ShortDisplayName()} and a single argument " +
//                                                $"of type {typeof(IOptions<OperationalStoreOptions>).ShortDisplayName()}.");
//        }

//        public void Dispose()
//        {
//            MaxSize = 0;
//            while (Pool.TryDequeue(out var result))
//            {
//                result.ClearLease();
//                result.Dispose();
//            }
//        }
//        public ValueTask DisposeAsync()
//        {
//            Dispose();
//            return default;
//        }
//        public IDbContextPoolable Rent()
//        {
//            if (Pool.TryDequeue(out var result))
//            {
//                Interlocked.Decrement(ref Count);
//                return result;
//            }

//            result = Activator();
//            result.SnapshotConfiguration();
//            return result;
//        }
//        public void Return(IDbContextPoolable context)
//        {
//            if (Interlocked.Increment(ref Count) <= MaxSize)
//            {
//                context.ResetState();
//                Pool.Enqueue(context);
//            }
//            else
//            {
//                PooledReturn(context);
//            }
//        }
//        public async ValueTask ReturnAsync(IDbContextPoolable context, CancellationToken cancellationToken = default)
//        {
//            if (Interlocked.Increment(ref Count) <= MaxSize)
//            {
//                await context.ResetStateAsync(cancellationToken);
//                Pool.Enqueue(context);
//            }
//            else
//            {
//                PooledReturn(context);
//            }
//        }
//        private void PooledReturn(IDbContextPoolable context)
//        {
//            Interlocked.Decrement(ref Count);
//            context.ClearLease();
//            context.Dispose();
//        }

//    }
//#pragma warning restore EF1001 // Internal EF Core API usage.
//}
