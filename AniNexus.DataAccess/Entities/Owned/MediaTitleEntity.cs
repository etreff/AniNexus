using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.DataAccess.Entities.Owned;

/// <summary>
/// Models the name(s) of a piece of media.
/// </summary>
/// <remarks>
/// This entity should be configured with <see cref="OwnedEntity.ConfigureInline{TEntity, TRelatedEntity}(OwnedNavigationBuilder{TEntity, TRelatedEntity})"/>
/// or <see cref="OwnedEntity.ConfigureMany{TEntity, TRelatedEntity, TEntityKey}(OwnedNavigationBuilder{TEntity, TRelatedEntity})"/>.
/// </remarks>
public sealed class MediaTitleEntity : IOwnedEntity<MediaTitleEntity>
{
    /// <inheritdoc />
    public string NativeName { get; set; } = default!;

    /// <inheritdoc />
    public string? RomajiName { get; set; }

    /// <inheritdoc />
    public string? EnglishName { get; set; }

    /// <summary>
    /// Configures the ownership of this <see cref="MediaTitleEntity"/> using the specified navigation builder.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owning model.</typeparam>
    /// <param name="builder">The navigation builder.</param>
    public void ConfigureOwnership<TOwner>(OwnedNavigationBuilder<TOwner, MediaTitleEntity> builder)
        where TOwner : class
    {
        builder.Property(m => m.NativeName).HasComment("The native name.").HasColumnName(nameof(NativeName));
        builder.Property(m => m.RomajiName).HasComment("The Romanization of the native name.").HasColumnName(nameof(RomajiName));
        builder.Property(m => m.EnglishName).HasComment("The name in English.").HasColumnName(nameof(EnglishName));
    }
}
