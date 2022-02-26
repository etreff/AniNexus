﻿using System.Collections.Immutable;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace AniNexus;

/// <summary>
/// Numerics.
/// </summary>
public static class Number
{
    /// <summary>
    /// The size of a <see cref="Boolean"/>.
    /// </summary>
    [SuppressMessage("Style", "IDE0049:Simplify Names", Justification = "Intentional.")]
    public const byte BoolSize = sizeof(bool);

    /// <summary>
    /// The size of an <see cref="Int32"/>.
    /// </summary>
    [SuppressMessage("Style", "IDE0049:Simplify Names", Justification = "Intentional.")]
    public const byte IntSize = sizeof(int);

    /// <summary>
    /// The size of an <see cref="Int64"/>.
    /// </summary>
    [SuppressMessage("Style", "IDE0049:Simplify Names", Justification = "Intentional.")]
    public const byte Int64Size = sizeof(long);

    /// <summary>
    /// The size of an <see cref="UInt64"/>.
    /// </summary>
    [SuppressMessage("Style", "IDE0049:Simplify Names", Justification = "Intentional.")]
    public const byte UInt64Size = sizeof(ulong);

    /// <summary>
    /// The size of a <see cref="Single"/>.
    /// </summary>
    [SuppressMessage("Style", "IDE0049:Simplify Names", Justification = "Intentional.")]
    public const byte SingleSize = sizeof(Single);

    /// <summary>
    /// The size of a <see cref="Double"/>.
    /// </summary>
    [SuppressMessage("Style", "IDE0049:Simplify Names", Justification = "Intentional.")]
    public const byte DoubleSize = sizeof(double);

    /// <summary>
    /// Known integer types.
    /// </summary>
    public static IImmutableList<Type> IntegerTypes { get; } = new[]
    {
            typeof(byte), typeof(sbyte),
            typeof(ushort), typeof(short),
            typeof(uint), typeof(int),
            typeof(ulong), typeof(long),
            typeof(BigInteger)
        }.ToImmutableArray();

    /// <summary>
    /// Known floating point types.
    /// </summary>
    public static IImmutableList<Type> FloatingPointTypes { get; } = new[]
    {
            typeof(Complex), typeof(decimal),
            typeof(float), typeof(double)
        }.ToImmutableArray();

    /// <summary>
    /// Known numeric types.
    /// </summary>
    public static IImmutableList<Type> NumericTypes { get; } = IntegerTypes.AddRange(FloatingPointTypes);

    /// <summary>
    /// Returns the next power of 2.
    /// </summary>
    /// <param name="v">The number.</param>
    public static int NextPowerOfTwo(int v)
    {
        --v;
        v |= v >> 1;
        v |= v >> 2;
        v |= v >> 4;
        v |= v >> 8;
        ++v;
        return v;
    }

    /// <summary>
    /// Returns whether this number is a power of 2.
    /// </summary>
    /// <param name="v">The number.</param>
    public static bool IsPowerOfTwo(int v)
    {
        return (v & (v - 1)) == 0;
    }

    /// <summary>
    /// Returns the number of bits set in the integer.
    /// </summary>
    /// <param name="v">The integer.</param>
    /// <returns>The number of set bits.</returns>
    public static int BitCount(int v)
    {
        v -= (v >> 1) & 0x55555555;
        v = (v & 0x33333333) + ((v >> 2) & 0x33333333);
        return ((v + (v >> 4) & 0xF0F0F0F) * 0x1010101) >> 24;
    }

    /// <summary>
    /// Returns the number of bits set in the long.
    /// </summary>
    /// <param name="v">The long.</param>
    /// <returns>The number of set bits.</returns>
    public static int BitCount(long v)
    {
        v -= (v >> 1) & 0x5555555555555555;
        v = (v & 0x3333333333333333) + ((v >> 2) & 0x3333333333333333);
        return (int)(((v + (v >> 4) & 0xF0F0F0F0F0F0F0F) * 0x101010101010101) >> 56);
    }

    /// <summary>
    /// Returns whether the object is a numeric type.
    /// </summary>
    /// <param name="obj">The object to check.</param>
    public static bool IsNumericType(object? obj)
        => IsNumericType(obj?.GetType());

    /// <summary>
    /// Returns whether the type is a numeric type.
    /// </summary>
    /// <typeparam name="T">The type to check.</typeparam>
    public static bool IsNumericType<T>()
        => IsNumericType(typeof(T));

    /// <summary>
    /// Returns whether the type is a numeric type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    public static bool IsNumericType(Type? type)
    {
        return type is not null && NumericTypes.Contains(type);
    }

    /// <summary>
    /// Returns whether the object is an integer type and not a floating
    /// point type.
    /// </summary>
    /// <param name="obj">The object to check.</param>
    public static bool IsIntegerType(object? obj)
        => IsIntegerType(obj?.GetType());

    /// <summary>
    /// Returns whether the type is an integer type and not a floating
    /// point type.
    /// </summary>
    /// <typeparam name="T">The type to check.</typeparam>
    public static bool IsIntegerType<T>()
        => IsIntegerType(typeof(T));

    /// <summary>
    /// Returns whether the type is an integer type and not a floating
    /// point type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    public static bool IsIntegerType(Type? type)
    {
        return type is not null && IntegerTypes.Contains(type);
    }

    /// <summary>
    /// Returns whether the object is a floating point type and not an integer
    /// type.
    /// </summary>
    /// <param name="obj">The object to check.</param>
    public static bool IsFloatingPointType(object? obj)
        => IsFloatingPointType(obj?.GetType());

    /// <summary>
    /// Returns whether the type is a floating point type and not an integer
    /// type.
    /// </summary>
    /// <typeparam name="T">The type to check.</typeparam>
    public static bool IsFloatingPointType<T>()
        => IsFloatingPointType(typeof(T));

    /// <summary>
    /// Returns whether the type is a floating point type and not an integer
    /// type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    public static bool IsFloatingPointType(Type? type)
    {
        return type is not null && FloatingPointTypes.Contains(type);
    }

    /// <summary>
    /// Returns whether the <see cref="byte"/> represents an even number.
    /// </summary>
    /// <param name="n">The number to check.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEven(this byte n)
    {
        return (n & 1) == 0;
    }

    /// <summary>
    /// Returns whether the <see cref="sbyte"/> represents an even number.
    /// </summary>
    /// <param name="n">The number to check.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEven(this sbyte n)
    {
        return (n & 1) == 0;
    }

    /// <summary>
    /// Returns whether the <see cref="short"/> represents an even number.
    /// </summary>
    /// <param name="n">The number to check.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEven(this short n)
    {
        return (n & 1) == 0;
    }

    /// <summary>
    /// Returns whether the <see cref="ushort"/> represents an even number.
    /// </summary>
    /// <param name="n">The number to check.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEven(this ushort n)
    {
        return (n & 1) == 0;
    }

    /// <summary>
    /// Returns whether the <see cref="int"/> represents an even number.
    /// </summary>
    /// <param name="n">The number to check.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEven(this int n)
    {
        return (n & 1) == 0;
    }

    /// <summary>
    /// Returns whether the <see cref="uint"/> represents an even number.
    /// </summary>
    /// <param name="n">The number to check.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEven(this uint n)
    {
        return (n & 1) == 0;
    }

    /// <summary>
    /// Returns whether the <see cref="long"/> represents an even number.
    /// </summary>
    /// <param name="n">The number to check.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEven(this long n)
    {
        return (n & 1) == 0;
    }

    /// <summary>
    /// Returns whether the <see cref="ulong"/> represents an even number.
    /// </summary>
    /// <param name="n">The number to check.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEven(this ulong n)
    {
        return (n & 1) == 0;
    }

    /// <summary>
    /// Returns whether the <see cref="byte"/> represents an odd number.
    /// </summary>
    /// <param name="n">The number to check.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOdd(this byte n)
    {
        return (n & 1) == 1;
    }

    /// <summary>
    /// Returns whether the <see cref="sbyte"/> represents an odd number.
    /// </summary>
    /// <param name="n">The number to check.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOdd(this sbyte n)
    {
        return (n & 1) == 1;
    }

    /// <summary>
    /// Returns whether the <see cref="short"/> represents an odd number.
    /// </summary>
    /// <param name="n">The number to check.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOdd(this short n)
    {
        return (n & 1) == 1;
    }

    /// <summary>
    /// Returns whether the <see cref="ushort"/> represents an odd number.
    /// </summary>
    /// <param name="n">The number to check.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOdd(this ushort n)
    {
        return (n & 1) == 1;
    }

    /// <summary>
    /// Returns whether the <see cref="int"/> represents an odd number.
    /// </summary>
    /// <param name="n">The number to check.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOdd(this int n)
    {
        return (n & 1) == 1;
    }

    /// <summary>
    /// Returns whether the <see cref="uint"/> represents an odd number.
    /// </summary>
    /// <param name="n">The number to check.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOdd(this uint n)
    {
        return (n & 1) == 1;
    }

    /// <summary>
    /// Returns whether the <see cref="long"/> represents an odd number.
    /// </summary>
    /// <param name="n">The number to check.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOdd(this long n)
    {
        return (n & 1) == 1;
    }

    /// <summary>
    /// Returns whether the <see cref="ulong"/> represents an odd number.
    /// </summary>
    /// <param name="n">The number to check.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOdd(this ulong n)
    {
        return (n & 1) == 1;
    }

    /// <summary>
    /// Returns the least signficant set bit.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LeastSigBitSet(this int value)
    {
        return value & -value;
    }

    /// <summary>
    /// Returns the index of the least signficant set bit.
    /// </summary>
    /// <param name="value">The value.</param>
    public static int LeastSigBitSetIndex(this int value)
    {
        int x = value.LeastSigBitSet();
        int index = -1;
        do
        {
            x >>= 1;
            ++index;
        }
        while (x != 0);
        return index;
    }

    /// <summary>
    /// Returns the most signficant set bit.
    /// </summary>
    /// <param name="value">The value.</param>
    public static int MostSigBitSet(this int value)
    {
        value |= value >> 1;
        value |= value >> 2;
        value |= value >> 4;
        value |= value >> 8;
        value |= value >> 16;

        return value & ~(value >> 1);
    }

    /// <summary>
    /// Returns the index of the most signficant set bit.
    /// </summary>
    /// <param name="value">The value.</param>
    public static int MostSigBitSetIndex(this int value)
    {
        int x = value.MostSigBitSet();
        int index = -1;
        do
        {
            x >>= 1;
            ++index;
        }
        while (x != 0);
        return index;
    }

    /// <summary>
    /// Returns the least signficant set bit.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint LeastSigBitSet(this uint value)
    {
        return value & 0u - value;
    }

    /// <summary>
    /// Returns the index of the least signficant set bit.
    /// </summary>
    /// <param name="value">The value.</param>
    public static uint LeastSigBitSetIndex(this uint value)
    {
        uint x = value.LeastSigBitSet();
        uint index = 0u;
        do
        {
            x >>= 1;
            ++index;
        }
        while (x != 0);
        return index - 1;
    }

    /// <summary>
    /// Returns the most signficant set bit.
    /// </summary>
    /// <param name="value">The value.</param>
    public static uint MostSigBitSet(this uint value)
    {
        value |= value >> 1;
        value |= value >> 2;
        value |= value >> 4;
        value |= value >> 8;
        value |= value >> 16;

        return value & ~(value >> 1);
    }

    /// <summary>
    /// Returns the index of the most signficant set bit.
    /// </summary>
    /// <param name="value">The value.</param>
    public static uint MostSigBitSetIndex(this uint value)
    {
        uint x = value.MostSigBitSet();
        uint index = 0u;
        do
        {
            x >>= 1;
            ++index;
        }
        while (x != 0);
        return index - 1;
    }

    /// <summary>
    /// Returns the number of set bits in an <see langword="int"/>.
    /// </summary>
    /// <param name="value">The <see langword="int"/> value.</param>
    /// <returns>The number of set bits.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int NumSetBits(this int value)
    {
        return NumSetBits(Convert.ToInt64(value));
    }

    /// <summary>
    /// Returns the number of set bits in an <see langword="uint"/>.
    /// </summary>
    /// <param name="value">The <see langword="uint"/> value.</param>
    /// <returns>The number of set bits.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int NumSetBits(this uint value)
    {
        return NumSetBits(Convert.ToUInt64(value));
    }

    /// <summary>
    /// Returns the number of set bits in an <see langword="ulong"/>.
    /// </summary>
    /// <param name="value">The <see langword="ulong"/> value.</param>
    /// <returns>The number of set bits.</returns>
    public static int NumSetBits(this long value)
    {
        long val = value;
        int count = 0;
        while (val != 0)
        {
            if ((val & 0x1) == 0x1)
            {
                ++count;
            }
            val >>= 1;
        }

        return count;
    }

    /// <summary>
    /// Returns the number of set bits in an <see langword="ulong"/>.
    /// </summary>
    /// <param name="value">The <see langword="ulong"/> value.</param>
    /// <returns>The number of set bits.</returns>
    public static int NumSetBits(this ulong value)
    {
        ulong val = value;
        int count = 0;
        while (val != 0)
        {
            if ((val & 0x1) == 0x1)
            {
                ++count;
            }
            val >>= 1;
        }

        return count;
    }

    /// <summary>
    /// Rounds <paramref name="value"/> to the nearest multiple defined in <paramref name="multiple"/>.
    /// </summary>
    /// <param name="value">The value to round.</param>
    /// <param name="multiple">The nearest multiple to round to.</param>
    public static int RoundToNearest(this int value, int multiple)
    {
        int a = value / multiple * multiple;
        int b = value + multiple;
        return (value - a > b - value) ? b : a;
    }

    /// <summary>
    /// Rounds <paramref name="value"/> to the nearest multiple defined in <paramref name="multiple"/>.
    /// </summary>
    /// <param name="value">The value to round.</param>
    /// <param name="multiple">The nearest multiple to round to.</param>
    public static long RoundToNearest(this long value, int multiple)
    {
        long a = value / multiple * multiple;
        long b = value + multiple;
        return (value - a > b - value) ? b : a;
    }

    /// <summary>
    /// Rounds <paramref name="value"/> to the nearest multiple defined in <paramref name="multiple"/>.
    /// </summary>
    /// <param name="value">The value to round.</param>
    /// <param name="multiple">The nearest multiple to round to.</param>
    public static long RoundToNearest(this long value, long multiple)
    {
        long a = value / multiple * multiple;
        long b = value + multiple;
        return (value - a > b - value) ? b : a;
    }

    /// <summary>
    /// Rounds <paramref name="value"/> to the nearest multiple defined in <paramref name="multiple"/>.
    /// </summary>
    /// <param name="value">The value to round.</param>
    /// <param name="multiple">The nearest multiple to round to.</param>
    public static uint RoundToNearest(this uint value, uint multiple)
    {
        uint a = value / multiple * multiple;
        uint b = value + multiple;
        return (value - a > b - value) ? b : a;
    }

    /// <summary>
    /// Rounds <paramref name="value"/> to the nearest multiple defined in <paramref name="multiple"/>.
    /// </summary>
    /// <param name="value">The value to round.</param>
    /// <param name="multiple">The nearest multiple to round to.</param>
    public static ulong RoundToNearest(this ulong value, uint multiple)
    {
        ulong a = value / multiple * multiple;
        ulong b = value + multiple;
        return (value - a > b - value) ? b : a;
    }

    /// <summary>
    /// Rounds <paramref name="value"/> to the nearest multiple defined in <paramref name="multiple"/>.
    /// </summary>
    /// <param name="value">The value to round.</param>
    /// <param name="multiple">The nearest multiple to round to.</param>
    public static ulong RoundToNearest(this ulong value, ulong multiple)
    {
        ulong a = value / multiple * multiple;
        ulong b = value + multiple;
        return (value - a > b - value) ? b : a;
    }
}

