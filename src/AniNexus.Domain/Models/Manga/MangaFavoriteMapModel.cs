using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a map of users and the manga they have favorited.
/// </summary>
public class MangaFavoriteMapModel : IEntityTypeConfiguration<MangaFavoriteMapModel>
{
    /// <summary>
    /// The Id of the manga.
    /// </summary>
    /// <seealso cref="MangaModel"/>
    public int MangaId { get; set; }

    /// <summary>
    /// The Id of the user who favorited the manga.
    /// </summary>
    /// <seealso cref="ApplicationUserModel"/>
    public string UserId { get; set; } = default!;

    #region Navigation Properties
    /// <summary>
    /// The manga that got favorited.
    /// </summary>
    public MangaModel Manga { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<MangaFavoriteMapModel> builder)
    {
        builder.ToTable("MangaFavoriteMap");

        builder.HasKey(m => new { m.MangaId, m.UserId });
        builder.HasIndex(m => m.UserId);

        builder.HasOne(m => m.Manga).WithMany(m => m.Favorites).HasForeignKey(m => m.MangaId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Manga).AutoInclude();
    }
}
