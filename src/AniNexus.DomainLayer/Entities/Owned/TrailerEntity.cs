using AniNexus.Domain.Validation;
using Microsoft.Toolkit.Diagnostics;

namespace AniNexus.Domain.Entities;

/// <summary>
/// Models a media trailer.
/// </summary>
public sealed class TrailerEntity : IOwnedEntity<TrailerEntity>
{
    /// <summary>
    /// The Id of the owner of this entity.
    /// </summary>
    public Guid OwnerId { get; set; }

    /// <summary>
    /// A link to the trailer.
    /// </summary>
    /// <remarks>
    /// This URL must be valid.
    /// </remarks>
    public string ResourceUrl { get; set; } = default!;

    /// <inheritdoc/>
    public void Validate(ValidationBuilder<TrailerEntity> validator)
    {
        validator.Property(m => m.ResourceUrl).IsValidUrl();
    }
}

/// <summary>
/// <see cref="OwnedNavigationBuilder"/> extensions.
/// </summary>
public static partial class OwnedNavigationBuilderExtensions
{
    /// <summary>
    /// Configures the <see cref="TrailerEntity"/> owned type for this entity.
    /// </summary>
    /// <typeparam name="TEntity">The owning type.</typeparam>
    /// <param name="builder">The owned type builder.</param>
    /// <param name="createTable">Whether to create table.</param>
    /// <param name="tableName">The table name to use. If left <see langword="null"/>, <see cref="Entity.GetDefaultTableName{TEntity}"/> will be used.</param>
    public static OwnedNavigationBuilder<TEntity, TrailerEntity> ConfigureTrailerEntity<TEntity>(this OwnedNavigationBuilder<TEntity, TrailerEntity> builder, bool createTable = true, string? tableName = null)
        where TEntity : class
    {
        Guard.IsNotNull(builder, nameof(builder));

        if (createTable)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                tableName = Entity.GetDefaultTableName<TEntity>();
            }
            builder.ToTable($"{tableName}Name");
        }

        builder.WithOwner().HasForeignKey(m => m.OwnerId);

        builder.Property(m => m.ResourceUrl).HasComment("A link to the trailer.");

        return builder;
    }
}
