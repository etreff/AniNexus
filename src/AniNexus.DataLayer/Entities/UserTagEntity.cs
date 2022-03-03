using System.ComponentModel.DataAnnotations;
using AniNexus.Data.Validation;

namespace AniNexus.Data.Entities;

/// <summary>
/// Models a user tag for a media.
/// </summary>
public sealed class UserTagEntity : Entity<UserTagEntity, int>, IHasTranslations<UserTagTranslationEntity, UserTagEntity, int>
{
    /// <summary>
    /// The name of the tag.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// The UTC date and time this entity was added to the table.
    /// </summary>
    public DateTime DateAdded { get; set; }

    /// <summary>
    /// The UTC date and time this entity was added to the table.
    /// </summary>
    public DateTime SubmissionDate { get; set; }

    /// <summary>
    /// The Id of the user who submitted this tag.
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// The user who submitted this tag.
    /// </summary>
    public UserEntity? User { get; set; }

    /// <inheritdoc/>
    public IList<UserTagTranslationEntity> Translations { get; set; } = default!;

    /// <summary>
    /// Anime that this tag has been applied to.
    /// </summary>
    public IList<AnimeTagMapEntity> Anime { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<UserTagEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => m.Name).IsUnique();
        builder.HasIndex(m => m.UserId).HasNotNullFilter();
        // 2. Navigation properties
        builder.HasOne(m => m.User).WithMany(m => m.ApprovedTags).HasForeignKey(m => m.UserId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
        // 3. Propery specification
        builder.Property(m => m.Name).HasComment("The name of the tag.").HasMaxLength(64);
        builder.Property(m => m.DateAdded).HasComputedColumnSql("getutcdate()").ValueGeneratedOnAdd().HasComment("The date the entity was created.");
        builder.Property(m => m.SubmissionDate).HasComment("The date the translation was submitted for review.");
        // 4. Other
    }
}

/// <summary>
/// Models a user tag for a media that is pending approval.
/// </summary>
public sealed class UserTagPendingEntity : Entity<UserTagPendingEntity, int>
{
    /// <summary>
    /// The name of the tag.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// The UTC date and time this entity was added to the table.
    /// </summary>
    public DateTime SubmissionDate { get; set; }

    /// <summary>
    /// The Id of the user who submitted this tag.
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// The user who submitted this tag.
    /// </summary>
    public UserEntity? User { get; set; }

    /// <inheritdoc/>
    public IList<UserTagTranslationEntity> Translations { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<UserTagPendingEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => m.Name).IsUnique();
        builder.HasIndex(m => m.UserId).HasNotNullFilter();
        // 2. Navigation properties
        builder.HasOne(m => m.User).WithMany(m => m.PendingTags).HasForeignKey(m => m.UserId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
        // 3. Propery specification
        builder.Property(m => m.Name).HasComment("The name of the tag.").HasMaxLength(64);
        builder.Property(m => m.SubmissionDate).HasComputedColumnSql("getutcdate()").ValueGeneratedOnAdd().HasComment("The date the entity was created.");
        // 4. Other
    }
}

/// <summary>
/// Models a <see cref="UserTagEntity"/> that was rejected by staff.
/// </summary>
public sealed class UserTagRejectedEntity : Entity<UserTagRejectedEntity, int>
{
    /// <summary>
    /// The name of the tag.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// The Id of the user who submitted this tag.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// The UTC date this translation was submitted.
    /// </summary>
    public DateTime SubmissionDate { get; set; }

    /// <summary>
    /// The UTC date this translation was rejected.
    /// </summary>
    public DateTime RejectionDate { get; set; }

    /// <summary>
    /// The user who submitted this translation.
    /// </summary>
    public UserEntity User { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<UserTagRejectedEntity> builder)
    {
        // 1. Primary key specification (if not Entity<>)
        // 2. Index specification
        // 3. Navigation properties
        builder.HasOne(m => m.User).WithMany(m => m.RejectedTags).HasForeignKey(m => m.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // 4. Propery specification
        builder.Property(m => m.Name).HasMaxLength(64).HasComment("The name of the tag.");
        builder.Property(m => m.SubmissionDate).HasComment("The date the translation was submitted for review.");
        builder.Property(m => m.RejectionDate).HasComputedColumnSql("getutcdate()").ValueGeneratedOnAdd().HasComment("The date the translation was rejected.");
    }

    /// <inheritdoc/>
    protected override void Validate(ValidationContext validationContext, UserTagRejectedEntity entity, ValidationBuilder<UserTagRejectedEntity> validator)
    {
        base.Validate(validationContext, entity, validator);

        validator.Property(m => m.Name).IsNotNullOrWhiteSpace();
        validator.Property(m => m.SubmissionDate).IsNotDefault();
    }
}

/// <summary>
/// Models a translation for a user tag.
/// </summary>
public sealed class UserTagTranslationEntity : TranslationEntity<UserTagTranslationEntity, UserTagEntity, int>
{
    /// <summary>
    /// The Id of the user who submitted this tag.
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// The user who submitted this tag.
    /// </summary>
    public UserEntity? User { get; set; }

    /// <summary>
    /// The UTC date this translation was submitted.
    /// </summary>
    public DateTime SubmissionDate { get; set; }

    /// <summary>
    /// The UTC date this translation was accepted.
    /// </summary>
    public DateTime ApprovalDate { get; set; }

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<UserTagTranslationEntity> builder)
    {
        base.ConfigureEntity(builder);

        // 1. Primary key specification (if not Entity<>)
        // 2. Index specification
        // 3. Navigation properties
        builder.HasOne(m => m.User).WithMany(m => m.ApprovedTagTranslations).HasForeignKey(m => m.UserId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
        // 4. Propery specification
        builder.Property(m => m.ApprovalDate).HasComputedColumnSql("getutcdate()").ValueGeneratedOnAdd().HasComment("The date the translation was rejected.");
        builder.Property(m => m.SubmissionDate).HasComment("The date the translation was submitted for review.");
    }

    /// <inheritdoc/>
    protected override void Validate(ValidationContext validationContext, UserTagTranslationEntity entity, ValidationBuilder<UserTagTranslationEntity> validator)
    {
        base.Validate(validationContext, entity, validator);

        validator.Property(m => m.SubmissionDate).IsNotDefault();
    }
}

/// <summary>
/// Models a <see cref="UserTagTranslationEntity"/> that is pending review and approval from staff.
/// </summary>
public sealed class UserTagPendingTranslationEntity : Entity<UserTagPendingTranslationEntity, int>
{
    /// <summary>
    /// The translation.
    /// </summary>
    public string Translation { get; set; } = default!;

    /// <summary>
    /// The Id of the user tag that the translation would be applied to.
    /// </summary>
    public Guid UserTagId { get; set; }

    /// <summary>
    /// The Id of the language of this translation.
    /// </summary>
    public short LanguageId { get; set; }

    /// <summary>
    /// The Id of the user who submitted this translation.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// The UTC date this translation was submitted.
    /// </summary>
    public DateTime SubmissionDate { get; set; }

    /// <summary>
    /// The user tag that the translation would be applied to.
    /// </summary>
    public UserTagEntity UserTag { get; set; } = default!;

    /// <summary>
    /// The language of this translation.
    /// </summary>
    public LanguageEntity Language { get; set; } = default!;

    /// <summary>
    /// The user who submitted this translation.
    /// </summary>
    public UserEntity User { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<UserTagPendingTranslationEntity> builder)
    {
        // 1. Primary key specification (if not Entity<>)
        // 2. Index specification
        // 3. Navigation properties
        builder.HasOne(m => m.Language).WithMany().HasForeignKey(m => m.LanguageId).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.User).WithMany(m => m.PendingTagTranslations).HasForeignKey(m => m.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.UserTag).WithMany().HasForeignKey(m => m.UserTagId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // 4. Propery specification
        builder.Property(m => m.Translation).HasMaxLength(250).HasComment("The translation.");
        builder.Property(m => m.SubmissionDate).HasComputedColumnSql("getutcdate()").ValueGeneratedOnAdd().HasComment("The date the translation was submitted for review.");
    }

    /// <inheritdoc/>
    protected override void Validate(ValidationContext validationContext, UserTagPendingTranslationEntity entity, ValidationBuilder<UserTagPendingTranslationEntity> validator)
    {
        base.Validate(validationContext, entity, validator);

        validator.Property(m => m.Translation).IsNotNullOrWhiteSpace();
    }
}

/// <summary>
/// Models a <see cref="UserTagTranslationEntity"/> that was rejected by staff.
/// </summary>
public sealed class UserTagRejectedTranslationEntity : Entity<UserTagRejectedTranslationEntity, int>
{
    /// <summary>
    /// The translation.
    /// </summary>
    public string Translation { get; set; } = default!;

    /// <summary>
    /// The Id of the user tag that the translation would have be applied to.
    /// </summary>
    public Guid UserTagId { get; set; }

    /// <summary>
    /// The Id of the language of this translation.
    /// </summary>
    public short LanguageId { get; set; }

    /// <summary>
    /// The Id of the user who submitted this translation.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// The UTC date this translation was submitted.
    /// </summary>
    public DateTime SubmissionDate { get; set; }

    /// <summary>
    /// The UTC date this translation was rejected.
    /// </summary>
    public DateTime RejectionDate { get; set; }

    /// <summary>
    /// The user tag that the translation would be applied to.
    /// </summary>
    public UserTagEntity UserTag { get; set; } = default!;

    /// <summary>
    /// The language of this translation.
    /// </summary>
    public LanguageEntity Language { get; set; } = default!;

    /// <summary>
    /// The user who submitted this translation.
    /// </summary>
    public UserEntity User { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<UserTagRejectedTranslationEntity> builder)
    {
        // 1. Primary key specification (if not Entity<>)
        // 2. Index specification
        // 3. Navigation properties
        builder.HasOne(m => m.Language).WithMany().HasForeignKey(m => m.LanguageId).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.User).WithMany(m => m.RejectedTagTranslations).HasForeignKey(m => m.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.UserTag).WithMany().HasForeignKey(m => m.UserTagId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // 4. Propery specification
        builder.Property(m => m.Translation).HasMaxLength(250).HasComment("The translation.");
        builder.Property(m => m.SubmissionDate).HasComment("The date the translation was submitted for review.");
        builder.Property(m => m.RejectionDate).HasComputedColumnSql("getutcdate()").ValueGeneratedOnAdd().HasComment("The date the translation was rejected.");
    }

    /// <inheritdoc/>
    protected override void Validate(ValidationContext validationContext, UserTagRejectedTranslationEntity entity, ValidationBuilder<UserTagRejectedTranslationEntity> validator)
    {
        base.Validate(validationContext, entity, validator);

        validator.Property(m => m.Translation).IsNotNullOrWhiteSpace();
        validator.Property(m => m.SubmissionDate).IsNotDefault();
    }
}
