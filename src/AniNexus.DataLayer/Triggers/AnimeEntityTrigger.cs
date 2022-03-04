using AniNexus.Data.Entities;
using EntityFrameworkCore.Triggered;

namespace AniNexus.Data.Triggers;

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

            await DeleteAsync(dbContext.AnimeSystemRecommendations, r => r.AnimeId == context.Entity.Id || r.AnimeRecommendationId == context.Entity.Id, cancellationToken);
            await DeleteAsync(dbContext.AnimeUserRecommendations, r => r.AnimeId == context.Entity.Id || r.AnimeRecommendationId == context.Entity.Id, cancellationToken);
            await DeleteAsync(dbContext.AnimeRelatedMap, a => a.AnimeId == context.Entity.Id || a.RelatedAnimeId == context.Entity.Id, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
