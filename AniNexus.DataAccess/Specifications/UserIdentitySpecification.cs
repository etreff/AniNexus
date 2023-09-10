using AniNexus.DataAccess.Entities;

namespace AniNexus.DataAccess.Specifications;

/// <summary>
/// A specification for getting the user and their claims.
/// </summary>
public sealed class UserIdentitySpecification : QuerySpecification<UserEntity>
{
    /// <summary>
    /// Creates a new <see cref="UserIdentitySpecification"/> instance.
    /// </summary>
    /// <param name="username">The username of the identity to fetch.</param>
    /// <param name="includeBanReasons">Whether to fetch ban reasons. Used for login and moderation.</param>
    public UserIdentitySpecification(string username, bool includeBanReasons = false)
        : base(x => x.Username == username)
    {
        // Include the teams, the claim mappings, and the claims themselves.
        AddInclude(x => x.Teams!)
            .ThenInclude(x => x.Team)
            .ThenInclude(x => x!.Roles!)
            .ThenInclude(x => x.Role);

        if (includeBanReasons)
        {
            AddInclude(x => x.BanReasons!);
        }
    }
}
