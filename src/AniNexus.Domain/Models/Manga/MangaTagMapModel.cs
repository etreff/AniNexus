using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping between a manga and its tags.
/// </summary>
public class MangaTagMapModel : IEntityTypeConfiguration<MangaTagMapModel>
{
    /// <summary>
    /// The Id of the manga.
    /// </summary>
    /// <seealso cref="MangaModel"/>
    public int MangaId { get; set; }

    /// <summary>
    /// The Id of the tag.
    /// </summary>
    /// <seealso cref="MediaTagModel"/>
    public int TagId { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The manga the tag should be applied to.
    /// </summary>
    public MangaModel Manga { get; set; } = default!;

    /// <summary>
    /// The tag to apply to the manga.
    /// </summary>
    public MediaTagModel Tag { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<MangaTagMapModel> builder)
    {
        builder.ToTable("MangaTagMap");

        builder.HasKey(m => new { m.MangaId, m.TagId });
        builder.HasIndex(m => m.TagId);

        builder.HasOne(m => m.Manga).WithMany(m => m.Tags).HasForeignKey(m => m.MangaId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Tag).WithMany(m => m.Manga).HasForeignKey(m => m.TagId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Manga).AutoInclude();
        builder.Navigation(m => m.Tag).AutoInclude();
    }
}
