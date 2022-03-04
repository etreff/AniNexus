using System.ComponentModel.DataAnnotations;
using AniNexus.Data.Validation;

namespace AniNexus.Data.Entities;

/// <summary>
/// Models social media information.
/// </summary>
public sealed class SocialMediaEntity : Entity<SocialMediaEntity>, IHasImage
{
    /// <summary>
    /// The name of the social media entity.
    /// </summary>
    public NameEntity Name { get; set; } = default!;

    /// <summary>
    /// A link to the social media's website.
    /// </summary>
    public string WebsiteUrl { get; set; } = default!;

    /// <inheritdoc/>
    public Guid? ImageId { get; set; }

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<SocialMediaEntity> builder)
    {
        // 1. Index specification
        // 2. Navigation properties
        builder.OwnsOne(m => m.Name, static name => name.ConfigureOwnedEntity());
        // 3. Propery specification
        // 4. Other
    }

    /// <inheritdoc/>
    protected override void Validate(ValidationContext validationContext, ValidationBuilder<SocialMediaEntity> validator)
    {
        base.Validate(validationContext, validator);

        validator.Property(m => m.WebsiteUrl).IsValidUrl();
    }

    /// <inheritdoc/>
    protected override IEnumerable<SocialMediaEntity> GetSeedData()
    {
        return new[]
        {
            new SocialMediaEntity { WebsiteUrl = "www.facebook.com", Name = { EnglishName = "Facebook" }},
            new SocialMediaEntity { WebsiteUrl = "www.twitter.com", Name = { EnglishName = "Twitter" }}
        };
    }
}
