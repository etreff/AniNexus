using AniNexus.Domain.Models;
using EntityFrameworkCore.Triggered;
using Microsoft.EntityFrameworkCore;

namespace AniNexus.Domain.Triggers;

public class UserModelTrigger : IBeforeSaveTrigger<UserModel>
{
    private readonly IDbContextFactory<ApplicationDbContext> DbContextFactory;

    public UserModelTrigger(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        DbContextFactory = dbContextFactory;
    }

    public async Task BeforeSave(ITriggerContext<UserModel> context, CancellationToken cancellationToken)
    {
        if (context.ChangeType == ChangeType.Deleted)
        {
            await DeleteUserRecommendationVotes(context, cancellationToken);
        }
    }

    private async Task DeleteUserRecommendationVotes(ITriggerContext<UserModel> context, CancellationToken cancellationToken)
    {
        await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);

        dbContext.AnimeUserRecommendationVotes.RemoveRange(r => r.UserId == context.Entity.Id);
        dbContext.GameUserRecommendationVotes.RemoveRange(r => r.UserId == context.Entity.Id);
        dbContext.MangaUserRecommendationVotes.RemoveRange(r => r.UserId == context.Entity.Id);

        dbContext.AnimeReviewVotes.RemoveRange(r => r.UserId == context.Entity.Id);
        dbContext.GameReviewVotes.RemoveRange(r => r.UserId == context.Entity.Id);
        dbContext.MangaReviewVotes.RemoveRange(r => r.UserId == context.Entity.Id);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
