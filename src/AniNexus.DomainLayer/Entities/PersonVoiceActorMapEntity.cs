namespace AniNexus.Domain.Entities;

/// <summary>
/// Models a mapping between a character and their voice actor.
/// </summary>
public class PersonVoiceActorMapEntity : Entity<PersonVoiceActorMapEntity>
{
    /// <summary>
    /// The Id of the character.
    /// </summary>
    public Guid CharacterId { get; set; }

    /// <summary>
    /// The Id of the person.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// The Id of the anime release this actor assumed the role of the character in.
    /// </summary>
    public Guid ReleaseId { get; set; }

    /// <summary>
    /// The character that is being voice acted.
    /// </summary>
    public CharacterEntity Character { get; set; } = default!;

    /// <summary>
    /// The actor that voices the character.
    /// </summary>
    public PersonEntity VoiceActor { get; set; } = default!;

    /// <summary>
    /// The anime release the actor assumed the role of the character in.
    /// </summary>
    public AnimeReleaseEntity Release { get; set; } = default!;

    /// <inheritdoc/>
    protected override string GetTableName()
    {
        return "PersonVAMap";
    }

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<PersonVoiceActorMapEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => new { m.CharacterId, m.PersonId, m.ReleaseId }).IsUnique();
        builder.HasIndex(m => m.PersonId);
        builder.HasIndex(m => m.ReleaseId);
        // 2. Navigation properties
        builder.HasOne(m => m.VoiceActor).WithMany(m => m.VoiceActedCharacters).HasForeignKey(m => m.PersonId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Character).WithMany(m => m.VoiceActors).HasForeignKey(m => m.CharacterId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Release).WithMany(m => m.VoiceActors).HasForeignKey(m => m.ReleaseId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // 3. Propery specification
        // 4. Other
    }
}
