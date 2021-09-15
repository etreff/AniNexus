using AniNexus.Domain.Models;
using EntityFrameworkCore.Triggered;

namespace AniNexus.Domain.Triggers;

public class MangaReleaseModelTrigger : BeforeSaveTriggerBase<MangaReleaseModel>
{
    public MangaReleaseModelTrigger(ApplicationDbContext dbContext)
        : base(dbContext)
    {

    }

    public override Task BeforeSave(ITriggerContext<MangaReleaseModel> context, CancellationToken cancellationToken)
    {
        var mangaEntry = DbContext.ChangeTracker.Entries<MangaModel>().FirstOrDefault(e => e.Entity.Id == context.Entity.MangaId);
        var releasesForSameManga = DbContext.ChangeTracker.Entries<MangaReleaseModel>().Where(e => e.Entity.MangaId == context.Entity.MangaId).ToArray();

        bool isMangaDeleted = mangaEntry is null || IsEntityDeleted(mangaEntry);

        if (context.ChangeType == ChangeType.Deleted)
        {
            bool willHaveAtLeastOneRelease = !releasesForSameManga.All(static r => IsEntityDeleted(r));
            if (!isMangaDeleted && !willHaveAtLeastOneRelease)
            {
                throw new InvalidOperationException("Deleting this release would cause an manga to be left without a release. Delete the manga as well.");
            }

            if (!isMangaDeleted && !releasesForSameManga.Any(static r => IsEntityDeleted(r) && r.Entity.IsPrimary))
            {
                throw new InvalidOperationException("This is the primary release. Mark another release as primary before deleting this release.");
            }

            DbContext.Set<MediaCompanyMangaMapModel>().RemoveRange(m => m.ReleaseId == context.Entity.Id);
        }
        else
        {
            if (releasesForSameManga.Count(static r => !IsEntityDeleted(r) && r.Entity.IsPrimary) != 1)
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
            int? oldVolumeCount = context.UnmodifiedEntity.VolumeCount;
            int? newVolumeCount = context.Entity.VolumeCount;
            int? oldChapterCount = context.UnmodifiedEntity.ChapterCount;
            int? newChapterCount = context.Entity.ChapterCount;

            if ((newVolumeCount.HasValue && oldVolumeCount != newVolumeCount) ||
                (newChapterCount.HasValue && oldChapterCount != newChapterCount))
            {
                foreach (var mangaListEntry in DbContext.MangaListEntries)
                {
                    if (newVolumeCount.HasValue && mangaListEntry.VolumeCount > newVolumeCount.Value)
                    {
                        mangaListEntry.VolumeCount = newVolumeCount.Value;
                    }
                    if (newChapterCount.HasValue && mangaListEntry.ChapterCount > newChapterCount.Value)
                    {
                        mangaListEntry.ChapterCount = newChapterCount.Value;
                    }
                }
            }
        }

        return Task.CompletedTask;
    }
}
