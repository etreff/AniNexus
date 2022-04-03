using System.Collections.Immutable;
using AniNexus.Collections.Concurrent;

namespace AniNexus.Components;

/// <summary>
/// A descriptor for a type.
/// </summary>
public class TypeDescriptor : DescriptorBase<TypeDescriptor, Type>
{
    private static readonly ThreadSafeCache<Type, TypeDescriptorCache> _cache;

    /// <summary>
    /// The public properties of this type.
    /// </summary>
    public IImmutableList<PropertyDescriptor> Properties => _cache.Get(Member).Properties;

    /// <summary>
    /// The public fields of this type.
    /// </summary>
    public IImmutableList<FieldDescriptor> Fields => _cache.Get(Member).Fields;

    static TypeDescriptor()
    {
        _cache = new(t => new TypeDescriptorCache
        {
            Properties = t.GetProperties().Select(p => new PropertyDescriptor(t, p)).ToImmutableArray(),
            Fields = t.GetFields().Select(f => new FieldDescriptor(t, f)).ToImmutableArray()
        });
    }

    /// <summary>
    /// Creates a new <see cref="TypeDescriptor"/> instance.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="type"/> is <see langword="null"/></exception>
    /// <exception cref="TypeLoadException">A custom attribute type could not be loaded.</exception>
    /// <exception cref="InvalidCastException">An element in the sequence cannot be cast to type <see cref="Attribute"/>.</exception>
    public TypeDescriptor(Type type)
        : base(type, type)
    {
    }

    private class TypeDescriptorCache
    {
        public IImmutableList<PropertyDescriptor> Properties { get; set; } = null!;
        public IImmutableList<FieldDescriptor> Fields { get; set; } = null!;
    }
}
