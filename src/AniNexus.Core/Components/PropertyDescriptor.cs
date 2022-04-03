using System.Reflection;

namespace AniNexus.Components;

/// <summary>
/// A descriptor for a <see cref="PropertyInfo"/>.
/// </summary>
public class PropertyDescriptor : DescriptorBase<PropertyDescriptor, PropertyInfo>
{
    /// <summary>
    /// Creates a new <see cref="PropertyDescriptor"/> instance.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="property"/> is <see langword="null"/></exception>
    /// <exception cref="TypeLoadException">A custom attribute type could not be loaded.</exception>
    /// <exception cref="InvalidCastException">An element in the sequence cannot be cast to type <see cref="Attribute"/>.</exception>
    public PropertyDescriptor(TypeDescriptor type, PropertyInfo property)
        : base(type, property)
    {
    }

    /// <summary>
    /// Creates a new <see cref="PropertyDescriptor"/> instance.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="property"/> is <see langword="null"/></exception>
    /// <exception cref="TypeLoadException">A custom attribute type could not be loaded.</exception>
    /// <exception cref="InvalidCastException">An element in the sequence cannot be cast to type <see cref="Attribute"/>.</exception>
    public PropertyDescriptor(Type type, PropertyInfo property)
        : base(type, property)
    {
    }
}
