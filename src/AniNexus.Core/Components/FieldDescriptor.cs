using System.Reflection;

namespace AniNexus.Components;

/// <summary>
/// A descriptor for a <see cref="FieldInfo"/>.
/// </summary>
public class FieldDescriptor : DescriptorBase<FieldDescriptor, FieldInfo>
{
    /// <summary>
    /// Creates a new <see cref="FieldDescriptor"/> instance.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="field"/> is <see langword="null"/></exception>
    /// <exception cref="TypeLoadException">A custom attribute type could not be loaded.</exception>
    /// <exception cref="InvalidCastException">An element in the sequence cannot be cast to type <see cref="Attribute"/>.</exception>
    public FieldDescriptor(TypeDescriptor type, FieldInfo field)
        : base(type, field)

    {
    }

    /// <summary>
    /// Creates a new <see cref="FieldDescriptor"/> instance.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="field"/> is <see langword="null"/></exception>
    /// <exception cref="TypeLoadException">A custom attribute type could not be loaded.</exception>
    /// <exception cref="InvalidCastException">An element in the sequence cannot be cast to type <see cref="Attribute"/>.</exception>
    public FieldDescriptor(Type type, FieldInfo field)
        : base(type, field)
    {
    }
}
