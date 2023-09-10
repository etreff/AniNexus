using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.DataAccess.Entities.Configuration;

// Since this is a TPH type, we implement IEntityTypeConfiguration instead of inheriting BaseConfiguration.
// The base class is handling the BaseConfiguration for the shared properties.
internal sealed class SeriesAnimeEntityConfiguration : IEntityTypeConfiguration<SeriesAnimeEntity>
{
    public void Configure(EntityTypeBuilder<SeriesAnimeEntity> builder)
    {
        builder.HasIndex(m => m.CategoryId);
        builder.HasIndex(m => m.SeasonId).HasNotNullFilter();

        builder.HasOne(m => m.Category).WithMany().HasForeignKey(m => m.CategoryId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(m => m.Season).WithMany().HasForeignKey(m => m.SeasonId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
    }
}
