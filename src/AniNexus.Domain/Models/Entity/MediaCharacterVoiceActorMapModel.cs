using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping between a character and their voice actor.
/// </summary>
public class MediaCharacterVoiceActorMapModel : IEntityTypeConfiguration<MediaCharacterVoiceActorMapModel>
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
    /// The Id of the anime release this actor assumed the role of the character in.
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
    /// The character that is being voice acted.
    /// </summary>
    public MediaCharacterModel Character { get; set; } = default!;

    /// <summary>
    /// The actor that voices the character.
    /// </summary>
    public MediaPersonModel VoiceActor { get; set; } = default!;

    /// <summary>
    /// The anime release the actor assumed the role of the character in.
    /// </summary>
    public AnimeReleaseModel AnimeRelease { get; set; } = default!;

    /// <summary>
    /// The locale this actor is voicing the character for.
    /// </summary>
    public LocaleModel Locale { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<MediaCharacterVoiceActorMapModel> builder)
    {
        builder.ToTable("CharacterVAMap");

        builder.HasKey(m => new { m.CharacterId, m.PersonId, m.AnimeReleaseId, m.LocaleId });
        builder.HasIndex(m => m.PersonId);
        builder.HasIndex(m => m.AnimeReleaseId);
        builder.HasIndex(m => m.LocaleId);

        builder.HasOne(m => m.VoiceActor).WithMany(m => m.VoiceActedCharacters).HasForeignKey(m => m.PersonId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Character).WithMany(m => m.VoiceActors).HasForeignKey(m => m.CharacterId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.AnimeRelease).WithMany(m => m.VoiceActors).HasForeignKey(m => m.AnimeReleaseId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Locale).WithMany().HasForeignKey(m => m.LocaleId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Character).AutoInclude();
        builder.Navigation(m => m.VoiceActor).AutoInclude();
        builder.Navigation(m => m.Locale).AutoInclude();

        builder.Property(m => m.Primary).HasComment("Whether this person was the primary voice actor for this character.").IsRequired();
    }
}
