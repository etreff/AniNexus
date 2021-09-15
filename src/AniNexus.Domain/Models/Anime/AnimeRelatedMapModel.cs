using AniNexus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping between related anime entries.
/// </summary>
public class AnimeRelatedMapModel : IEntityTypeConfiguration<AnimeRelatedMapModel>
{
    /// <summary>
    /// The Id of the anime.
    /// </summary>
    /// <seealso cref="AnimeModel"/>
    public int AnimeId { get; set; }

    /// <summary>
    /// The Id of the related anime.
    /// </summary>
    /// <seealso cref="AnimeModel"/>
    public int RelatedAnimeId { get; set; }

    /// <summary>
    /// The Id of the relation type.
    /// </summary>
    /// <seealso cref="EMediaRelationType"/>
    /// <seealso cref="MediaRelationTypeModel"/>
    public int RelationTypeId { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The anime.
    /// </summary>
    public AnimeModel Anime { get; set; } = default!;

    /// <summary>
    /// The anime that is related.
    /// </summary>
    public AnimeModel Related { get; set; } = default!;

    /// <summary>
    /// How the anime is related.
    /// </summary>
    public MediaRelationTypeModel RelationType { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<AnimeRelatedMapModel> builder)
    {
        builder.ToTable("AnimeRelatedMap");

        builder.HasKey(m => new { m.AnimeId, m.RelatedAnimeId });
        builder.HasIndex(m => m.RelatedAnimeId);
        builder.HasIndex(m => m.RelationTypeId);

        builder.HasOne(m => m.Anime).WithMany(m => m.Related).HasForeignKey(m => m.AnimeId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        // Handled by EF trigger - SqlServer will falsely flag multiple cascade paths since the mapping
        // points to two entities in the same table.
        builder.HasOne(m => m.Related).WithMany().HasForeignKey(m => m.RelatedAnimeId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(m => m.RelationType).WithMany().HasForeignKey(m => m.RelatedAnimeId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Anime).AutoInclude();
        builder.Navigation(m => m.Related).AutoInclude();
        builder.Navigation(m => m.RelationType).AutoInclude();
    }
}
