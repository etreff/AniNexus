using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping between an anime and a Twitter hashtag.
/// </summary>
public class AnimeTwitterHashTagMapModel : IEntityTypeConfiguration<AnimeTwitterHashTagMapModel>
{
    /// <summary>
    /// The Id of the anime.
    /// </summary>
    /// <seealso cref="AnimeModel"/>
    public int AnimeId { get; set; }

    /// <summary>
    /// The Id of the hashtag.
    /// </summary>
    /// <seealso cref="TwitterHashTagModel"/>
    public int TwitterHashTagId { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The anime the hashtag applies to.
    /// </summary>
    public AnimeModel Anime { get; set; } = default!;

    /// <summary>
    /// The hashtag that is applied to the anime.
    /// </summary>
    public TwitterHashTagModel TwitterHashTag { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<AnimeTwitterHashTagMapModel> builder)
    {
        builder.ToTable("AnimeTwitterHashTagMap");

        builder.HasKey(m => new { m.AnimeId, m.TwitterHashTagId });
        builder.HasIndex(m => m.TwitterHashTagId);

        builder.HasOne(m => m.Anime).WithMany(m => m.TwitterHashtags).HasForeignKey(m => m.AnimeId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.TwitterHashTag).WithMany().HasForeignKey(m => m.TwitterHashTagId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Anime).AutoInclude();
        builder.Navigation(m => m.TwitterHashTag).AutoInclude();
    }
}
