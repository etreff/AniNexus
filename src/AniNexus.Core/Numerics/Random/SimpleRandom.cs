using System.Runtime.InteropServices;
using Microsoft.Toolkit.Diagnostics;

namespace AniNexus.Numerics.Random;

/// <summary>
/// A simple <see cref="System.Random"/> that uses <see cref="IRandomNumberProvider"/> as its generator.
/// This class is not thread-safe.
/// </summary>
public sealed class SimpleRandom : IRandomNumberProvider
{
    private readonly System.Random _random;

    /// <summary>
    /// Creates a new <see cref="SimpleRandom"/> instance.
    /// </summary>
    public SimpleRandom()
    {
        _random = new System.Random();
    }

    /// <summary>
    /// Creates a new <see cref="SimpleRandom"/> instance.
    /// </summary>
    /// <param name="seed"></param>
    public SimpleRandom(int seed)
    {
        _random = new System.Random(seed);
    }

    /// <inheritdoc />
    /// <exception cref="InvalidOperationException">This function is not supported for this <see cref="IRandomNumberProvider"/> implementation.</exception>
    public ulong NextUInt64()
    {
        Span<byte> buffer = stackalloc byte[sizeof(ulong)];
        NextBytes(buffer);

        return MemoryMarshal.Read<ulong>(buffer);
    }

    /// <inheritdoc />
    public int NextInt32()
    {
        return _random.Next();
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxValue" /> is less than 0. </exception>
    public int NextInt32(int maxValue)
    {
        // System.Random is exclusive upper.
        int maxActual = maxValue == int.MaxValue ? int.MaxValue : maxValue + 1;
        return _random.Next(maxActual);
    }

    /// <inheritdoc />
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    public int NextInt32(int minValue, int maxValue)
    {
        var ordered = new OrderedElements<int>(minValue, maxValue);

        // System.Random is exclusive upper.
        int maxActual = ordered.Greater == int.MaxValue ? int.MaxValue : ordered.Greater + 1;

        return _random.Next(ordered.Lesser, maxActual);
    }

    /// <inheritdoc />
    public long NextInt64()
    {
        Span<byte> buffer = stackalloc byte[sizeof(long)];
        NextBytes(buffer);

        return MemoryMarshal.Read<long>(buffer);
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxValue"/> is less than 0.</exception>
    public long NextInt64(long maxValue)
    {
        return (long)(NextDouble(maxValue) * long.MaxValue);
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minValue"/> is less than 0.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxValue"/> is less than 0.</exception>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    public long NextInt64(long minValue, long maxValue)
    {
        return (long)NextDouble(minValue, maxValue);
    }

    /// <inheritdoc />
    public double NextDouble()
    {
        return _random.NextDouble();
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxValue"/> is less than 0.</exception>
    public double NextDouble(double maxValue)
    {
        Guard.IsGreaterThanOrEqualTo(maxValue, 0, nameof(maxValue));

        return _random.NextDouble() * maxValue;
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minValue"/> is less than 0.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxValue"/> is less than 0.</exception>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    public double NextDouble(double minValue, double maxValue)
    {
        Guard.IsGreaterThanOrEqualTo(minValue, 0, nameof(minValue));
        Guard.IsGreaterThanOrEqualTo(maxValue, 0, nameof(maxValue));

        var ordered = new OrderedElements<double>(minValue, maxValue);
        return _random.NextDouble() * (ordered.Greater - ordered.Lesser) + ordered.Lesser;
    }

    /// <summary>
    /// Obtains a random boolean.
    /// </summary>
    public bool NextBool()
    {
        Span<byte> buffer = stackalloc byte[sizeof(bool)];
        NextBytes(buffer);

        // Marshaling as a bool doesn't work as one would hope here.
        // We need to check for even/odd here instead.
        return (buffer[0] & 1) != 1;
    }

    /// <summary>
    /// Obtains random bytes.
    /// </summary>
    /// <param name="size">The number of bytes to return.</param>
    public byte[] NextBytes(long size)
    {
        lock (_random)
        {
            byte[] data = new byte[size];
            _random.NextBytes(data);
            return data;
        }
    }

    /// <summary>
    /// Fills <paramref name="buffer" /> with random bytes.
    /// </summary>
    /// <param name="buffer">The buffer to fill.</param>
    public void NextBytes(Span<byte> buffer)
    {
        lock (_random)
        {
            _random.NextBytes(buffer);
        }
    }
}

