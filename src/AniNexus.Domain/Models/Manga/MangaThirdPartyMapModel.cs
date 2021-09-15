using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping between an manga and a third party tracker.
/// </summary>
public class MangaThirdPartyMapModel : IEntityTypeConfiguration<MangaThirdPartyMapModel>
{
    /// <summary>
    /// The AniNexus manga Id.
    /// </summary>
    /// <seealso cref="MangaModel"/>
    public int MangaId { get; set; }

    /// <summary>
    /// The Id of the third party tracker.
    /// </summary>
    /// <seealso cref="ThirdPartyModel"/>
    public int ThirdPartyId { get; set; }

    /// <summary>
    /// The Id the third party tracker has assigned this manga.
    /// </summary>
    public string ExternalMediaId { get; set; } = default!;

    #region Navigation Properties
    /// <summary>
    /// The manga that the external media Id refers to.
    /// </summary>
    public MangaModel Manga { get; set; } = default!;

    /// <summary>
    /// The third party tracker.
    /// </summary>
    public ThirdPartyModel ThirdParty { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<MangaThirdPartyMapModel> builder)
    {
        builder.ToTable("MangaThirdPartyMap");

        builder.HasKey(m => new { m.MangaId, m.ThirdPartyId });
        builder.HasIndex(m => m.ThirdPartyId);

        builder.HasOne(m => m.Manga).WithMany(m => m.ExternalIds).HasForeignKey(m => m.MangaId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.ThirdParty).WithMany().HasForeignKey(m => m.ThirdPartyId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Manga).AutoInclude();
        builder.Navigation(m => m.ThirdParty).AutoInclude();

        builder.Property(m => m.ExternalMediaId).HasComment("The Id that the third party tracker has assigned to the manga entry.");
    }
}
