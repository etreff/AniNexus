using AniNexus.Domain.Models;
using EntityFrameworkCore.Triggered;

namespace AniNexus.Domain.Triggers;

public class MangaModelTrigger : BeforeSaveTrigger<MangaModel>
{
    public MangaModelTrigger(ApplicationDbContext dbContext)
        : base(dbContext)
    {

    }

    public override Task BeforeSave(ITriggerContext<MangaModel> context, CancellationToken cancellationToken)
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

    private void DeleteRecommendations(ITriggerContext<MangaModel> context)
    {
        DbContextFactory.MangaSystemRecommendations.RemoveRange(r => r.MangaId == context.Entity.Id || r.MangaRecommendationId == context.Entity.Id);
        DbContextFactory.MangaUserRecommendations.RemoveRange(r => r.MangaId == context.Entity.Id || r.MangaRecommendationId == context.Entity.Id);
    }

    private void DeleteRelated(ITriggerContext<MangaModel> context)
    {
        DbContextFactory.Set<MangaRelatedMapModel>().RemoveRange(a => a.MangaId == context.Entity.Id || a.RelatedMangaId == context.Entity.Id);
    }

    private void CheckInsertPreconditions(ITriggerContext<MangaModel> context)
    {
        if (!DbContextFactory.ChangeTracker.Entries<MangaReleaseModel>().Any(e => e.Entity.MangaId == context.Entity.Id && !IsEntityDeleted(e)))
        {
            throw new InvalidOperationException("A manga must have at least one release.");
        }

        //We only need to check the mapping. The database will check that the FK in the map points to a valid series.
        if (!DbContextFactory.ChangeTracker.Entries<MediaSeriesMangaMapModel>().Any(e => e.Entity.MangaId == context.Entity.Id && !IsEntityDeleted(e)))
        {
            throw new InvalidOperationException("A manga must be assigned to a series. Create a series or map the manga to an existing series.");
        }
    }
}
