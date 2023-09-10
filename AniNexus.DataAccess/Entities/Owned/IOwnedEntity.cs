using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.DataAccess.Entities.Owned;

/// <summary>
/// Defines an owned entity type.
/// </summary>
public interface IOwnedEntity<TOwnedEntity>
    where TOwnedEntity : class, IOwnedEntity<TOwnedEntity>
{
    /// <summary>
    /// Configures the ownership of this entity.
    /// </summary>
    /// <param name="builder">The owned entity navigation builder.</param>
    void ConfigureOwnership<TOwner>(OwnedNavigationBuilder<TOwner, TOwnedEntity> builder)
        where TOwner : class;
}
