using AniNexus.Domain.Converters;
using AniNexus.Domain.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AniNexus.Domain;
public partial class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUserModel>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IOptions<OperationalStoreOptions> operationalStoreOptions)
        : base(options, operationalStoreOptions)
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
            .HaveColumnType("char(10)");

        builder.Properties<Date?>()
            .HaveConversion<NullableDateConverter, NullableDateComparer>()
            .HaveColumnType("char(10)");

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