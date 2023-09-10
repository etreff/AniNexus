using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.DataAccess.Entities;

/// <summary>
/// Models a saved OAuth access token for a third party API.
/// </summary>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public sealed class AccessTokenEntity : Entity<AccessTokenEntity>
{
    /// <summary>
    /// The name of the token.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The time the token was issued.
    /// </summary>
    public required DateTime IssuedAt { get; set; }

    /// <summary>
    /// The time the token expires.
    /// </summary>
    public required DateTime ExpiresAt { get; set; }

    /// <summary>
    /// The token type.
    /// </summary>
    public required string TokenType { get; set; }

    /// <summary>
    /// The token.
    /// </summary>
    public string? AccessToken { get; set; }

    /// <summary>
    /// The token.
    /// </summary>
    public string? RefreshToken { get; set; }

    private string GetDebuggerDisplay()
    {
        return $"{TokenType}: {AccessToken ?? RefreshToken}";
    }
}

internal sealed class AccessTokenEntityConfiguration : EntityTypeConfiguration<AccessTokenEntity>
{
    public override void Configure(EntityTypeBuilder<AccessTokenEntity> builder)
    {
        base.Configure(builder);

        builder.HasIndex(x => x.Name).IsUnique();
    }

    protected override string GetTableName()
    {
        return "AccessTokens";
    }
}
