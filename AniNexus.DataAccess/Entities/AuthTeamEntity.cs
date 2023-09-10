namespace AniNexus.DataAccess.Entities;

/// <summary>
/// Models a team that contains one or more roles.
/// </summary>
public sealed class AuthTeamEntity : Entity<AuthTeamEntity>
{
    /// <summary>
    /// The name of the team.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// A description of the team.
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// The roles for the team.
    /// </summary>
    public IList<AuthTeamRoleMapEntity>? Roles { get; set; }
}
