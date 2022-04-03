using System.Reflection;
using AniNexus.Collections.Concurrent;
using AniNexus.Reflection;
using Microsoft.Toolkit.Diagnostics;

namespace AniNexus.Components;

/// <summary>
/// The base class for a descriptor.
/// </summary>
public abstract class DescriptorBase<T, TFor> : IEquatable<T>
    where T : DescriptorBase<T, TFor>
    where TFor : MemberInfo
{
    /// <summary>
    /// The attribute cache for the <see cref="MemberInfo"/>.
    /// </summary>
    private protected static readonly ThreadSafeCache<TFor, AttributeDescriptorCache[]> AttributeCache;

    /// <summary>
    /// The member this descriptor targets.
    /// </summary>
    public TFor Member { get; }

    /// <summary>
    /// The <see cref="Components.TypeDescriptor"/> associated with this member.
    /// </summary>
    public TypeDescriptor TypeDescriptor { get; }

    static DescriptorBase()
    {
        AttributeCache = new(f =>
        {
            var implemented = Attribute.GetCustomAttributes(f, false);
            var all = Attribute.GetCustomAttributes(f, true);

            return all.Select(a => new AttributeDescriptorCache
            {
                Attribute = a,
                IsInherited = implemented.Contains(a)
            }).ToArray();
        });
    }

    private protected DescriptorBase(Type type, TFor member)
        : this(new TypeDescriptor(type), member)
    {
    }

    private protected DescriptorBase(TypeDescriptor type, TFor member)
    {
        TypeDescriptor = type;
        Member = member;
    }

    /// <summary>
    /// Whether the type has the specified attribute.
    /// </summary>
    /// <typeparam name="TAttribute">The type of attribute.</typeparam>
    /// <param name="inherit">Whether to look for inherited attributes.</param>
    public bool HasAttribute<TAttribute>(bool inherit = false)
        where TAttribute : Attribute => HasAttribute(typeof(TAttribute), inherit);

    /// <summary>
    /// Whether the type has the specified attribute.
    /// </summary>
    /// <param name="attributeType">The type of attribute.</param>
    /// <param name="inherit">Whether to look for inherited attributes.</param>
    /// <exception cref="ArgumentNullException"><paramref name="attributeType"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="attributeType"/> is not an <see cref="Attribute"/> type.</exception>
    public bool HasAttribute(Type attributeType, bool inherit = false)
    {
        Guard.IsNotNull(attributeType, nameof(attributeType));
        GuardEx.IsTypeOf<Attribute>(attributeType, nameof(attributeType));

        foreach (var attribute in AttributeCache.Get(Member))
        {
            if (!inherit && attribute.IsInherited)
            {
                continue;
            }

            if (attribute.Attribute.GetType().IsTypeOf(attributeType))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Obtains the specified attribute from the type.
    /// </summary>
    /// <typeparam name="TAttribute">The type of attribute to get.</typeparam>
    /// <param name="inherit">Whether to look for inherited attributes.</param>
    public TAttribute? GetAttribute<TAttribute>(bool inherit = false)
        where TAttribute : Attribute => GetAttribute(typeof(TAttribute), inherit) as TAttribute;

    /// <summary>
    /// Obtains the specified attribute from the type.
    /// </summary>
    /// <param name="attributeType">The type of attribute.</param>
    /// <param name="inherit">Whether to look for inherited attributes.</param>
    /// <exception cref="ArgumentNullException"><paramref name="attributeType"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="attributeType"/> is not an <see cref="Attribute"/> type.</exception>
    public Attribute? GetAttribute(Type attributeType, bool inherit = false)
    {
        Guard.IsNotNull(attributeType, nameof(attributeType));
        GuardEx.IsTypeOf<Attribute>(attributeType, nameof(attributeType));

        foreach (var attribute in AttributeCache.Get(Member))
        {
            if (!inherit && attribute.IsInherited)
            {
                continue;
            }

            if (attribute.Attribute.GetType() == attributeType)
            {
                return attribute.Attribute;
            }
        }

        return null;
    }

    /// <summary>
    /// Obtains all of the specified attribute from the type.
    /// </summary>
    /// <typeparam name="TAttribute">The type of attributes to get.</typeparam>
    /// <param name="inherit">Whether to look for inherited attributes.</param>
    /// <exception cref="InvalidCastException">An element in the sequence cannot be cast to type <typeparamref name="TAttribute"/>.</exception>
    public IEnumerable<TAttribute> GetAttributes<TAttribute>(bool inherit = false)
        where TAttribute : Attribute => GetAttributes(typeof(TAttribute), inherit).Cast<TAttribute>();

    /// <summary>
    /// Obtains all of the specified attribute from the type.
    /// </summary>
    /// <param name="attributeType">The type of attribute.</param>
    /// <param name="inherit">Whether to look for inherited attributes.</param>
    /// <exception cref="ArgumentNullException"><paramref name="attributeType"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="attributeType"/> is not an <see cref="Attribute"/> type.</exception>
    public IEnumerable<Attribute> GetAttributes(Type attributeType, bool inherit = false)
    {
        Guard.IsNotNull(attributeType, nameof(attributeType));
        GuardEx.IsTypeOf<Attribute>(attributeType, nameof(attributeType));

        foreach (var attribute in AttributeCache.Get(Member))
        {
            if (!inherit && attribute.IsInherited)
            {
                continue;
            }

            if (attribute.Attribute.GetType() == attributeType)
            {
                yield return attribute.Attribute;
            }
        }
    }

    /// <inheritdoc />
    public bool Equals(T? other)
    {
        return other is not null && (ReferenceEquals(this, other) || Member.Equals(other.Member));
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is not null && (ReferenceEquals(this, obj) || (obj.GetType() == GetType() && Equals((FieldDescriptor)obj)));
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        // We do not want to return the same hash code as the member, otherwise we
        // will get collisions in a dictionary where the key is of type
        // System.Object.
        return HashCode.Combine(Member, typeof(TypeDescriptor));
    }

    private protected class AttributeDescriptorCache
    {
        public Attribute Attribute { get; set; } = null!;
        public bool IsInherited { get; set; }
    }

    /// <summary>
    /// Checks the equality of two instances.
    /// </summary>
    public class Comparer : IEqualityComparer<T>
    {
        /// <summary>
        /// A singleton instance of this comparer.
        /// </summary>
        public static Comparer Instance { get; } = new Comparer();

        /// <inheritdoc />
        public bool Equals(T? x, T? y)
        {
            if (x is null)
            {
                return y is null;
            }

            return x.Equals(y);
        }

        /// <inheritdoc />
        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }
}
