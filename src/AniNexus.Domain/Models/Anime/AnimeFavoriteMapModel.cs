using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a map of users and the anime they have favorited.
/// </summary>
public class AnimeFavoriteMapModel : IEntityTypeConfiguration<AnimeFavoriteMapModel>
{
    /// <summary>
    /// The Id of the anime.
    /// </summary>
    /// <seealso cref="AnimeModel"/>
    public int AnimeId { get; set; }

    /// <summary>
    /// The Id of the user who favorited the anime.
    /// </summary>
    /// <seealso cref="ApplicationUserModel"/>
    public string UserId { get; set; } = default!;

    #region Navigation Properties
    /// <summary>
    /// The anime that got favorited.
    /// </summary>
    public AnimeModel Anime { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<AnimeFavoriteMapModel> builder)
    {
        builder.ToTable("AnimeFavoriteMap");

        builder.HasKey(m => new { m.AnimeId, m.UserId });
        builder.HasIndex(m => m.UserId);
            
        builder.HasOne(m => m.Anime).WithMany(m => m.Favorites).HasForeignKey(m => m.AnimeId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Anime).AutoInclude();
    }
}
