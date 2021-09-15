using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using AniNexus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// The base class for a model that is backed by an <see cref="Enum"/>.
/// </summary>
/// <typeparam name="TEnum">The <see cref="Enum"/> type.</typeparam>
/// <typeparam name="TEnumModel">The child model class that is implemeting this base class.</typeparam>
public abstract class EnumModelBase<TEnum, TEnumModel> : IEntityTypeConfiguration<TEnumModel>
    where TEnum : struct, Enum
    where TEnumModel : EnumModelBase<TEnum, TEnumModel>, new()
{
    /// <summary>
    /// The Id of the enum value.
    /// </summary>
    /// <remarks>
    /// This will be mapped to the underlying value of the <typeparamref name="TEnum"/> member.
    /// </remarks>
    public int Id { get; set; }

    /// <summary>
    /// The name of the enum value.
    /// </summary>
    /// <remarks>
    /// This will be mapped to the name of the <typeparamref name="TEnum"/> member unless
    /// the member is decorated with <see cref="EnumNameAttribute"/>.
    /// </remarks>
    public string Name { get; set; } = default!;

    public void Configure(EntityTypeBuilder<TEnumModel> builder)
    {
        string enumName = typeof(TEnum).Name;
        if (enumName.StartsWith('E') && char.IsUpper(enumName[1]))
        {
            enumName = enumName[1..];
        }

        var tableNameAttr = typeof(Enum).GetCustomAttribute<TableAttribute>();
        string tableName = tableNameAttr is null ? enumName : tableNameAttr.Name;

        builder.ToTable(tableName);

        builder.HasKey(m => m.Id).IsClustered();
        builder.HasIndex(m => m.Name).IsUnique();

        builder.HasData(Enums.GetMembers<TEnum>().Select(static e =>
        {
            var nameAttr = e.GetAttribute<EnumNameAttribute>();
            return new TEnumModel
            {
                Id = Convert.ToInt32(e.Value),
                Name = !string.IsNullOrWhiteSpace(nameAttr?.Name) ? nameAttr.Name : e.Name
            };
        }));

        builder.Property(m => m.Id).ValueGeneratedNever();
        builder.Property(m => m.Name).HasComment($"The name of the {enumName} enum value.");
    }
}
