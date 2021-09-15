using AniNexus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping between related manga entries.
/// </summary>
public class MangaRelatedMapModel : IEntityTypeConfiguration<MangaRelatedMapModel>
{
    /// <summary>
    /// The Id of the manga.
    /// </summary>
    /// <seealso cref="MangaModel"/>
    public int MangaId { get; set; }

    /// <summary>
    /// The Id of the related manga.
    /// </summary>
    /// <seealso cref="MangaModel"/>
    public int RelatedMangaId { get; set; }

    /// <summary>
    /// The Id of the relation type.
    /// </summary>
    /// <seealso cref="EMediaRelationType"/>
    /// <seealso cref="MediaRelationTypeModel"/>
    public int RelationTypeId { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The manga.
    /// </summary>
    public MangaModel Manga { get; set; } = default!;

    /// <summary>
    /// The manga that is related.
    /// </summary>
    public MangaModel Related { get; set; } = default!;

    /// <summary>
    /// How the anime is related.
    /// </summary>
    public MediaRelationTypeModel RelationType { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<MangaRelatedMapModel> builder)
    {
        builder.ToTable("MangaRelatedMap");

        builder.HasKey(m => new { m.MangaId, m.RelatedMangaId });
        builder.HasIndex(m => m.RelatedMangaId);
        builder.HasIndex(m => m.RelationTypeId);

        builder.HasOne(m => m.Manga).WithMany(m => m.Related).HasForeignKey(m => m.MangaId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        // Handled by EF trigger - SqlServer will falsely flag multiple cascade paths since the mapping
        // points to two entities in the same table.
        builder.HasOne(m => m.Related).WithMany().HasForeignKey(m => m.RelatedMangaId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(m => m.RelationType).WithMany().HasForeignKey(m => m.RelatedMangaId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Manga).AutoInclude();
        builder.Navigation(m => m.Related).AutoInclude();
        builder.Navigation(m => m.RelationType).AutoInclude();
    }
}
