using AniNexus.Data.Models;
using AniNexus.Data.Models.User;

namespace AniNexus.Data.Repository;

/// <summary>
/// A repository for users.
/// </summary>
public interface IUserRepository : IRepository
{
    /// <summary>
    /// Returns basic information about a user.
    /// </summary>
    /// <param name="userId">The user's Id.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<UserInfo?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns basic information about a user.
    /// </summary>
    /// <param name="username">The user's username.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<UserInfo?> GetUserByNameAsync(string username, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the MFA key for a user.
    /// </summary>
    /// <param name="userId">The user's Id.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<string?> GetMFAKeyAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the MFA key for a user.
    /// </summary>
    /// <param name="username">The user's username.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<string?> GetMFAKeyAsync(string username, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the MFA key for a user.
    /// </summary>
    /// <param name="userId">The user's Id.</param>
    /// <param name="key">The MFA key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task SetMFAKeyAsync(Guid userId, string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Enables MFA for a user.
    /// </summary>
    /// <param name="userId">The user's Id.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task SetMFAEnabledAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Enables MFA for a user.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An updated <see cref="UserInfo"/> object.</returns>
    Task<UserInfo> SetMFAEnabledAsync(UserInfo user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Clears MFA information and disables MFA for a user.
    /// </summary>
    /// <param name="userId">The user's Id.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task ClearMFAAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Clears MFA information and disables MFA for a user.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An updated <see cref="UserInfo"/> object.</returns>
    Task<UserInfo> ClearMFAAsync(UserInfo user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks that a user's credentials are valid and returns an authentication token on success.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="password">The password.</param>
    /// <param name="code">The MFA code.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<LoginResult> LoginAsync(string username, string password, string? code, CancellationToken cancellationToken = default);
}
