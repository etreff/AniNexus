using AniNexus.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AniNexus.Infrastructure.Internal
{
    internal class DefaultRepositoryProvider : IRepositoryProvider
    {
        private readonly ApplicationDbContext DbContext;
        private readonly ILoggerFactory LoggerFactory;

        public DefaultRepositoryProvider(IDbContextFactory<ApplicationDbContext> dbContextFactory, ILoggerFactory loggerFactory)
        {
            DbContext = dbContextFactory.CreateDbContext();
            LoggerFactory = loggerFactory;
        }

        public void Dispose()
        {
            DbContext.SaveChanges();
            DbContext.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            await DbContext.SaveChangesAsync();
            await DbContext.DisposeAsync();
        }

        public IAnimeRepository GetAnimeRepository()
        {
            return new DefaultAnimeRepository(DbContext, LoggerFactory.CreateLogger<DefaultAnimeRepository>());
        }
    }
}
