using System.Runtime.CompilerServices;

namespace System;

/// <summary>
/// Numeric extensions.
/// </summary>
public static partial class NumericExtensions
{
    /// <summary>
    /// Converts this <see cref="byte"/> value to a <see cref="bool"/>
    /// without branching instructions or casts.
    /// </summary>
    /// <param name="b">The value to convert.</param>
    /// <returns><see langword="false"/> if <paramref name="b"/> equals 0, <see langword="true"/> otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ToBool(this byte b)
    {
        // Create a copy to help prevent register spills. Even if the generated IL is larger it will likely
        // be optimized away by the JIT anyway. This is also faster than using the argument directly.
        byte copy = b;

        return copy != 0;
    }
}
