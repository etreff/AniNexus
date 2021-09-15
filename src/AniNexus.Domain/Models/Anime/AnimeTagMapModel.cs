using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping between an anime and its tags.
/// </summary>
public class AnimeTagMapModel : IEntityTypeConfiguration<AnimeTagMapModel>
{
    /// <summary>
    /// The Id of the anime that the tag should be applied to.
    /// </summary>
    /// <seealso cref="AnimeModel"/>
    public int AnimeId { get; set; }

    /// <summary>
    /// The Id of the tag to apply to the anime.
    /// </summary>
    /// <seealso cref="MediaTagModel"/>
    public int TagId { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The anime the tag should be applied to.
    /// </summary>
    public AnimeModel Anime { get; set; } = default!;

    /// <summary>
    /// The tag to apply to the anime.
    /// </summary>
    public MediaTagModel Tag { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<AnimeTagMapModel> builder)
    {
        builder.ToTable("AnimeTagMap");

        builder.HasKey(m => new { m.AnimeId, m.TagId });
        builder.HasIndex(m => m.TagId);

        builder.HasOne(m => m.Anime).WithMany(m => m.Tags).HasForeignKey(m => m.AnimeId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Tag).WithMany(m => m.Anime).HasForeignKey(m => m.TagId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Anime).AutoInclude();
        builder.Navigation(m => m.Tag).AutoInclude();
    }
}
