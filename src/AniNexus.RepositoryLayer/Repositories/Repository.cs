using Microsoft.EntityFrameworkCore;

namespace AniNexus.Data.Repositories;

/// <summary>
/// The base class for a repository.
/// </summary>
public abstract class Repository : IAsyncDisposable
{
    /// <summary>
    /// The database context to use for the lifetime of this repository.
    /// </summary>
    protected ApplicationDbContext DbContext { get; }

    /// <summary>
    /// Initializes a new <see cref="Repository"/> instance.
    /// </summary>
    /// <param name="dbContextFactory">The database context factory.</param>
    protected Repository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        DbContext = dbContextFactory.CreateDbContext();
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        await OnDisposeAsync();
        await DbContext.DisposeAsync();

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of any underlying resources.
    /// </summary>
    protected virtual ValueTask OnDisposeAsync()
    {
        return default;
    }

    /// <summary>
    /// Commits changes to the underlying data source.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    public Task CommitAsync(CancellationToken cancellationToken = default)
    {
        var config = new BulkConfig
        {
            IncludeGraph = true,
            OnSaveChangesSetFK = true,
            PreserveInsertOrder = true,
            SetOutputIdentity = true,
            a
        };
        return DbContext.BulkSaveChangesAsync(config, null, cancellationToken);
    }
}
