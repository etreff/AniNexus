using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a record that identifies an <see cref="AnimeReleaseModel"/> that is currently airing.
/// </summary>
public class AnimeReleaseAiringModel : IEntityTypeConfiguration<AnimeReleaseAiringModel>
{
    /// <summary>
    /// The Id of the anime release that is airing.
    /// </summary>
    /// <seealso cref="AnimeReleaseModel"/>
    public int ReleaseId { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The anime release that is airing.
    /// </summary>
    public AnimeReleaseModel Release { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<AnimeReleaseAiringModel> builder)
    {
        builder.ToTable("AnimeReleaseAiring");

        builder.HasKey(m => m.ReleaseId);

        builder.HasOne(m => m.Release).WithOne().HasForeignKey<AnimeReleaseAiringModel>(m => m.ReleaseId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - When accessing this information we will ultimately need the anime information to display
        // to the user.
        builder.Navigation(m => m.Release).AutoInclude();

        builder.Property(m => m.ReleaseId).ValueGeneratedNever();
    }
}
