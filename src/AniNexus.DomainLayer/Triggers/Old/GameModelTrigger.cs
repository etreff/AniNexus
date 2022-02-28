using EntityFrameworkCore.Triggered;

namespace AniNexus.Domain.Triggers;

public class GameModelTrigger : BeforeSaveTrigger<GameModel>
{
    public GameModelTrigger(ApplicationDbContext dbContext)
        : base(dbContext)
    {

    }

    public override Task BeforeSave(ITriggerContext<GameModel> context, CancellationToken cancellationToken)
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

    private void DeleteRecommendations(ITriggerContext<GameModel> context)
    {
        DbContextFactory.GameSystemRecommendations.RemoveRange(r => r.GameId == context.Entity.Id || r.GameRecommendationId == context.Entity.Id);
        DbContextFactory.GameUserRecommendations.RemoveRange(r => r.GameId == context.Entity.Id || r.GameRecommendationId == context.Entity.Id);
    }

    private void DeleteRelated(ITriggerContext<GameModel> context)
    {
        DbContextFactory.Set<GameRelatedMapModel>().RemoveRange(a => a.GameId == context.Entity.Id || a.RelatedGameId == context.Entity.Id);
    }

    private void CheckInsertPreconditions(ITriggerContext<GameModel> context)
    {
        //We only need to check the mapping. The database will check that the FK in the map points to a valid series.
        if (!DbContextFactory.ChangeTracker.Entries<MediaSeriesGameMapModel>().Any(e => e.Entity.GameId == context.Entity.Id && !IsEntityDeleted(e)))
        {
            throw new InvalidOperationException("An anime must be assigned to a series. Create a series or map the game to an existing series.");
        }
    }
}
