using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using AniNexus.Collections.Concurrent;

namespace System;

/// <summary>
/// A helper for working with Enums.
/// </summary>
public static class EnumHelper
{
    private static readonly ThreadSafeCache<Type, EnumInfo> _cache = new(t => new EnumInfo(t));

    /// <summary>
    /// Returns whether <typeparamref name="T"/> is a flags enum type.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    public static bool IsFlagEnum<T>()
        where T : struct, Enum
    {
        return _cache.Get(typeof(T)).IsFlag;
    }

    /// <summary>
    /// Returns whether <paramref name="value"/> is a valid member of
    /// <typeparamref name="T"/> or whether it is a valid flag combination.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="value">The value to check.</param>
    /// <param name="strict">Whether <paramref name="value"/> must be strictly defined in the case of a flag enum.</param>
    public static bool IsValid<T>(this T value, bool strict = false)
        where T : struct, Enum
    {
        return _cache.Get(typeof(T)).IsValid(value, strict);
    }

    /// <summary>
    /// Returns the <see cref="DescriptionAttribute.Description"/> of the <see cref="DescriptionAttribute"/>
    /// decorating this enum member, if one exists.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="value">The enum value.</param>
    /// <exception cref="AmbiguousMatchException">More than one of the requested attributes was found.</exception>
    /// <exception cref="TypeLoadException">A custom attribute type cannot be loaded.</exception>
    public static string? GetDescription<T>(this T value)
        where T : struct, Enum
    {
        return GetField(value)?.GetCustomAttribute<DescriptionAttribute>()?.Description;
    }

    /// <summary>
    /// Returns the fields of the enum type.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FieldInfo? GetField<T>(this T value)
        where T : struct, Enum
    {
        var info = _cache.Get(typeof(T));
        return info.TryGetField(value, out var field) ? field : null;
    }

    /// <summary>
    /// Returns the fields of the enum type.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FieldInfo[] GetFields<T>()
        where T : struct, Enum
    {
        return _cache.Get(typeof(T)).Fields.Values.ToArray();
    }

    /// <summary>
    /// Returns the individual set flags of the enum value.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    public static IEnumerable<T> GetSetFlags<T>()
        where T : struct, Enum
    {
        return _cache.Get(typeof(T)).Flags.Cast<T>();
    }

    /// <summary>
    /// Returns the individual set flags of the enum value.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    public static IEnumerable<T> GetSetFlags<T>(this T value)
        where T : struct, Enum
    {
        long v = Convert.ToInt64(value);
        foreach (var flag in GetSetFlags<T>())
        {
            long f = Convert.ToInt64(flag);
            if ((v & f) == f)
            {
                yield return flag;
            }
        }
    }

    private readonly struct EnumInfo
    {
        public readonly bool IsFlag { get; }
        public readonly long Mask { get; }
        public readonly IImmutableDictionary<int, FieldInfo> Fields { get; }
        public readonly IImmutableList<object> Flags { get; }

        public EnumInfo(Type enumType)
        {
            IsFlag = enumType.GetCustomAttribute<FlagsAttribute>() is not null;
            Flags = GetFlags(enumType).ToImmutableArray();
            Fields = enumType.GetTypeInfo().DeclaredFields
                .Where(f => (f.Attributes & (FieldAttributes.Static | FieldAttributes.Public)) == (FieldAttributes.Static | FieldAttributes.Public))
                .ToImmutableDictionary(f => f.GetValue(null)!.GetHashCode(), f => f);

            long mask = 0;
            foreach (object value in Enum.GetValues(enumType))
            {
                mask |= Convert.ToInt64(value);
            }

            Mask = mask;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetField<T>(T value, [NotNullWhen(true)] out FieldInfo? result)
            where T : struct, Enum
        {
            return Fields.TryGetValue(value.GetHashCode(), out result);
        }

        public bool IsValid<T>(T value, bool strict)
            where T : struct, Enum
        {
            if (!IsFlag || strict)
            {
                return Fields.ContainsKey(value.GetHashCode());
            }

            long v = Convert.ToInt64(value);

            return (v & Mask) == v;
        }

        private IEnumerable<object> GetFlags(Type enumType)
        {
            if (!IsFlag)
            {
                foreach (object? value in Enum.GetValues(enumType))
                {
                    yield return value;
                }

                yield break;
            }

            foreach (object? val in Enum.GetValues(enumType))
            {
                ulong v = Convert.ToUInt64(val);
                int bitCount = BitOperations.PopCount(v);
                if (bitCount is not 1)
                {
                    continue;
                }

                yield return val;
            }
        }
    }
}
