namespace AniNexus.Data.Entities;

/// <summary>
/// Models a key-value dictionary of application resources.
/// </summary>
public sealed class AppResourceEntity : AuditableEntity<AppResourceEntity>
{
    /// <summary>
    /// The name of the resource.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// The value of the resource.
    /// </summary>
    public string? Value { get; set; }

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<AppResourceEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(e => e.Name);
        // 2. Navigation properties
        // 3. Propery specification
        builder.Property(m => m.Name).HasComment("The key of the resource.");
        builder.Property(m => m.Value).HasComment("The value of the resource.");
        // 4. Other
    }

    /// <inheritdoc/>
    protected override IEnumerable<AppResourceEntity> GetSeedData()
    {
        return AppResource.Resources.Select(r => new AppResourceEntity { Name = r.Key, Value = r.Value });
    }
}
