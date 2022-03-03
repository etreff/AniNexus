using System.Collections.Immutable;
using Microsoft.Toolkit.Diagnostics;

namespace AniNexus.Components;

/// <summary>
/// A descriptor for a type.
/// </summary>
public class TypeDescriptor : IEquatable<TypeDescriptor>
{
    private static readonly Dictionary<Type, TypeDescriptorCache> _cache = new();
    private static readonly Dictionary<Type, AttributeDescriptorCache[]> _attributeCache = new();

    /// <summary>
    /// The type this descriptor is for.
    /// </summary>
    public Type Type { get; }

    /// <summary>
    /// The public properties of this type.
    /// </summary>
    public IImmutableList<PropertyDescriptor> Properties => _info.Properties;

    /// <summary>
    /// The public fields of this type.
    /// </summary>
    public IImmutableList<FieldDescriptor> Fields => _info.Fields;

    private readonly TypeDescriptorCache _info;
    private readonly AttributeDescriptorCache[] _attributes;

    /// <summary>
    /// Creates a new <see cref="TypeDescriptor"/> instance.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="type"/> is <see langword="null"/></exception>
    /// <exception cref="TypeLoadException">A custom attribute type could not be loaded.</exception>
    /// <exception cref="InvalidCastException">An element in the sequence cannot be cast to type <see cref="Attribute"/>.</exception>
    public TypeDescriptor(Type type)
    {
        Guard.IsNotNull(type, nameof(type));

        Type = type;
        if (_cache.TryGetValue(Type, out var cache))
        {
            _info = cache;
        }
        else
        {
            lock (_cache)
            {
                if (_cache.TryGetValue(Type, out cache))
                {
                    _info = cache;
                }
                else
                {
                    _info = new TypeDescriptorCache
                    {
                        Properties = Type.GetProperties().Select(p => new PropertyDescriptor(p, this)).ToImmutableArray(),
                        Fields = Type.GetFields().Select(f => new FieldDescriptor(f, this)).ToImmutableArray()
                    };

                    _cache.Add(Type, _info);
                }
            }
        }

        if (_attributeCache.TryGetValue(Type, out var attributeCache))
        {
            _attributes = attributeCache;
        }
        else
        {
            lock (_attributeCache)
            {
                if (_attributeCache.TryGetValue(Type, out attributeCache))
                {
                    _attributes = attributeCache;
                }
                else
                {
                    var implemented = Type.GetCustomAttributes(false).Cast<Attribute>().Select(static a => new AttributeDescriptorCache(a, false)).ToList();
                    var inherited = Type.GetCustomAttributes(true).Cast<Attribute>().Select(static a => new AttributeDescriptorCache(a, true)).ToList();

                    implemented.AddRangeUnique(inherited);

                    _attributes = implemented.ToArray();
                    _attributeCache.Add(Type, _attributes);
                }
            }
        }
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

        foreach (var attribute in _attributes)
        {
            if (!inherit && attribute.IsInherited)
            {
                continue;
            }

            if (attribute.Attribute.GetType() == attributeType)
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

        foreach (var attribute in _attributes)
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

        foreach (var attribute in _attributes)
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
    public bool Equals(TypeDescriptor? other)
    {
        return other is not null && (ReferenceEquals(this, other) || Type == other.Type);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is not null && (ReferenceEquals(this, obj) || (obj is TypeDescriptor other && Equals(other)));
    }

    /// <summary>
    /// Checks two instances for equality.
    /// </summary>
    public static bool operator ==(TypeDescriptor? left, TypeDescriptor? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Checks two instances for inequality.
    /// </summary>
    public static bool operator !=(TypeDescriptor? left, TypeDescriptor? right)
    {
        return !Equals(left, right);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        // We do not want to return the same hash code as Type, otherwise we
        // will get collisions in a dictionary where the key is of type
        // System.Object.
        return HashCode.Combine(Type, typeof(TypeDescriptor));
    }

    private class TypeDescriptorCache
    {
        public IImmutableList<PropertyDescriptor> Properties { get; set; } = null!;
        public IImmutableList<FieldDescriptor> Fields { get; set; } = null!;
    }

    private class AttributeDescriptorCache : IEquatable<AttributeDescriptorCache>
    {
        public Attribute Attribute { get; }
        public bool IsInherited { get; }

        public AttributeDescriptorCache(Attribute attribute, bool isInherited)
        {
            Attribute = attribute;
            IsInherited = isInherited;
        }

        /// <inheritdoc />
        public bool Equals(AttributeDescriptorCache? other)
        {
            return other is not null && (ReferenceEquals(this, other) || Attribute.Equals(other.Attribute));
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return obj is not null && (ReferenceEquals(this, obj) || (obj.GetType() == GetType() && Equals((AttributeDescriptorCache)obj)));
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(Attribute, IsInherited);
        }

        public static bool operator ==(AttributeDescriptorCache? left, AttributeDescriptorCache? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AttributeDescriptorCache? left, AttributeDescriptorCache? right)
        {
            return !Equals(left, right);
        }
    }
}

/// <summary>
/// Checks the equality of two <see cref="TypeDescriptor"/>s.
/// </summary>
public class TypeDescriptorEqualityComparer : IEqualityComparer<TypeDescriptor>
{
    /// <inheritdoc />
    public bool Equals(TypeDescriptor? x, TypeDescriptor? y)
    {
        return x == y;
    }

    /// <inheritdoc />
    public int GetHashCode(TypeDescriptor obj)
    {
        return obj.GetHashCode();
    }
}
