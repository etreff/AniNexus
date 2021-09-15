using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping between an anime and a third party tracker.
/// </summary>
public class AnimeThirdPartyMapModel : IEntityTypeConfiguration<AnimeThirdPartyMapModel>
{
    /// <summary>
    /// The AniNexus anime Id.
    /// </summary>
    /// <seealso cref="AnimeModel"/>
    public int AnimeId { get; set; }

    /// <summary>
    /// The Id of the third party tracker.
    /// </summary>
    /// <seealso cref="ThirdPartyModel"/>
    public int ThirdPartyId { get; set; }

    /// <summary>
    /// The Id the third party tracker has assigned this anime.
    /// </summary>
    public string ExternalMediaId { get; set; } = default!;

    #region Navigation Properties
    /// <summary>
    /// The anime that the external media Id refers to.
    /// </summary>
    public AnimeModel Anime { get; set; } = default!;

    /// <summary>
    /// The third party tracker.
    /// </summary>
    public ThirdPartyModel ThirdParty { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<AnimeThirdPartyMapModel> builder)
    {
        builder.ToTable("AnimeThirdPartyMap");

        builder.HasKey(m => new { m.AnimeId, m.ThirdPartyId });
        builder.HasIndex(m => m.ThirdPartyId);

        builder.HasOne(m => m.Anime).WithMany(m => m.ExternalIds).HasForeignKey(m => m.AnimeId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.ThirdParty).WithMany().HasForeignKey(m => m.ThirdPartyId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Anime).AutoInclude();
        builder.Navigation(m => m.ThirdParty).AutoInclude();

        builder.Property(m => m.ExternalMediaId).HasComment("The Id that the third party tracker has assigned to the anime entry.");
    }
}
