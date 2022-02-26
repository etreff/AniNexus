//using System.ComponentModel.DataAnnotations;
//using System.Text.RegularExpressions;
//using AniNexus.Domain.Validation;

//namespace AniNexus.Domain.Entities;

///// <summary>
///// Models a Twitter hashtag.
///// </summary>
//public sealed class TwitterHashTagEntity : Entity<TwitterHashTagEntity>
//{
//    /// <summary>
//    /// The hashtag without the '#'.
//    /// </summary>
//    public byte[] Hashtag { get; set; } = default!;

//    /// <summary>
//    /// Gets a string representation of <see cref="Hashtag"/>.
//    /// </summary>
//    public string GetHashtag()
//    {
//        return ConvertToString(Hashtag);
//    }

//    /// <summary>
//    /// Sets the value of <see cref="Hashtag"/>.
//    /// </summary>
//    public void SetHashtag(string value)
//    {
//        Hashtag = ConvertToBytes(value);
//    }

//    /// <inheritdoc/>
//    protected override void ConfigureEntity(EntityTypeBuilder<TwitterHashTagEntity> builder)
//    {
//        // 1. Index specification
//        builder.HasIndex(m => m.Hashtag).IsUnique();
//        // 2. Navigation properties
//        // 3. Propery specification
//        builder.Property(m => m.Hashtag).HasComment("The Twitter hashtag.").HasMaxLength(280).IsUnicode().UseCollation(Collation.Japanese);
//        // 4. Other
//    }

//    /// <inheritdoc/>
//    protected override void Validate(ValidationContext validationContext, TwitterHashTagEntity entity, ValidationBuilder<TwitterHashTagEntity> validator)
//    {
//        base.Validate(validationContext, entity, validator);

//        if (!IsValidHashtag(entity.GetHashtag()))
//        {
//            validator.AddValidationResult(new ValidationResult("The hashtag is not valid.", new[] { nameof(Hashtag) }));
//        }
//    }

//    // https://gist.github.com/janogarcia/3946583
//    private static readonly Regex _hashtagRegexValidator = new(@"^(?=.{2,140}$)(?:#|\uff03)([0-9_\p{L}]*[_\p{L}][0-9_\p{L}]*)$", RegexOptions.Compiled);

//    private static bool IsValidHashtag(string hashtag)
//    {
//        return _hashtagRegexValidator.IsMatch(hashtag);
//    }
//}
