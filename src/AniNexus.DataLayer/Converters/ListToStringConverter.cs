using System.Linq.Expressions;
using AniNexus.Collections;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AniNexus.Data.Converters
{
    /// <summary>
    /// Converts a collection of elements of type <typeparamref name="T"/> to a <see cref="string"/>
    /// and vice versa.
    /// </summary>
    public class ListToStringConverter<T> : ValueConverter<IList<T>, string?>
    {
        /// <summary>
        /// Default string split options for transforming the string to a collection of elements.
        /// </summary>
        public const StringSplitOptions DefaultSplitOptions = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

        /// <summary>
        /// Creates a new <see cref="ListToStringConverter{T}"/> instance.
        /// </summary>
        /// <param name="valueConverter">The converter to convert string to an element of type <typeparamref name="T"/>.</param>
        /// <param name="delimiter">The delimiter to use to separate elements in the collection.</param>
        public ListToStringConverter(Func<string, T> valueConverter, char delimiter = ListToStringConverter.DefaultDelimiter)
            : base(l => l != null ? string.Join(delimiter, l) : null, s => s != null ? s.Split(delimiter, DefaultSplitOptions).Select(valueConverter).ToList() : new List<T>())
        {
        }

        /// <inheritdoc/>
        public ListToStringConverter(Expression<Func<IList<T>, string?>> convertToProviderExpression, Expression<Func<string?, IList<T>>> convertFromProviderExpression, ConverterMappingHints? mappingHints = null)
        : base(convertToProviderExpression, convertFromProviderExpression, mappingHints)
        {
        }
    }

    /// <summary>
    /// Converts a collection of string to a single <see cref="string"/>
    /// and vice versa.
    /// </summary>
    public class ListToStringConverter : ListToStringConverter<string>
    {
        /// <summary>
        /// The default delimiter to use.
        /// </summary>
        /// <remarks>
        /// The default delimiter is equivalent to the unit separator.
        /// </remarks>
        public const char DefaultDelimiter = (char)0x1f;

        /// <summary>
        /// Creates a new <see cref="ListToStringConverter"/> instance that converts multiple strings
        /// into a single string.
        /// </summary>
        /// <param name="delimiter">The delimiter to use to separate elements in the collection.</param>
        public ListToStringConverter(char delimiter = DefaultDelimiter)
            : base(l => string.Join(delimiter, l), s => s != null ? s.Split(delimiter, DefaultSplitOptions).ToList() : new List<string>())
        {
        }
    }

    /// <summary>
    /// Compares a collection of elements of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListToStringComparer<T> : ValueComparer<IList<T?>>
    {
        /// <summary>
        /// Creates a new <see cref="ListToStringComparer{T}"/> instance.
        /// </summary>
        /// <param name="equalityComparer">The equality comparer to use.</param>
        /// <param name="orderMatters">Whether order matters for equality of two lists.</param>
        public ListToStringComparer(IEqualityComparer<T?>? equalityComparer = null, bool orderMatters = false)
            : base((a, b) => GetEquals(a, b, equalityComparer, orderMatters), a => GetHash(a, orderMatters))
        {
        }

        private static bool GetEquals(IList<T?>? a, IList<T?>? b, IEqualityComparer<T?>? equalityComparer, bool orderMatters)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            if (a.Count != b.Count)
            {
                return false;
            }

            if (orderMatters)
            {
                return a.SequenceEqual(b);
            }
            else
            {
                return new UnsortedEqualityComparer<T?>(equalityComparer).Equals(a, b);
            }
        }

        private static int GetHash(IList<T?> elements, bool orderMatters)
        {
            if (elements is null)
            {
                return 0;
            }

            if (orderMatters)
            {
                var hashCode = new HashCode();
                foreach (var element in elements)
                {
                    hashCode.Add(element);
                }

                return hashCode.ToHashCode();
            }

            return UnsortedEqualityComparer<T?>.Default.GetHashCode(elements);
        }
    }

    /// <summary>
    /// Compares a collection of string elements.
    /// </summary>
    public class ListToStringComparer : ListToStringComparer<string>
    {
        /// <summary>
        /// A singleton instance of this class.
        /// </summary>
        public static ListToStringComparer Instance { get; } = new ListToStringComparer();
    }
}
