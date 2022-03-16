using System.Runtime.CompilerServices;

namespace AniNexus;

/// <summary>
/// Number extensions.
/// </summary>
public static partial class NumericExtensions
{
    /// <summary>
    /// Converts this <see cref="bool"/> value to a <see cref="byte"/>
    /// without branching instructions or casts.
    /// </summary>
    /// <param name="b">The value to convert.</param>
    /// <returns>1 if the value is <see langword="true"/>, 0 otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ToByte(this bool b)
    {
        return Unsafe.As<bool, byte>(ref b);
    }

    /// <summary>
    /// Converts this <see cref="bool"/> value to an <see cref="short"/>
    /// without branching instructions or casts.
    /// </summary>
    /// <param name="b">The value to convert.</param>
    /// <returns>1 if the value is <see langword="true"/>, 0 otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static short ToInt16(this bool b)
    {
        return Unsafe.As<bool, byte>(ref b);
    }

    /// <summary>
    /// Converts this <see cref="bool"/> value to an <see cref="int"/>
    /// without branching instructions or casts.
    /// </summary>
    /// <param name="b">The value to convert.</param>
    /// <returns>1 if the value is <see langword="true"/>, 0 otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToInt32(this bool b)
    {
        return Unsafe.As<bool, byte>(ref b);
    }

    /// <summary>
    /// Converts this <see cref="bool"/> value to an <see cref="long"/>
    /// without branching instructions or casts.
    /// </summary>
    /// <param name="b">The value to convert.</param>
    /// <returns>1 if the value is <see langword="true"/>, 0 otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long ToInt64(this bool b)
    {
        return Unsafe.As<bool, byte>(ref b);
    }
}
