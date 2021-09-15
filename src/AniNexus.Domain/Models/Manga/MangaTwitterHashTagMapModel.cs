using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping between an manga and a Twitter hashtag.
/// </summary>
public class MangaTwitterHashTagMapModel : IEntityTypeConfiguration<MangaTwitterHashTagMapModel>
{
    /// <summary>
    /// The Id of the manga.
    /// </summary>
    /// <seealso cref="MangaModel"/>
    public int MangaId { get; set; }

    /// <summary>
    /// The Id of the hashtag.
    /// </summary>
    /// <seealso cref="TwitterHashTagModel"/>
    public int TwitterHashTagId { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The manga the hashtag applies to.
    /// </summary>
    public MangaModel Manga { get; set; } = default!;

    /// <summary>
    /// The hashtag that is applied to the manga.
    /// </summary>
    public TwitterHashTagModel TwitterHashTag { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<MangaTwitterHashTagMapModel> builder)
    {
        builder.ToTable("MangaTwitterHashTagMap");

        builder.HasKey(m => new { m.MangaId, m.TwitterHashTagId });
        builder.HasIndex(m => m.TwitterHashTagId);

        builder.HasOne(m => m.Manga).WithMany(m => m.TwitterHashtags).HasForeignKey(m => m.MangaId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.TwitterHashTag).WithMany().HasForeignKey(m => m.TwitterHashTagId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Manga).AutoInclude();
        builder.Navigation(m => m.TwitterHashTag).AutoInclude();
    }
}
