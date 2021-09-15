using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a key-value dictionary of application resources.
/// </summary>
public class ApplicationResourceModel : IEntityTypeConfiguration<ApplicationResourceModel>
{
    /// <summary>
    /// The name of the resource.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// The value of the resource.
    /// </summary>
    public string? Value { get; set; }

    public void Configure(EntityTypeBuilder<ApplicationResourceModel> builder)
    {
        builder.ToTable("AppResource");

        builder.HasKey(m => m.Name);

        builder.HasData(
            new ApplicationResourceModel { Name = "ContentHostKey", Value = "https://localhost:5001" },
            new ApplicationResourceModel { Name = "AnimeCoverArtPath", Value = "assets/coverart/anime/{0}.jpg" },
            new ApplicationResourceModel { Name = "MediaSeriesCoverArtPath", Value = "assets/coverart/series/{0}.jpg" },
            new ApplicationResourceModel { Name = "SoundTrackAlbumArtPath", Value = "assets/coverart/ost/{0}.jpg" },
            new ApplicationResourceModel { Name = "CharacterPicturePath", Value = "assets/coverart/character/{0}.jpg" },
            new ApplicationResourceModel { Name = "PersonPicturePath", Value = "assets/coverart/person/{0}.jpg" });

        builder.Property(m => m.Name).HasComment("The key of the dictionary.").ValueGeneratedNever();
        builder.Property(m => m.Value).HasComment("The value of the dictionary.");
    }
}
