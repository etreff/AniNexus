using AniNexus.Data.Entities;
using AniNexus.Models;
using EntityFrameworkCore.Triggered;

namespace AniNexus.Data.Triggers;

/// <summary>
/// A trigger that is invoked whenever a <see cref="AnimeReleaseEntity"/> is saved
/// to the database.
/// </summary>
public sealed class AnimeReleaseEntityTrigger : AfterSaveTrigger<AnimeReleaseEntity>
{
    /// <summary>
    /// Creates a new <see cref="AnimeEntityTrigger"/> instance.
    /// </summary>
    /// <param name="dbContextFactory">The database context factory.</param>
    public AnimeReleaseEntityTrigger(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        : base(dbContextFactory)
    {
    }

    /// <inheritdoc/>
    public override async Task AfterSave(ITriggerContext<AnimeReleaseEntity> context, CancellationToken cancellationToken)
    {
        await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);

        EnsureOnePrimaryRelease(dbContext, context.Entity);

        if (context.ChangeType == ChangeType.Deleted)
        {
            // A release may be deleted due to a cascade when an anime is deleted. In such a case we cannot delete this
            // BeforeSave since the database would be responsible for the cascade. It is possible that EntityFramework
            // is aware of this cascade and can detect its deletion before the dataset is saved, but that is an implementation
            // detail that we don't want to rely on. Either way it is another call to the database, so it matters little if
            // it is done before or after the initial save.
            await DeleteAsync(dbContext.CompanyAnimeMap, r => r.ReleaseId == context.Entity.Id, cancellationToken);

            EnsureOnePrimaryRelease(dbContext, context.Entity);
        }
        else if (context.ChangeType == ChangeType.Added || context.ChangeType == ChangeType.Modified)
        {
            if (context.Entity.IsPrimary)
            {
                // If the primary release is updated (either via add in batch or update via single),
                // update all anime list entries to ensure that their episode count is not higher than
                // the actual length of the release.
                short? episodeCount = context.Entity.ElementCount;
                if (episodeCount.HasValue)
                {
                    var adjustedAnimeListUsers = new List<Guid>();
                    var animeListEntries = await dbContext.AnimeListEntry
                        .Where(e => e.AnimeId == context.Entity.OwnerId)
                        .Select(e => new { e.Id, e.EpisodeCount, e.StatusId, e.UserId })
                        .ToArrayAsync(cancellationToken);

                    foreach (var entry in animeListEntries)
                    {
                        // Instead of checking the old records to see what the episode count was, we are going to go based off of
                        // the user's status. If they had it set to complete and the new episode count is higher than what is on
                        // their list, update the status to incomplete. One can argue that we shouldn't update the user's list
                        // without a their permission - we will hit that bridge when we get to it.
                        bool needsStatusAdjustment = entry.EpisodeCount < episodeCount.Value && (EAnimeListStatus)entry.StatusId == EAnimeListStatus.Complete;

                        if (entry.EpisodeCount > episodeCount.Value || needsStatusAdjustment)
                        {
                            var listEntry = new AnimeListEntryEntity { Id = entry.Id };
                            dbContext.AnimeListEntry.Attach(listEntry);
                            listEntry.EpisodeCount = episodeCount.Value;
                            if (needsStatusAdjustment)
                            {
                                listEntry.StatusId = (byte)EAnimeListStatus.Paused;
                            }
                            adjustedAnimeListUsers.Add(entry.UserId);
                        }
                    }

                    //TODO: Send each user an email notifying them that we have adjusted an anime on their list.
                }
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private void EnsureOnePrimaryRelease(DbContext dbContext, AnimeReleaseEntity entity)
    {
        var relatedReleases = dbContext.ChangeTracker.Entries<AnimeReleaseEntity>().Where(r => r.Entity.OwnerId == entity.OwnerId).ToArray();
        int deletedReleaseCount = relatedReleases.Count(static r => IsEntityDeleted(r));
        if (relatedReleases.Length == deletedReleaseCount)
        {
            // All entries are being deleted.
            return;
        }

        int primaryReleaseCount = relatedReleases.Count(static r => !IsEntityDeleted(r) && r.Entity.IsPrimary);
        if (primaryReleaseCount != 1)
        {
            throw new InvalidOperationException("Exactly one release must be marked as the primary release.");
        }
    }
}
