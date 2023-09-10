namespace AniNexus.DataAccess.Entities;

/// <summary>
/// Maps a <see cref="UserEntity"/> to a <see cref="AuthTeamEntity"/>.
/// </summary>
public class AuthTeamUserMapEntity : Entity<AuthTeamUserMapEntity, long>
{
    /// <summary>
    /// The Id of the user.
    /// </summary>
    public required Guid UserId { get; set; }

    /// <summary>
    /// The Id of the team.
    /// </summary>
    public required Guid TeamId { get; set; }

    /// <summary>
    /// The user.
    /// </summary>
    public UserEntity? User { get; set; }

    /// <summary>
    /// The team entity.
    /// </summary>
    public AuthTeamEntity? Team { get; set; }
}
