using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.DataAccess.Entities.Configuration;

// Since this is a TPH type, we implement IEntityTypeConfiguration instead of inheriting BaseConfiguration.
// The base class is handling the BaseConfiguration for the shared properties.
internal sealed class SeriesMangaEntityConfiguration : IEntityTypeConfiguration<SeriesMangaEntity>
{
    public void Configure(EntityTypeBuilder<SeriesMangaEntity> builder)
    {
        builder.HasIndex(m => m.CategoryId);

        builder.HasOne(m => m.Category).WithMany().HasForeignKey(m => m.CategoryId).IsRequired().OnDelete(DeleteBehavior.Restrict);
    }
}
