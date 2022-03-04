using AniNexus.Data.Entities;
using EntityFrameworkCore.Triggered;

namespace AniNexus.Data.Triggers;

/// <summary>
/// A trigger that is invoked whenever a <see cref="UserEntity"/> is saved
/// to the database.
/// </summary>
public sealed class UserEntityTrigger : BeforeSaveTrigger<UserEntity>
{
    /// <summary>
    /// Creates a new <see cref="UserEntityTrigger"/> instance.
    /// </summary>
    /// <param name="dbContextFactory">The database context factory.</param>
    public UserEntityTrigger(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        : base(dbContextFactory)
    {
    }

    /// <inheritdoc/>
    public override async Task BeforeSave(ITriggerContext<UserEntity> context, CancellationToken cancellationToken)
    {
        if (context.ChangeType == ChangeType.Deleted)
        {
            await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);

            await DeleteAsync(dbContext.AnimeUserRecommendationVotes, r => r.UserId == context.Entity.Id, cancellationToken);
            await DeleteAsync(dbContext.AnimeUserReviewVotes, r => r.UserId == context.Entity.Id, cancellationToken);
            await DeleteAsync(dbContext.AnimeUserReviewsDeleted, r => r.UserId == context.Entity.Id, cancellationToken);

            foreach (var id in dbContext.AnimeUserReviewsDeleted.Where(r => r.DeletedByUserId == context.Entity.Id).Select(r => r.Id).ToArray())
            {
                var review = new AnimeUserReviewDeletedEntity { Id = id };
                dbContext.AnimeUserReviewsDeleted.Attach(review);
                review.DeletedByUserId = null;
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
