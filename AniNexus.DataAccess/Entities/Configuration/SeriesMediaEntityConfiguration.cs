using AniNexus.DataAccess.Entities.Owned;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.DataAccess.Entities.Configuration;

internal sealed class SeriesMediaEntityConfiguration : BaseConfiguration<SeriesMediaEntity, int>
{
    public override void Configure(EntityTypeBuilder<SeriesMediaEntity> builder)
    {
        base.Configure(builder);

        // https://www.learnentityframeworkcore.com/inheritance/table-per-hierarchy
        builder
            .HasDiscriminator<int>("MediaType")
            .HasValue<SeriesAnimeEntity>(1)
            .HasValue<SeriesMangaEntity>(2);

        builder.OwnsOne(m => m.Name, OwnedEntity.ConfigureInline);

        builder.Property(m => m.ActiveRating).HasComment("The user rating of the media (Watching + Completed), from 0 to 100. Calculated by the system periodically.");
        builder.Property(m => m.Rating).HasComment("The user rating of the media (Completed Only), from 0 to 100. Calculated by the system periodically.");
        builder.Property(m => m.Synopsis).HasMaxLength(2000).HasComment("A synopsis or description of the media.");
        builder.Property(m => m.Votes).HasComment("The number of votes that contributed to the rating. Calculated by the system periodically.").HasDefaultValue(0);
        builder.Property(m => m.WebsiteUrl).HasComment("The URL to the media's official website.");
    }

    protected override string GetTableName()
    {
        return "Series";
    }
}
