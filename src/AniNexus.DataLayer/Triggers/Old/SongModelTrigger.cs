#if SONGMODEL

using AniNexus.Data.Models;
using EntityFrameworkCore.Triggered;
using Microsoft.EntityFrameworkCore;

namespace AniNexus.Data.Triggers;

public class SongModelTrigger : IBeforeSaveTrigger<SongModel>
{
    private readonly ApplicationDbContext DbContext;

    public SongModelTrigger(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task BeforeSave(ITriggerContext<SongModel> context, CancellationToken cancellationToken)
    {
        if (context.ChangeType == ChangeType.Deleted)
        {
            await SetNullAnimeEpisodeOpEd(context, cancellationToken);
        }
    }

    private async Task SetNullAnimeEpisodeOpEd(ITriggerContext<SongModel> context, CancellationToken cancellationToken)
    {
        int songId = context.Entity.Id;

        var entriesToUpdate = await DbContext.AnimeEpisodes
            .Where(e => e.OpeningSongId == songId ||
                        e.EndingSongId == songId)
            .ToListAsync(cancellationToken);

        foreach (var entry in entriesToUpdate)
        {
            if (entry.OpeningSongId == songId)
            {
                entry.OpeningSongId = null;
                entry.OpeningSong = null;
            }
            if (entry.EndingSongId == songId)
            {
                entry.EndingSongId = null;
                entry.EndingSong = null;
            }
        }
    }
}

#endif