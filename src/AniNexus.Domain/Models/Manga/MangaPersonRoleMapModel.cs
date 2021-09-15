using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping between a person and the roles that the person has played in an manga.
/// </summary>
/// <remarks>
/// Voice actors or live-action actors are not included in this collection.
/// </remarks>
public class MangaPersonRoleMapModel : IEntityTypeConfiguration<MangaPersonRoleMapModel>
{
    /// <summary>
    /// The person Id.
    /// </summary>
    /// <seealso cref="MediaPersonModel"/>
    public int PersonId { get; set; }

    /// <summary>
    /// The manga Id.
    /// </summary>
    /// <seealso cref="MangaModel"/>
    public int MangaId { get; set; }

    /// <summary>
    /// The role Id.
    /// </summary>
    /// <seealso cref="EPersonRole"/>
    /// <seealso cref="PersonRoleTypeModel"/>
    public int RoleId { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The creator.
    /// </summary>
    public MediaPersonModel Person { get; set; } = default!;

    /// <summary>
    /// The manga.
    /// </summary>
    public MangaModel Manga { get; set; } = default!;

    /// <summary>
    /// The role the person played in the manga.
    /// </summary>
    public PersonRoleTypeModel Role { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<MangaPersonRoleMapModel> builder)
    {
        builder.ToTable("MangaPersonRoleMap");

        builder.HasKey(m => new { m.PersonId, m.MangaId, m.RoleId });
        builder.HasKey(m => m.MangaId);
        builder.HasKey(m => m.RoleId);

        builder.HasOne(m => m.Person).WithMany(m => m.MangaRoles).HasForeignKey(m => m.PersonId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Manga).WithMany(m => m.People).HasForeignKey(m => m.MangaId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Role).WithMany().HasForeignKey(m => m.RoleId).IsRequired().OnDelete(DeleteBehavior.Restrict);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Manga).AutoInclude();
        builder.Navigation(m => m.Person).AutoInclude();
        builder.Navigation(m => m.Role).AutoInclude();
    }
}
