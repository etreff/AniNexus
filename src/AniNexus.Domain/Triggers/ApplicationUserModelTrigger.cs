using AniNexus.Domain.Models;
using EntityFrameworkCore.Triggered;

namespace AniNexus.Domain.Triggers;

public class ApplicationUserModelTrigger : IBeforeSaveTrigger<ApplicationUserModel>
{
    private readonly ApplicationDbContext DbContext;

    public ApplicationUserModelTrigger(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public Task BeforeSave(ITriggerContext<ApplicationUserModel> context, CancellationToken cancellationToken)
    {
        if (context.ChangeType == ChangeType.Deleted)
        {
            DeleteUserRecommendationVotes(context);
        }

        return Task.CompletedTask;
    }

    private void DeleteUserRecommendationVotes(ITriggerContext<ApplicationUserModel> context)
    {
        DbContext.AnimeUserRecommendationVotes.RemoveRange(r => r.UserId == context.Entity.Id);
        DbContext.GameUserRecommendationVotes.RemoveRange(r => r.UserId == context.Entity.Id);
        DbContext.MangaUserRecommendationVotes.RemoveRange(r => r.UserId == context.Entity.Id);

        DbContext.AnimeReviewVotes.RemoveRange(r => r.UserId == context.Entity.Id);
        DbContext.GameReviewVotes.RemoveRange(r => r.UserId == context.Entity.Id);
        DbContext.MangaReviewVotes.RemoveRange(r => r.UserId == context.Entity.Id);
    }
}
