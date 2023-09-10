namespace AniNexus.DataAccess.Entities;

/// <summary>
/// Maps a <see cref="AuthTeamEntity"/> to a <see cref="AuthTeamRoleEntity"/>.
/// </summary>
public sealed class AuthTeamRoleMapEntity : Entity<AuthTeamRoleMapEntity, long>
{
    /// <summary>
    /// The Id of the team.
    /// </summary>
    public required Guid TeamId { get; set; }

    /// <summary>
    /// The Id of the role.
    /// </summary>
    public required Guid RoleId { get; set; }

    /// <summary>
    /// The team.
    /// </summary>
    public AuthTeamEntity? Team { get; set; }

    /// <summary>
    /// The role.
    /// </summary>
    public AuthTeamRoleEntity? Role { get; set; }
}
