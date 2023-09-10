namespace AniNexus.DataAccess.Entities;

/// <summary>
/// Models a user who is banned.
/// </summary>
public sealed class UserBannedEntity : Entity<UserBannedEntity, Guid>
{
    /// <summary>
    /// The Id of the user that was banned.
    /// </summary>
    public required Guid UserId { get; set; }

    /// <summary>
    /// The UTC date until which the user is banned.
    /// </summary>
    public DateTime? BannedUntil { get; set; }

    /// <summary>
    /// The reason the user was banned.
    /// </summary>
    public required string Reason { get; set; }

    /// <summary>
    /// The user that was banned.
    /// </summary>
    public UserEntity? User { get; set; }
}
