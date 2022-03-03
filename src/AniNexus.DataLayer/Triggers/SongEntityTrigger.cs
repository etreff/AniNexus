using AniNexus.Data.Entities;
using EFCore.BulkExtensions;
using EntityFrameworkCore.Triggered;

namespace AniNexus.Data.Triggers;

/// <summary>
/// A trigger that is invoked whenever a <see cref="SongEntityTrigger"/> is saved
/// to the database.
/// </summary>
public sealed class SongEntityTrigger : BeforeSaveTrigger<SongEntity>
{
    /// <summary>
    /// Creates a new <see cref="SongEntityTrigger"/> instance.
    /// </summary>
    /// <param name="dbContextFactory">The database context factory.</param>
    public SongEntityTrigger(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        : base(dbContextFactory)
    {
    }

    /// <inheritdoc/>
    public override async Task BeforeSave(ITriggerContext<SongEntity> context, CancellationToken cancellationToken)
    {
        if (context.ChangeType == ChangeType.Deleted)
        {
            await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);

            foreach (var id in dbContext.AnimeEpisodes
                .Where(e => e.OpeningSongId == context.Entity.Id || e.EndingSongId == context.Entity.Id)
                .Select(r => new { r.Id, r.OpeningSongId, r.EndingSongId })
                .ToArray())
            {
                var episode = new AnimeEpisodeEntity { Id = id.Id };
                dbContext.AnimeEpisodes.Attach(episode);
                if (id.OpeningSongId == context.Entity.Id)
                {
                    episode.OpeningSongId = null;
                }
                if (id.EndingSongId == context.Entity.Id)
                {
                    episode.EndingSongId = null;
                }
            }

            await dbContext.BulkSaveChangesAsync(cancellationToken: cancellationToken);
        }
    }
}
