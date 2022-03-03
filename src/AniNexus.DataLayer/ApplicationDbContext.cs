using AniNexus.Data.Conventions;
using AniNexus.Data.Entities;

namespace AniNexus.Data;

/// <summary>
/// The application <see cref="DbContext"/>.
/// </summary>
public partial class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Creates a new <see cref="ApplicationDbContext"/> instance.
    /// </summary>
    /// <param name="options"></param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <inheritdoc/>
    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        var conventionTypes = ConventionProvider.GetTypeConventions();
        foreach (var convention in ConventionProvider.CreateTypeConventions(conventionTypes))
        {
            convention.Configure(builder);
        }

        base.ConfigureConventions(builder);
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        var conventionTypes = ConventionProvider.GetEntityConventions();
        var conventions = ConventionProvider.CreateTypeConventions(conventionTypes);

        var preConventions = conventions.OfType<IPreConfigureEntityConvention>().ToArray();
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            foreach (var convention in preConventions)
            {
                convention.PreConfigure(builder, entity);
            }
        }

        builder.ApplyConfigurationsFromAssembly(typeof(IEntity).Assembly);

        base.OnModelCreating(builder);

        var postConventions = conventions.OfType<IPostConfigureEntityConvention>().ToArray();
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            foreach (var convention in postConventions)
            {
                convention.PostConfigure(builder, entity);
            }
        }
    }

    /// <summary>
    /// Returns entities that are soft deleted.
    /// </summary>
    /// <typeparam name="TEntity">The soft-deletable entity type.</typeparam>
    /// <returns>A query to further filter results.</returns>
    public IQueryable<TEntity> GetSoftDeletedEntities<TEntity>()
        where TEntity : class, IHasSoftDelete
    {
        return Set<TEntity>().IgnoreQueryFilters().Where(m => m.IsSoftDeleted);
    }
}
