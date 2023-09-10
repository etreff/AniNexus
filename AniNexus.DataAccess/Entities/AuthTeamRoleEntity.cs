namespace AniNexus.DataAccess.Entities;

/// <summary>
/// Models a user claim.
/// </summary>
public sealed class AuthTeamRoleEntity : Entity<AuthTeamRoleEntity>
{
    /// <summary>
    /// The name of the role.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The value of role.
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// A description of the role.
    /// </summary>
    public string? Description { get; set; }
}
