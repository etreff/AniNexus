using System.Reflection;
using AniNexus.DataAccess.Conventions;
using AniNexus.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace AniNexus.DataAccess;

/// <summary>
/// The AniNexus database context.
/// </summary>
public sealed partial class AniNexusDbContext : DbContext
{
    private static readonly MethodInfo _applyConfigurationMethodInfo = typeof(ModelBuilder).GetMethod(nameof(ModelBuilder.ApplyConfiguration))!;

    /// <summary>
    /// Third-party API access tokens.
    /// </summary>
    public DbSet<AccessTokenEntity> AccessTokens { get; set; }

    /// <summary>
    /// Media entries.
    /// </summary>
    public DbSet<SeriesMediaEntity> Media { get; set; }

    /// <summary>
    /// Anime media entries.
    /// </summary>
    public DbSet<SeriesAnimeEntity> AnimeMedia { get; set; }

    /// <summary>
    /// Manga media entries.
    /// </summary>
    public DbSet<SeriesMangaEntity> MangaMedia { get; set; }

    /// <summary>
    /// Roles that can be assigned to a team.
    /// </summary>
    public DbSet<AuthTeamRoleEntity> TeamRoles { get; set; }

    /// <summary>
    /// Users.
    /// </summary>
    public DbSet<UserEntity> Users { get; set; }

    /// <summary>
    /// Mappings of teams to roles.
    /// </summary>
    public DbSet<AuthTeamRoleMapEntity> UserTeamRoleMappings { get; set; }

    /// <summary>
    /// User teams.
    /// </summary>
    public DbSet<AuthTeamEntity> UserTeams { get; set; }

    /// <summary>
    /// Mappings of users to teams.
    /// </summary>
    public DbSet<AuthTeamUserMapEntity> UserTeamMappings { get; set; }

    /// <summary>
    /// User ban information.
    /// </summary>
    public DbSet<UserBannedEntity> UserBans { get; set; }

    /// <summary>
    /// Creates a new <see cref="AniNexusDbContext"/> instance with the specified options.
    /// </summary>
    /// <param name="options">The database options.</param>
    public AniNexusDbContext(DbContextOptions<AniNexusDbContext> options)
        : base(options)
    {
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation(Collation.CaseInsensitive);

        var entityConventions = ConventionProvider.CreateEntityConventions();
        var preConventions = entityConventions.OfType<IPreConfigureEntityConvention>().ToArray();
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var convention in preConventions)
            {
                convention.PreConfigure(modelBuilder, entity);
            }
        }

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AniNexusDbContext).Assembly);

        base.OnModelCreating(modelBuilder);

        var postConventions = entityConventions.OfType<IPostConfigureEntityConvention>().ToArray();
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var convention in postConventions)
            {
                convention.PostConfigure(modelBuilder, entity);
            }
        }
    }

    /// <inheritdoc />
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        foreach (var typeConvention in ConventionProvider.CreateTypeConventions())
        {
            typeConvention.Configure(configurationBuilder);
        }

        base.ConfigureConventions(configurationBuilder);
    }
}
