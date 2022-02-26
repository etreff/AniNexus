using AniNexus.Domain.Entities;
using EFCore.BulkExtensions;
using EntityFrameworkCore.Triggered;

namespace AniNexus.Domain.Triggers;

/// <summary>
/// A trigger that is invoked whenever a <see cref="AnimeEntity"/> is saved
/// to the database.
/// </summary>
public sealed class AnimeEntityTrigger : BeforeSaveTrigger<AnimeEntity>
{
    /// <summary>
    /// Creates a new <see cref="AnimeEntityTrigger"/> instance.
    /// </summary>
    /// <param name="dbContextFactory">The database context factory.</param>
    public AnimeEntityTrigger(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        : base(dbContextFactory)
    {
    }

    /// <inheritdoc/>
    public override async Task BeforeSave(ITriggerContext<AnimeEntity> context, CancellationToken cancellationToken)
    {
        if (context.ChangeType == ChangeType.Deleted)
        {
            await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);

            await dbContext.AnimeSystemRecommendations.Where(r => r.AnimeId == context.Entity.Id || r.AnimeRecommendationId == context.Entity.Id).BatchDeleteAsync(cancellationToken);
            await dbContext.AnimeUserRecommendations.Where(r => r.AnimeId == context.Entity.Id || r.AnimeRecommendationId == context.Entity.Id).BatchDeleteAsync(cancellationToken);
            await dbContext.AnimeRelatedMap.Where(a => a.AnimeId == context.Entity.Id || a.RelatedAnimeId == context.Entity.Id).BatchDeleteAsync(cancellationToken);
        }
        else if (context.ChangeType == ChangeType.Added)
        {
            await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);

            if (!dbContext.ChangeTracker.Entries<AnimeReleaseEntity>().Any(e => e.Entity.AnimeId == context.Entity.Id && !IsEntityDeleted(e)))
            {
                throw new InvalidOperationException("An anime must have at least one release.");
            }

            //We only need to check the mapping. The database will check that the FK in the map points to a valid series.
            if (!dbContext.ChangeTracker.Entries<MediaSeriesAnimeMapModel>().Any(e => e.Entity.AnimeId == context.Entity.Id && !IsEntityDeleted(e)))
            {
                throw new InvalidOperationException("An anime must be assigned to a series. Create a series or map the anime to an existing series.");
            }
        }
    }
}
