using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a content tag that describes a piece of media.
/// </summary>
public class MediaTagModel : IHasAudit, IHasSoftDelete, IEntityTypeConfiguration<MediaTagModel>
{
    /// <summary>
    /// The Id of the tag.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The name of the tag.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// The Id of the user who created the tag.
    /// </summary>
    /// <seealso cref="UserModel"/>
    public Guid? UserId { get; set; }

    /// <summary>
    /// A description of the tag.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Whether the tag would be a spoiler for the media it is attached to.
    /// </summary>
    public bool IsSpoiler { get; set; }

    /// <summary>
    /// Whether the tag would mark a piece of media as not safe for work.
    /// </summary>
    public bool IsNSFW { get; set; }

    /// <summary>
    /// Whether the tag would mark a piece of media as containing gore.
    /// </summary>
    public bool IsGore { get; set; }

    #region Interface Properties
    /// <summary>
    /// The UTC date and time this entry was added to the table.
    /// </summary>
    public DateTime DateAdded { get; set; }

    /// <summary>
    /// The UTC date and time this entry was last updated.
    /// </summary>
    public DateTime DateUpdated { get; set; }

    /// <summary>
    /// Whether this entry is soft-deleted. It will not be included in queries unless
    /// <see cref="M:Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.IgnoreQueryFilters``1(System.Linq.IQueryable{``0})" />
    /// is invoked.
    /// </summary>
    public bool IsSoftDeleted { get; set; }
    #endregion

    #region Navigation Properties
    /// <summary>
    /// The user who created the tag.
    /// </summary>
    public UserModel? User { get; set; }

    /// <summary>
    /// The anime that contain this tag.
    /// </summary>
    public IList<AnimeTagMapModel> Anime { get; set; } = default!;

    /// <summary>
    /// The games that contain this tag.
    /// </summary>
    public IList<GameTagMapModel> Games { get; set; } = default!;

    /// <summary>
    /// The manga that contain this tag.
    /// </summary>
    public IList<MangaTagMapModel> Manga { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<MediaTagModel> builder)
    {
        builder.ToTable("MediaTag");

        builder.HasKey(m => m.Id);
        builder.HasIndex(m => m.Name).IsUnique();

        builder.HasOne(m => m.User).WithMany().HasForeignKey(m => m.UserId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);

        builder.Property(m => m.Name).HasComment("The name of the tag.").HasMaxLength(32);
        builder.Property(m => m.Description).HasComment("A short description of the tag.").HasMaxLength(200);
        builder.Property(m => m.IsSpoiler).HasComment("Whether this tag is a spoiler for an event in the media.");
        builder.Property(m => m.IsNSFW).HasComment("Whether this tag marks an entity as NSFW.").IsRequired();
        builder.Property(m => m.IsGore).HasComment("Whether this tag marks an entity as containing gore.");
    }
}
