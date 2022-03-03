using Microsoft.Toolkit.Diagnostics;

namespace AniNexus.Data;

/// <summary>
/// <see cref="IndexBuilder{T}"/> extensions.
/// </summary>
public static class IndexBuilderExtensions
{
    /// <summary>
    /// Applies an index to the property only if the value is not null.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity this index is being applied to.</typeparam>
    /// <param name="builder">The index builder.</param>
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null"/>.</exception>
    public static IndexBuilder<TEntity> HasNotNullFilter<TEntity>(this IndexBuilder<TEntity> builder)
    {
        Guard.IsNotNull(builder, nameof(builder));

        string appliedProperty = builder.Metadata.Properties.Single().Name;
        return builder.HasFilter($"[{appliedProperty}] IS NOT NULL");
    }
}
