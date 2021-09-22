using AniNexus.Domain.Converters;
using AniNexus.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AniNexus.Domain;
public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        builder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter, DateOnlyComparer>()
            .HaveColumnType("date");

        builder.Properties<DateOnly?>()
            .HaveConversion<NullableDateOnlyConverter, NullableDateOnlyComparer>()
            .HaveColumnType("date");

        // YYYY.MM.DD
        builder.Properties<Date>()
            .HaveConversion<DateConverter, DateComparer>()
            .HaveColumnType("char(10)")
            .AreFixedLength(true);

        builder.Properties<Date?>()
            .HaveConversion<NullableDateConverter, NullableDateComparer>()
            .HaveColumnType("char(10)")
            .AreFixedLength(true);

        builder.Properties<Guid>()
            .HaveConversion<GuidConverter, GuidComparer>()
            .HaveColumnType("char(36)")
            .AreFixedLength(true);

        builder.Properties<Guid?>()
            .HaveConversion<NullableGuidConverter, NullableGuidComparer>()
            .HaveColumnType("char(36)")
            .AreFixedLength(true);

        base.ConfigureConventions(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        AutoConfigureProperties(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AnimeModel).Assembly);

        base.OnModelCreating(builder);

        FixSoftDeleteQueryFiltersOnNavigationProperties(builder);
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
