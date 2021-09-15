using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping between a character and their live-action actor.
/// </summary>
public class MediaCharacterActorMapModel : IEntityTypeConfiguration<MediaCharacterActorMapModel>
{
    /// <summary>
    /// The Id of the character.
    /// </summary>
    /// <seealso cref="MediaCharacterModel"/>
    public int CharacterId { get; set; }

    /// <summary>
    /// The Id of the person.
    /// </summary>
    /// <seealso cref="MediaPersonModel"/>
    public int PersonId { get; set; }

    /// <summary>
    /// The Id of the release of the anime this actor assumed the role of the character in.
    /// </summary>
    /// <seealso cref="AnimeReleaseModel"/>
    public int AnimeReleaseId { get; set; }

    /// <summary>
    /// The Id of the locale.
    /// </summary>
    /// <seealso cref="LocaleModel"/>
    public int LocaleId { get; set; }

    /// <summary>
    /// Whether this actor is the primary actor for the character.
    /// </summary>
    public bool Primary { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The character that is being acted.
    /// </summary>
    public MediaCharacterModel Character { get; set; } = default!;

    /// <summary>
    /// The actor that is playing the role of the character.
    /// </summary>
    public MediaPersonModel Actor { get; set; } = default!;

    /// <summary>
    /// The anime release the actor assumed the role of the character in.
    /// </summary>
    public AnimeReleaseModel AnimeRelease { get; set; } = default!;

    /// <summary>
    /// The locale this actor is playing the character for.
    /// </summary>
    public LocaleModel Locale { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<MediaCharacterActorMapModel> builder)
    {
        builder.ToTable("CharacterActorMap");

        builder.HasKey(m => new { m.CharacterId, m.PersonId, m.AnimeReleaseId, m.LocaleId });
        builder.HasIndex(m => m.CharacterId);
        builder.HasIndex(m => m.PersonId);
        builder.HasIndex(m => m.AnimeReleaseId);
        builder.HasIndex(m => m.LocaleId);

        builder.HasOne(m => m.Character).WithMany(m => m.LiveActors).HasForeignKey(m => m.CharacterId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Actor).WithMany(m => m.ActedCharacters).HasForeignKey(m => m.PersonId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.AnimeRelease).WithMany(m => m.LiveActors).HasForeignKey(m => m.AnimeReleaseId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Locale).WithMany().HasForeignKey(m => m.LocaleId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Character).AutoInclude();
        builder.Navigation(m => m.Actor).AutoInclude();
        builder.Navigation(m => m.Locale).AutoInclude();

        builder.Property(m => m.Primary).HasComment("Whether this person was the primary actor for this character.").IsRequired();
    }
}
