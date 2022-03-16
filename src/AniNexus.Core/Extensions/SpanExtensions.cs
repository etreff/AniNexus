using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AniNexus;

public static partial class MemoryExtensions
{
    /// <summary>
    /// Replaces all instances of <paramref name="oldValue"/> in <paramref name="span"/>
    /// with <paramref name="newValue"/>.
    /// </summary>
    /// <param name="span">The span to search.</param>
    /// <param name="oldValue">The value to replace.</param>
    /// <param name="newValue">The new value.</param>
    public static void Replace(this in Span<byte> span, byte oldValue, byte newValue)
    {
        ref byte s0 = ref MemoryMarshal.GetReference(span);

        for (int i = 0; i < span.Length; ++i)
        {
            ref byte si = ref Unsafe.Add(ref s0, i);
            if (si == oldValue)
            {
                si = newValue;
            }
        }
    }

    /// <summary>
    /// Replaces all instances of <paramref name="oldValue"/> in <paramref name="span"/>
    /// with <paramref name="newValue"/>.
    /// </summary>
    /// <param name="span">The span to search.</param>
    /// <param name="oldValue">The value to replace.</param>
    /// <param name="newValue">The new value.</param>
    public static void Replace<T>(this in Span<T?> span, T? oldValue, T? newValue)
        where T : IEquatable<T>
    {
        ref var s0 = ref MemoryMarshal.GetReference(span);

        for (int i = 0; i < span.Length; ++i)
        {
            ref var si = ref Unsafe.Add(ref s0, i);
            if (Unsafe.IsNullRef(ref si))
            {
                if (oldValue is null)
                {
                    si = newValue;
                }
            }
            else if (si!.Equals(oldValue))
            {
                si = newValue;
            }
        }
    }
}
