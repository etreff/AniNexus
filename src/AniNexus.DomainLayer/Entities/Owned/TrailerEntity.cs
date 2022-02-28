using AniNexus.Domain.Validation;

namespace AniNexus.Domain.Entities;

/// <summary>
/// Models a media trailer.
/// </summary>
public sealed class TrailerEntity : OwnedEntity<TrailerEntity>
{
    /// <summary>
    /// A link to the trailer.
    /// </summary>
    /// <remarks>
    /// This URL must be valid.
    /// </remarks>
    public string ResourceUrl { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity<TOwnerEntity>(OwnedNavigationBuilder<TOwnerEntity, TrailerEntity> builder)
    {
        builder.Property(m => m.ResourceUrl).HasComment("A link to the trailer.");
    }

    /// <inheritdoc/>
    public override void Validate(ValidationBuilder<TrailerEntity> validator)
    {
        validator.Property(m => m.ResourceUrl).IsValidUrl();
    }
}
