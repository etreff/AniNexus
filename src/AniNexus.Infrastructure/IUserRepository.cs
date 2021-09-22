using AniNexus.Models.User;

namespace AniNexus.Infrastructure
{
    public interface IUserRepository
    {
        Task<UserDTO?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<UserDTO?> GetUserByNameAsync(string username, CancellationToken cancellationToken);
        Task<string?> GetMFAKeyAsync(Guid userId, CancellationToken cancellationToken);
        Task<string?> GetMFAKeyAsync(string username, CancellationToken cancellationToken);
        Task SetMFAKeyAsync(Guid userId, string key, CancellationToken cancellationToken);
        Task SetMFAEnabledAsync(Guid userId, CancellationToken cancellationToken);
        Task ClearMFAAsync(Guid userId, CancellationToken cancellationToken);
    }
}
