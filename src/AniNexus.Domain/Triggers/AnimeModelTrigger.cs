using AniNexus.Domain.Models;
using EntityFrameworkCore.Triggered;
using Microsoft.EntityFrameworkCore;

namespace AniNexus.Domain.Triggers;

public class AnimeModelTrigger : BeforeSaveTriggerBase<AnimeModel>
{
    public AnimeModelTrigger(ApplicationDbContext dbContext)
        : base(dbContext)
    {

    }

    public override Task BeforeSave(ITriggerContext<AnimeModel> context, CancellationToken cancellationToken)
    {
        if (context.ChangeType == ChangeType.Deleted)
        {
            DeleteRecommendations(context);
            DeleteRelated(context);
        }
        else if (context.ChangeType == ChangeType.Added)
        {
            CheckInsertPreconditions(context);
        }

        return Task.CompletedTask;
    }

    private void DeleteRecommendations(ITriggerContext<AnimeModel> context)
    {
        DbContext.AnimeSystemRecommendations.RemoveRange(r => r.AnimeId == context.Entity.Id || r.AnimeRecommendationId == context.Entity.Id);
        DbContext.AnimeUserRecommendations.RemoveRange(r => r.AnimeId == context.Entity.Id || r.AnimeRecommendationId == context.Entity.Id);
    }

    private void DeleteRelated(ITriggerContext<AnimeModel> context)
    {
        DbContext.Set<AnimeRelatedMapModel>().RemoveRange(a => a.AnimeId == context.Entity.Id || a.RelatedAnimeId == context.Entity.Id);
    }

    private void CheckInsertPreconditions(ITriggerContext<AnimeModel> context)
    {
        if (!DbContext.ChangeTracker.Entries<AnimeReleaseModel>().Any(e => e.Entity.AnimeId == context.Entity.Id && !IsEntityDeleted(e)))
        {
            throw new InvalidOperationException("An anime must have at least one release.");
        }

        //We only need to check the mapping. The database will check that the FK in the map points to a valid series.
        if (!DbContext.ChangeTracker.Entries<MediaSeriesAnimeMapModel>().Any(e => e.Entity.AnimeId == context.Entity.Id && !IsEntityDeleted(e)))
        {
            throw new InvalidOperationException("An anime must be assigned to a series. Create a series or map the anime to an existing series.");
        }
    }
}
