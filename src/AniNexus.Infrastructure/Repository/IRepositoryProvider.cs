namespace AniNexus.Repository;

/// <summary>
/// Defines a repository providers and a unit of work.
/// </summary>
public interface IRepositoryProvider
{
    /// <summary>
    /// Creates a synchronous scope.
    /// </summary>
    public IRepositoryScope CreateScope();

    /// <summary>
    /// Creates an asynchronous scope.
    /// </summary>
    /// <param name="cancellationToken">A token that, when cancelled, will stop commits to the scope.</param>
    public IAsyncRepositoryScope CreateAsyncScope(in CancellationToken cancellationToken = default);
}
