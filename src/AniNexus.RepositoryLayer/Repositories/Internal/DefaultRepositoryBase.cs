using AniNexus.Data.Repository;
using AniNexus.Domain;
using Microsoft.Extensions.Logging;

namespace AniNexus.Data.Repository.Internal
{
    internal abstract class DefaultRepositoryBase : IRepository, IDisposable, IAsyncDisposable
    {
        protected ApplicationDbContext Context { get; }

        protected ILogger Logger { get; }

        protected DefaultRepositoryBase(ApplicationDbContext context, ILogger logger)
        {
            Context = context;
            Logger = logger;
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            return Context.DisposeAsync();
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            return Context.SaveChangesAsync(cancellationToken);
        }
    }
}
