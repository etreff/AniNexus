using AniNexus.Collections.Concurrent;
using AniNexus.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AniNexus.Repository.Internal;

internal class DefaultRepositoryProvider : IRepositoryProvider
{
    private readonly IServiceProvider Services;

    public DefaultRepositoryProvider(IServiceProvider services)
    {
        // Taking in the service provider itself is an anti-pattern, but since this is an internal implementation
        // we will know and inject the necessary dependencies.
        Services = services;
    }

    public IRepositoryScope CreateScope()
    {
        return new RepositoryScope(Services, default);
    }

    public IAsyncRepositoryScope CreateAsyncScope(in CancellationToken cancellationToken)
    {
        return new RepositoryScope(Services, cancellationToken);
    }

    private class RepositoryScope : IAsyncRepositoryScope
    {
        private class RepositoryCache : ThreadSafeCache<Type, IRepository>
        {
            public RepositoryCache(IServiceProvider services)
                : base(t => (IRepository)services.CreateService(t, services.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext()))
            {

            }
        }

        private readonly RepositoryCache Cache;
        private readonly CancellationToken Token;
        private readonly ILogger Logger;

        public RepositoryScope(IServiceProvider services, in CancellationToken cancellationToken)
        {
            Token = cancellationToken;
            Cache = new RepositoryCache(services);
            Logger = services.GetRequiredService<ILogger<RepositoryScope>>();
        }

        public void Dispose()
        {
            foreach (var repository in Cache.Values)
            {
                try
                {
                    repository.Commit();
                }
                catch (Exception e)
                {
                    Logger.LogError(e, "An error has occurred while commiting a repository.");
                }

                if (repository is IDisposable d)
                {
                    d.Dispose();
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (!Token.IsCancellationRequested)
            {
                foreach (var repository in Cache.Values)
                {
                    if (Token.IsCancellationRequested)
                    {
                        break;
                    }

                    try
                    {
                        await repository.CommitAsync(Token);
                    }
                    catch (OperationCanceledException)
                    {
                        // Suppress
                    }
                    catch (Exception e)
                    {
                        Logger.LogError(e, "An error has occurred while commiting a repository.");
                    }

                    if (repository is IAsyncDisposable d)
                    {
                        await d.DisposeAsync();
                    }
                }
            }
        }

        public IAnimeRepository GetAnimeRepository()
        {
            return (IAnimeRepository)Cache.Get(typeof(DefaultAnimeRepository));
        }

        public IUserRepository GetUserRepository()
        {
            return (IUserRepository)Cache.Get(typeof(DefaultUserRepository));
        }
    }
}
