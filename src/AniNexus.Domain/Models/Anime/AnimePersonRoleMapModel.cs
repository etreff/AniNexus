using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping between a person and the roles that the person has played in the creation of an anime.
/// </summary>
/// <remarks>
/// Voice actors or live-action actors are not included in this collection.
/// </remarks>
public class AnimePersonRoleMapModel : IEntityTypeConfiguration<AnimePersonRoleMapModel>
{
    /// <summary>
    /// The Id of the person who played a role in the creation of the anime.
    /// </summary>
    /// <seealso cref="MediaPersonModel"/>
    public int PersonId { get; set; }

    /// <summary>
    /// The Id of the anime the person played a role in creating.
    /// </summary>
    /// <seealso cref="AnimeModel"/>
    public int AnimeId { get; set; }

    /// <summary>
    /// The Id of the role the person played in the creation of the anime.
    /// </summary>
    /// <seealso cref="EPersonRole"/>
    /// <seealso cref="PersonRoleTypeModel"/>
    public int RoleId { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The person who played a role in the creation of the anime.
    /// </summary>
    public MediaPersonModel Person { get; set; } = default!;

    /// <summary>
    /// The anime the person played a role in creating.
    /// </summary>
    public AnimeModel Anime { get; set; } = default!;

    /// <summary>
    /// The role the person played in the creation of the anime.
    /// </summary>
    public PersonRoleTypeModel Role { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<AnimePersonRoleMapModel> builder)
    {
        builder.ToTable("AnimePersonRoleMap");

        builder.HasKey(m => new { m.PersonId, m.AnimeId, m.RoleId });
        builder.HasIndex(m => m.AnimeId);
        builder.HasIndex(m => m.RoleId);

        builder.HasOne(m => m.Person).WithMany(m => m.AnimeRoles).HasForeignKey(m => m.PersonId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Anime).WithMany(m => m.People).HasForeignKey(m => m.AnimeId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Role).WithMany().HasForeignKey(m => m.RoleId).IsRequired().OnDelete(DeleteBehavior.Restrict);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Anime).AutoInclude();
        builder.Navigation(m => m.Person).AutoInclude();
        builder.Navigation(m => m.Role).AutoInclude();
    }
}
