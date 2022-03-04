using AniNexus.Data.Entities;
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
        await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);

        if (context.ChangeType == ChangeType.Added || context.ChangeType == ChangeType.Modified)
        {
            if (context.Entity.SubGenreId.HasValue)
            {
                // I don't know what we are supposed to throw here, but since I can't find any documentation
                // on doing something similar in native EFCore without overriding SaveChangesAsync (which is
                // also skipped by the batching library) we need to do this here.
                var subgenre = await dbContext.MusicSubGenres.FindAsync(new object?[] { context.Entity.SubGenreId.Value }, cancellationToken);
                if (subgenre is null)
                {
                    throw new InvalidOperationException($"No sub-genre was found with the Id {context.Entity.SubGenreId.Value}.");
                }
                if (context.Entity.GenreId != subgenre.GenreId)
                {
                    var genre = await dbContext.MusicGenres.FindAsync(new object?[] { context.Entity.GenreId }, cancellationToken);
                    if (genre is not null)
                    {
                        throw new InvalidOperationException($"{subgenre.Name} is not a valid sub-genre of genre {genre.Name}.");
                    }

                    // If the genre is not found, this will break the FK constraint and the database will throw a more natural error.
                }
            }
        }
        else if (context.ChangeType == ChangeType.Deleted)
        {
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
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
