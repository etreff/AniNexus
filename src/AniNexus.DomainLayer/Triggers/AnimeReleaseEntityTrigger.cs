using AniNexus.Domain.Entities;
using EFCore.BulkExtensions;
using EntityFrameworkCore.Triggered;

namespace AniNexus.Domain.Triggers;

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
        if (context.ChangeType == ChangeType.Deleted)
        {
            await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);

            // A release may be deleted due to a cascade when an anime is deleted. In such a case we cannot delete this
            // BeforeSave since the database would be responsible for the cascade. It is possible that EntityFramework
            // is aware of this cascade and can detect its deletion before the dataset is saved, but that is an implementation
            // detail that we don't want to rely on. Either way it is another call to the database, so it matters little if
            // it is done before or after the initial save.
            await dbContext.CompanyAnimeMap.Where(r => r.ReleaseId == context.Entity.Id).BatchDeleteAsync(cancellationToken);
        }
    }
}
