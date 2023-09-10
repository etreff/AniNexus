using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.DataAccess.Entities;

/// <summary>
/// Marker interface for use in <see cref="Entity"/> base class.
/// </summary>
internal interface IEnumEntity
{
}

/// <summary>
/// The base class for an entity that models an enum.
/// </summary>
/// <typeparam name="TEntity">The enum entity type.</typeparam>
/// <typeparam name="TEnum">The enum type.</typeparam>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public abstract class EnumEntity<TEntity, TEnum> : Entity<TEntity, int>, IEnumEntity, IEntityTypeConfiguration<TEntity>
    where TEntity : EnumEntity<TEntity, TEnum>, new()
    where TEnum : struct, Enum
{
    private static readonly string TypeName = typeof(TEnum).Name;

    /// <summary>
    /// The string representation of the enum value.
    /// </summary>
    public string Name => _name;
#pragma warning disable IDE0032 // Use auto property
    private string _name = default!;
#pragma warning restore IDE0032 // Use auto property

    /// <summary>
    /// Provides an opportunity to add extra data to the entity while seeding the data.
    /// </summary>
    /// <param name="entity">The database entity for the enum.</param>
    /// <param name="value">The enum value being configured.</param>
    protected virtual void ConfigureDataElement(TEntity entity, TEnum value)
    {
    }

    private string GetDebuggerDisplay()
    {
        return $"{TypeName}.{Name}";
    }

    void IEntityTypeConfiguration<TEntity>.Configure(EntityTypeBuilder<TEntity> builder)
    {
        new EnumEntityConfiguration().Configure(builder);
    }

    private sealed class EnumEntityConfiguration : EntityTypeConfiguration<TEntity, int>
    {
        protected override bool GenerateKeyOnAdd { get; } = false;

        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            base.Configure(builder);

            builder.Property(m => m.Name)
                .UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction);
        }

        protected override string GetTableName()
        {
            string className = typeof(TEnum).Name;
            if (className.Length >= 2 && className[0] == 'E' && char.IsUpper(className[1]))
            {
                className = className[1..];
            }

            return "__enum_" + className;
        }

        protected override IEnumerable<TEntity> GetSeedData()
        {
            foreach (var entity in base.GetSeedData())
            {
                int copy = entity.Id;
                entity.ConfigureDataElement(entity, Unsafe.As<int, TEnum>(ref copy));

                yield return entity;
            }
        }

        protected override IEnumerable<Entity> GetSeedDataImpl()
        {
            foreach (var element in Enum.GetValues<TEnum>())
            {
                var copy = element;
                yield return new Entity(Unsafe.As<TEnum, int>(ref copy), new TEntity
                {
                    _name = element.ToString()
                });
            }
        }
    }
}
