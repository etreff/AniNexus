using AniNexus.Data.Models;
using EntityFrameworkCore.Triggered;

namespace AniNexus.Data.Triggers;

public class AnimeReleaseModelTrigger : BeforeSaveTrigger<AnimeReleaseModel>
{
    public AnimeReleaseModelTrigger(ApplicationDbContext dbContext)
        : base(dbContext)
    {

    }

    public override Task BeforeSave(ITriggerContext<AnimeReleaseModel> context, CancellationToken cancellationToken)
    {
        var animeEntry = DbContextFactory.ChangeTracker.Entries<AnimeModel>().FirstOrDefault(e => e.Entity.Id == context.Entity.AnimeId);
        var releasesForSameAnime = DbContextFactory.ChangeTracker.Entries<AnimeReleaseModel>().Where(e => e.Entity.AnimeId == context.Entity.AnimeId).ToArray();

        bool isAnimeDeleted = animeEntry is null || IsEntityDeleted(animeEntry);

        if (context.ChangeType == ChangeType.Deleted)
        {
            bool willHaveAtLeastOneRelease = !releasesForSameAnime.All(static r => IsEntityDeleted(r));
            if (!isAnimeDeleted && !willHaveAtLeastOneRelease)
            {
                throw new InvalidOperationException("Deleting this release would cause an anime to be left without a release. Delete the anime as well.");
            }

            if (!isAnimeDeleted && !releasesForSameAnime.Any(static r => IsEntityDeleted(r) && r.Entity.IsPrimary))
            {
                throw new InvalidOperationException("This is the primary release. Mark another release as primary before deleting this release.");
            }

            DbContextFactory.Set<MediaCompanyAnimeMapModel>().RemoveRange(m => m.ReleaseId == context.Entity.Id);
        }
        else
        {
            if (releasesForSameAnime.Count(static r => !IsEntityDeleted(r) && r.Entity.IsPrimary) != 1)
            {
                throw new InvalidOperationException("Exactly one release must be marked as the primary release.");
            }

            if (context.Entity.Names.Count(static n => n.IsPrimary) != 1)
            {
                throw new InvalidOperationException("A release must have exactly one primary name.");
            }
        }

        if (context.ChangeType == ChangeType.Modified && context.Entity.IsPrimary && context.UnmodifiedEntity is not null)
        {
            int? oldEpisodeCount = context.UnmodifiedEntity.EpisodeCount;
            int? newEpisodeCount = context.Entity.EpisodeCount;
            if (newEpisodeCount is not null && oldEpisodeCount != newEpisodeCount)
            {
                foreach (var animeListEntry in DbContextFactory.AnimeListEntries)
                {
                    if (animeListEntry.EpisodeCount > newEpisodeCount.Value)
                    {
                        animeListEntry.EpisodeCount = newEpisodeCount.Value;
                    }
                }
            }

        }

        return Task.CompletedTask;
    }
}
