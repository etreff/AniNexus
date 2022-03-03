using System.ComponentModel.DataAnnotations;
using AniNexus.Domain.Validation;

namespace AniNexus.Domain.Entities;

/// <summary>
/// Models a band or group that produces music.
/// </summary>
public sealed class SongArtistEntity : AuditableEntity<SongArtistEntity>, IHasPublicId
{
    /// <summary>
    /// The name of the artist.
    /// </summary>
    public NameEntity Name { get; set; } = default!;

    /// <summary>
    /// The public Id of this artist.
    /// </summary>
    public int PublicId { get; set; }

    /// <summary>
    /// The date this group was established.
    /// </summary>
    public Date? EstablishmentDate { get; set; }

    /// <summary>
    /// The date this group was disbanded.
    /// </summary>
    public Date? DisbandedDate { get; set; }

    /// <summary>
    /// A description of the group.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Albums created by this group.
    /// </summary>
    public IList<SongArtistAlbumMapEntity> Albums { get; set; } = default!;

    /// <summary>
    /// Songs that this group created that are not part of an album.
    /// </summary>
    public IList<SongArtistSongMapEntity> Songs { get; set; } = default!;

    /// <summary>
    /// The members who make up this group.
    /// </summary>
    public IList<SongArtistPersonMapEntity> Members { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<SongArtistEntity> builder)
    {
        base.ConfigureEntity(builder);

        // 1. Index specification
        // 2. Navigation properties
        builder.OwnsOne(m => m.Name, static owned => owned.ConfigureOwnedEntity());
        // 3. Propery specification
        builder.Property(m => m.EstablishmentDate).HasColumnName("Established").HasComment("The date this group was established.");
        builder.Property(m => m.DisbandedDate).HasColumnName("Disbanded").HasComment("The date this group was disbanded.");
        builder.Property(m => m.Description).HasComment("A description of the group.");
        // 4. Other
    }

    /// <inheritdoc/>
    protected override void Validate(ValidationContext validationContext, ValidationBuilder<SongArtistEntity> validator)
    {
        base.Validate(validationContext, validator);

        validator.Property(m => m.EstablishmentDate).IsLessThanOrEqualTo(m => m.DisbandedDate ?? Date.MaxValue, "The date the group was established must be earlier than the date the group disbanded.");
    }
}
