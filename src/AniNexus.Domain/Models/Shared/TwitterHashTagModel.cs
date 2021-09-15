using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a Twitter hashtag.
/// </summary>
public class TwitterHashTagModel : IEntityTypeConfiguration<TwitterHashTagModel>, IValidatableObject
{
    /// <summary>
    /// The Id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The hashtag without the '#'.
    /// </summary>
    public string Hashtag { get; set; } = default!;

    public void Configure(EntityTypeBuilder<TwitterHashTagModel> builder)
    {
        builder.ToTable("TwitterHashTag");

        builder.HasKey(m => m.Id);
        builder.HasIndex(m => m.Hashtag).IsUnique();

        builder.Property(m => m.Hashtag).HasComment("The Twitter hashtag.").HasMaxLength(240).UseCollation(Collation.Japanese);
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!IsValidHashtag(Hashtag))
        {
            yield return new ValidationResult("The hashtag is not valid.", new[] { nameof(Hashtag) });
        }
    }

    // https://gist.github.com/janogarcia/3946583
    private static readonly Regex HashtagRegexValidator = new (@"^(?=.{2,140}$)(?:#|\uff03)([0-9_\p{L}]*[_\p{L}][0-9_\p{L}]*)$", RegexOptions.Compiled);

    private static bool IsValidHashtag(string hashtag)
    {
        return HashtagRegexValidator.IsMatch(hashtag);
    }
}
