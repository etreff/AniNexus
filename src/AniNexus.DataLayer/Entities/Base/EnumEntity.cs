using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Runtime.CompilerServices;
using AniNexus.Models;

namespace AniNexus.Data.Entities;

/// <summary>
/// The base class for an entity that is backed by an <see cref="Enum"/>.
/// </summary>
/// <typeparam name="TEnum">The <see cref="Enum"/> type.</typeparam>
/// <typeparam name="TEnumEntity">The child entity class that is implemeting this base class.</typeparam>
/// <typeparam name="TUnderlyingType">The underlying type of the enum.</typeparam>
public abstract class EnumEntity<TEnum, TEnumEntity, TUnderlyingType> : Entity<TEnumEntity, TUnderlyingType>
    where TEnum : struct, Enum
    where TEnumEntity : EnumEntity<TEnum, TEnumEntity, TUnderlyingType>, new()
    where TUnderlyingType : struct, IComparable<TUnderlyingType>, IEquatable<TUnderlyingType>
{
    /// <summary>
    /// The name of the enum value.
    /// </summary>
    /// <remarks>
    /// This will be mapped to the name of the <typeparamref name="TEnum"/> member unless
    /// the member is decorated with <see cref="EnumNameAttribute"/>.
    /// </remarks>
    public string Name { get; set; } = default!;

    /// <inheritdoc/>
    protected override string GetTableName()
    {
        string enumName = typeof(TEnum).Name;
        if (enumName.StartsWith('E') && char.IsUpper(enumName[1]))
        {
            enumName = enumName[1..];
        }

        var tableNameAttr = typeof(Enum).GetCustomAttribute<TableAttribute>();
        return tableNameAttr is null ? enumName : tableNameAttr.Name;
    }

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<TEnumEntity> builder)
    {
        // 1. Primary key specification (if not Entity<>)
        // 2. Index specification
        builder.HasIndex(m => m.Name).IsUnique();
        // 3. Navigation properties
        // 4. Propery specification
        builder.Property(m => m.Id).ValueGeneratedNever();
        builder.Property(m => m.Name).HasComment($"The name of the {typeof(TEnum).Name} enum value.");
    }

    /// <inheritdoc/>
    protected override IEnumerable<TEnumEntity> GetSeedData()
    {
        return Enums.GetMembers<TEnum>().Select(static e =>
        {
            var nameAttr = e.GetAttribute<EnumNameAttribute>();
            var value = e.Value;
            return new TEnumEntity
            {
                Id = Unsafe.As<TEnum, TUnderlyingType>(ref value),
                Name = !string.IsNullOrWhiteSpace(nameAttr?.Name) ? nameAttr.Name : e.Name
            };
        });
    }
}

/// <summary>
/// The base class for an entity that is backed by an <see cref="Enum"/>.
/// </summary>
/// <typeparam name="TEnum">The <see cref="Enum"/> type.</typeparam>
/// <typeparam name="TEnumEntity">The child entity class that is implemeting this base class.</typeparam>
public abstract class EnumEntity<TEnum, TEnumEntity> : EnumEntity<TEnum, TEnumEntity, int>
    where TEnum : struct, Enum
    where TEnumEntity : EnumEntity<TEnum, TEnumEntity>, new()
{
}
