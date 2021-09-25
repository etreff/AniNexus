namespace AniNexus.Repository
{
    public interface IRepository
    {
        /// <summary>
        /// Commits changes to the underlying data source.
        /// </summary>
        void Commit();

        /// <summary>
        /// Commits changes to the underlying data source.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task CommitAsync(CancellationToken cancellationToken = default);
    }
}
