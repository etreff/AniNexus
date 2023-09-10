using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using AniNexus.Helpers;

/***
 * NOTE
 *
 * This class has a lot of duplication of code. This is intentional due a trade-off
 * between assembly size and speed. We are opting for pure speed here, and merging
 * the different functions will lead to significant overhead due to branching.
 *
 * Many of these extensions are taken from third party sources. Special credit is due
 * to the folks at Microsoft as many of these methods are internal helper methods that have
 * been tweaked for general/public use.
 */

namespace System;

/// <summary>
/// <see cref="Span{T}"/>, <see cref="ReadOnlySpan{T}"/>, <see cref="Memory{T}"/>, and <see cref="ReadOnlyMemory{T}"/> extensions.
/// </summary>
public static partial class MemoryExtensions
{
    private const ulong _xorPowerOfTwoToHighByte = (0x07ul | 0x06ul << 8 | 0x05ul << 16 | 0x04ul << 24 | 0x03ul << 32 | 0x02ul << 40 | 0x01ul << 48) + 1;

    private const byte _whiteSpaceByte = (byte)' ';

    /// <summary>
    /// Returns the index of the first byte(s) that match the value specified in <paramref name="value"/>.
    /// </summary>
    /// <param name="data">The data to search.</param>
    /// <param name="value">The value to find the index of.</param>
    /// <returns>The index if found, -1 otherwise.</returns>
    /// <remarks>
    /// This method uses vectorized searching using intrinsics so should be preferred over iterating each
    /// byte.
    /// </remarks>
    public static unsafe int IndexOf(this ReadOnlySpan<byte> data, uint value)
    {
        if (data.Length == 0)
        {
            return -1;
        }

        ref byte searchSpace = ref MemoryMarshal.GetReference(data);

        nint index = (IntPtr)0;
        nint length = (IntPtr)data.Length;

        if (Vector.IsHardwareAccelerated && data.Length >= Vector<byte>.Count * 2)
        {
            int unaligned = (int)Unsafe.AsPointer(ref searchSpace) & (Vector<byte>.Count - 1);
            length = (IntPtr)((Vector<byte>.Count - unaligned) & (Vector<byte>.Count - 1));
        }

        SequentialScan:
        uint lookUp;
        while ((byte*)length >= (byte*)8)
        {
            length -= 8;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp == value)
            {
                goto Found;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 1);
            if (lookUp == value)
            {
                goto Found1;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 2);
            if (lookUp == value)
            {
                goto Found2;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 3);
            if (lookUp == value)
            {
                goto Found3;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 4);
            if (lookUp == value)
            {
                goto Found4;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 5);
            if (lookUp == value)
            {
                goto Found5;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 6);
            if (lookUp == value)
            {
                goto Found6;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 7);
            if (lookUp == value)
            {
                goto Found7;
            }

            index += 8;
        }

        if ((byte*)length >= (byte*)4)
        {
            length -= 4;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp == value)
            {
                goto Found;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 1);
            if (lookUp == value)
            {
                goto Found1;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 2);
            if (lookUp == value)
            {
                goto Found2;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 3);
            if (lookUp == value)
            {
                goto Found3;
            }

            index += 4;
        }

        while ((byte*)length > (byte*)0)
        {
            length -= 1;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp == value)
            {
                goto Found;
            }

            length += 1;
        }

        if (Vector.IsHardwareAccelerated && ((int)(byte*)index < data.Length))
        {
            if ((int)(byte*)index < data.Length)
            {
                length -= (int)(byte*)index;
                goto SequentialScan;
            }
        }

        return -1;

        Found: // Workaround for https://github.com/dotnet/runtime/issues/8795
        return (int)(byte*)index;
        Found1:
        return (int)(byte*)(index + 1);
        Found2:
        return (int)(byte*)(index + 2);
        Found3:
        return (int)(byte*)(index + 3);
        Found4:
        return (int)(byte*)(index + 4);
        Found5:
        return (int)(byte*)(index + 5);
        Found6:
        return (int)(byte*)(index + 6);
        Found7:
        return (int)(byte*)(index + 7);
    }

    /// <summary>
    /// Returns the index of the first byte(s) that match the value specified in <paramref name="valueA"/> or
    /// <paramref name="valueB"/>.
    /// </summary>
    /// <param name="data">The data to search.</param>
    /// <param name="valueA">The first valid value to find the index of.</param>
    /// <param name="valueB">The second valid value to find the index of.</param>
    /// <returns>The index if found, -1 otherwise.</returns>
    /// <remarks>
    /// This method uses vectorized searching using intrinsics so should be preferred over iterating each
    /// byte.
    /// </remarks>
    public static unsafe int IndexOf(this ReadOnlySpan<byte> data, uint valueA, uint valueB)
    {
        if (data.Length == 0)
        {
            return -1;
        }

        ref byte searchSpace = ref MemoryMarshal.GetReference(data);

        nint index = (IntPtr)0;
        nint length = (IntPtr)data.Length;

        if (Vector.IsHardwareAccelerated && data.Length >= Vector<byte>.Count * 2)
        {
            int unaligned = (int)Unsafe.AsPointer(ref searchSpace) & (Vector<byte>.Count - 1);
            length = (IntPtr)((Vector<byte>.Count - unaligned) & (Vector<byte>.Count - 1));
        }

        SequentialScan:
        uint lookUp;
        while ((byte*)length >= (byte*)8)
        {
            length -= 8;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp == valueA || lookUp == valueB)
            {
                goto Found;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 1);
            if (lookUp == valueA || lookUp == valueB)
            {
                goto Found1;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 2);
            if (lookUp == valueA || lookUp == valueB)
            {
                goto Found2;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 3);
            if (lookUp == valueA || lookUp == valueB)
            {
                goto Found3;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 4);
            if (lookUp == valueA || lookUp == valueB)
            {
                goto Found4;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 5);
            if (lookUp == valueA || lookUp == valueB)
            {
                goto Found5;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 6);
            if (lookUp == valueA || lookUp == valueB)
            {
                goto Found6;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 7);
            if (lookUp == valueA || lookUp == valueB)
            {
                goto Found7;
            }

            index += 8;
        }

        if ((byte*)length >= (byte*)4)
        {
            length -= 4;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp == valueA || lookUp == valueB)
            {
                goto Found;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 1);
            if (lookUp == valueA || lookUp == valueB)
            {
                goto Found1;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 2);
            if (lookUp == valueA || lookUp == valueB)
            {
                goto Found2;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 3);
            if (lookUp == valueA || lookUp == valueB)
            {
                goto Found3;
            }

            index += 4;
        }

        while ((byte*)length > (byte*)0)
        {
            length -= 1;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp == valueA || lookUp == valueB)
            {
                goto Found;
            }

            length += 1;
        }

        if (Vector.IsHardwareAccelerated && ((int)(byte*)index < data.Length))
        {
            if ((int)(byte*)index < data.Length)
            {
                length -= (int)(byte*)index;
                goto SequentialScan;
            }
        }

        return -1;

        Found: // Workaround for https://github.com/dotnet/runtime/issues/8795
        return (int)(byte*)index;
        Found1:
        return (int)(byte*)(index + 1);
        Found2:
        return (int)(byte*)(index + 2);
        Found3:
        return (int)(byte*)(index + 3);
        Found4:
        return (int)(byte*)(index + 4);
        Found5:
        return (int)(byte*)(index + 5);
        Found6:
        return (int)(byte*)(index + 6);
        Found7:
        return (int)(byte*)(index + 7);
    }

    /// <summary>
    /// Returns the index of the first byte(s) that match the value specified in <paramref name="valueA"/>,
    /// <paramref name="valueB"/>, or <paramref name="valueC"/>.
    /// </summary>
    /// <param name="data">The data to search.</param>
    /// <param name="valueA">The first valid value to find the index of.</param>
    /// <param name="valueB">The second valid value to find the index of.</param>
    /// <param name="valueC">The third valid value to find the index of.</param>
    /// <returns>The index if found, -1 otherwise.</returns>
    /// <remarks>
    /// This method uses vectorized searching using intrinsics so should be preferred over iterating each
    /// byte.
    /// </remarks>
    public static unsafe int IndexOf(this ReadOnlySpan<byte> data, uint valueA, uint valueB, uint valueC)
    {
        if (data.Length == 0)
        {
            return -1;
        }

        ref byte searchSpace = ref MemoryMarshal.GetReference(data);

        nint index = (IntPtr)0;
        nint length = (IntPtr)data.Length;

        if (Vector.IsHardwareAccelerated && data.Length >= Vector<byte>.Count * 2)
        {
            int unaligned = (int)Unsafe.AsPointer(ref searchSpace) & (Vector<byte>.Count - 1);
            length = (IntPtr)((Vector<byte>.Count - unaligned) & (Vector<byte>.Count - 1));
        }

        SequentialScan:
        uint lookUp;
        while ((byte*)length >= (byte*)8)
        {
            length -= 8;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp == valueA || lookUp == valueB || lookUp == valueC)
            {
                goto Found;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 1);
            if (lookUp == valueA || lookUp == valueB || lookUp == valueC)
            {
                goto Found1;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 2);
            if (lookUp == valueA || lookUp == valueB || lookUp == valueC)
            {
                goto Found2;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 3);
            if (lookUp == valueA || lookUp == valueB || lookUp == valueC)
            {
                goto Found3;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 4);
            if (lookUp == valueA || lookUp == valueB || lookUp == valueC)
            {
                goto Found4;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 5);
            if (lookUp == valueA || lookUp == valueB || lookUp == valueC)
            {
                goto Found5;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 6);
            if (lookUp == valueA || lookUp == valueB || lookUp == valueC)
            {
                goto Found6;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 7);
            if (lookUp == valueA || lookUp == valueB || lookUp == valueC)
            {
                goto Found7;
            }

            index += 8;
        }

        if ((byte*)length >= (byte*)4)
        {
            length -= 4;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp == valueA || lookUp == valueB || lookUp == valueC)
            {
                goto Found;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 1);
            if (lookUp == valueA || lookUp == valueB || lookUp == valueC)
            {
                goto Found1;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 2);
            if (lookUp == valueA || lookUp == valueB || lookUp == valueC)
            {
                goto Found2;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 3);
            if (lookUp == valueA || lookUp == valueB || lookUp == valueC)
            {
                goto Found3;
            }

            index += 4;
        }

        while ((byte*)length > (byte*)0)
        {
            length -= 1;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp == valueA || lookUp == valueB || lookUp == valueC)
            {
                goto Found;
            }

            length += 1;
        }

        if (Vector.IsHardwareAccelerated && ((int)(byte*)index < data.Length))
        {
            if ((int)(byte*)index < data.Length)
            {
                length -= (int)(byte*)index;
                goto SequentialScan;
            }
        }

        return -1;

        Found: // Workaround for https://github.com/dotnet/runtime/issues/8795
        return (int)(byte*)index;
        Found1:
        return (int)(byte*)(index + 1);
        Found2:
        return (int)(byte*)(index + 2);
        Found3:
        return (int)(byte*)(index + 3);
        Found4:
        return (int)(byte*)(index + 4);
        Found5:
        return (int)(byte*)(index + 5);
        Found6:
        return (int)(byte*)(index + 6);
        Found7:
        return (int)(byte*)(index + 7);
    }

    /// <summary>
    /// Returns the index of the first byte(s) that are less than the value specified in <paramref name="lessThan"/>.
    /// </summary>
    /// <param name="data">The data to search.</param>
    /// <param name="lessThan">The upper exclusive valid value.</param>
    /// <returns>The index if found, -1 otherwise.</returns>
    /// <remarks>
    /// This method uses vectorized searching using intrinsics so should be preferred over iterating each
    /// byte.
    /// </remarks>
    public static unsafe int IndexOfLessThan(this ReadOnlySpan<byte> data, uint lessThan)
    {
        if (data.Length == 0)
        {
            return -1;
        }

        ref byte searchSpace = ref MemoryMarshal.GetReference(data);

        nint index = (IntPtr)0;
        nint length = (IntPtr)data.Length;

        if (Vector.IsHardwareAccelerated && data.Length >= Vector<byte>.Count * 2)
        {
            int unaligned = (int)Unsafe.AsPointer(ref searchSpace) & (Vector<byte>.Count - 1);
            length = (IntPtr)((Vector<byte>.Count - unaligned) & (Vector<byte>.Count - 1));
        }

        SequentialScan:
        uint lookUp;
        while ((byte*)length >= (byte*)8)
        {
            length -= 8;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp < lessThan)
            {
                goto Found;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 1);
            if (lookUp < lessThan)
            {
                goto Found1;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 2);
            if (lookUp < lessThan)
            {
                goto Found2;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 3);
            if (lookUp < lessThan)
            {
                goto Found3;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 4);
            if (lookUp < lessThan)
            {
                goto Found4;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 5);
            if (lookUp < lessThan)
            {
                goto Found5;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 6);
            if (lookUp < lessThan)
            {
                goto Found6;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 7);
            if (lookUp < lessThan)
            {
                goto Found7;
            }

            index += 8;
        }

        if ((byte*)length >= (byte*)4)
        {
            length -= 4;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp < lessThan)
            {
                goto Found;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 1);
            if (lookUp < lessThan)
            {
                goto Found1;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 2);
            if (lookUp < lessThan)
            {
                goto Found2;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 3);
            if (lookUp < lessThan)
            {
                goto Found3;
            }

            index += 4;
        }

        while ((byte*)length > (byte*)0)
        {
            length -= 1;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp < lessThan)
            {
                goto Found;
            }

            length += 1;
        }

        if (Vector.IsHardwareAccelerated && ((int)(byte*)index < data.Length))
        {
            length = (IntPtr)((data.Length - (int)(byte*)index) & ~(Vector<byte>.Count - 1));

            var valuesLessThan = new Vector<byte>((byte)lessThan);

            while ((byte*)length > (byte*)index)
            {
                var vData = Unsafe.ReadUnaligned<Vector<byte>>(ref Unsafe.AddByteOffset(ref searchSpace, index));
                var vMatches = Vector.LessThan(vData, valuesLessThan);

                if (Vector<byte>.Zero.Equals(vMatches))
                {
                    index += Vector<byte>.Count;
                    continue;
                }

                return (int)(byte*)index + LocateFirstFoundByte(vMatches);
            }

            if ((int)(byte*)index < data.Length)
            {
                length -= (int)(byte*)index;
                goto SequentialScan;
            }
        }

        return -1;

        Found: // Workaround for https://github.com/dotnet/runtime/issues/8795
        return (int)(byte*)index;
        Found1:
        return (int)(byte*)(index + 1);
        Found2:
        return (int)(byte*)(index + 2);
        Found3:
        return (int)(byte*)(index + 3);
        Found4:
        return (int)(byte*)(index + 4);
        Found5:
        return (int)(byte*)(index + 5);
        Found6:
        return (int)(byte*)(index + 6);
        Found7:
        return (int)(byte*)(index + 7);
    }

    /// <summary>
    /// Returns the index of the first byte(s) that are greater than the value specified in <paramref name="greaterThan"/>.
    /// </summary>
    /// <param name="data">The data to search.</param>
    /// <param name="greaterThan">The lower exclusive valid value.</param>
    /// <returns>The index if found, -1 otherwise.</returns>
    /// <remarks>
    /// This method uses vectorized searching using intrinsics so should be preferred over iterating each
    /// byte.
    /// </remarks>
    public static unsafe int IndexOfGreaterThan(this ReadOnlySpan<byte> data, uint greaterThan)
    {
        if (data.Length == 0)
        {
            return -1;
        }

        ref byte searchSpace = ref MemoryMarshal.GetReference(data);

        nint index = (IntPtr)0;
        nint length = (IntPtr)data.Length;

        if (Vector.IsHardwareAccelerated && data.Length >= Vector<byte>.Count * 2)
        {
            int unaligned = (int)Unsafe.AsPointer(ref searchSpace) & (Vector<byte>.Count - 1);
            length = (IntPtr)((Vector<byte>.Count - unaligned) & (Vector<byte>.Count - 1));
        }

        SequentialScan:
        uint lookUp;
        while ((byte*)length >= (byte*)8)
        {
            length -= 8;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp > greaterThan)
            {
                goto Found;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 1);
            if (lookUp > greaterThan)
            {
                goto Found1;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 2);
            if (lookUp > greaterThan)
            {
                goto Found2;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 3);
            if (lookUp > greaterThan)
            {
                goto Found3;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 4);
            if (lookUp > greaterThan)
            {
                goto Found4;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 5);
            if (lookUp > greaterThan)
            {
                goto Found5;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 6);
            if (lookUp > greaterThan)
            {
                goto Found6;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 7);
            if (lookUp > greaterThan)
            {
                goto Found7;
            }

            index += 8;
        }

        if ((byte*)length >= (byte*)4)
        {
            length -= 4;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp > greaterThan)
            {
                goto Found;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 1);
            if (lookUp > greaterThan)
            {
                goto Found1;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 2);
            if (lookUp > greaterThan)
            {
                goto Found2;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 3);
            if (lookUp > greaterThan)
            {
                goto Found3;
            }

            index += 4;
        }

        while ((byte*)length > (byte*)0)
        {
            length -= 1;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp > greaterThan)
            {
                goto Found;
            }

            length += 1;
        }

        if (Vector.IsHardwareAccelerated && ((int)(byte*)index < data.Length))
        {
            length = (IntPtr)((data.Length - (int)(byte*)index) & ~(Vector<byte>.Count - 1));

            var valuesLessThan = new Vector<byte>((byte)greaterThan);

            while ((byte*)length > (byte*)index)
            {
                var vData = Unsafe.ReadUnaligned<Vector<byte>>(ref Unsafe.AddByteOffset(ref searchSpace, index));
                var vMatches = Vector.GreaterThan(vData, valuesLessThan);

                if (Vector<byte>.Zero.Equals(vMatches))
                {
                    index += Vector<byte>.Count;
                    continue;
                }

                return (int)(byte*)index + LocateFirstFoundByte(vMatches);
            }

            if ((int)(byte*)index < data.Length)
            {
                length -= (int)(byte*)index;
                goto SequentialScan;
            }
        }

        return -1;

        Found: // Workaround for https://github.com/dotnet/runtime/issues/8795
        return (int)(byte*)index;
        Found1:
        return (int)(byte*)(index + 1);
        Found2:
        return (int)(byte*)(index + 2);
        Found3:
        return (int)(byte*)(index + 3);
        Found4:
        return (int)(byte*)(index + 4);
        Found5:
        return (int)(byte*)(index + 5);
        Found6:
        return (int)(byte*)(index + 6);
        Found7:
        return (int)(byte*)(index + 7);
    }

    /// <summary>
    /// Returns the index of the first byte(s) that equal <paramref name="value"/> or are less than the value
    /// specified in <paramref name="lessThan"/>.
    /// </summary>
    /// <param name="data">The data to search.</param>
    /// <param name="value">A valid value to find the index of.</param>
    /// <param name="lessThan">The upper exclusive valid value.</param>
    /// <returns>The index if found, -1 otherwise.</returns>
    /// <remarks>
    /// This method uses vectorized searching using intrinsics so should be preferred over iterating each
    /// byte.
    /// </remarks>
    public static unsafe int IndexOfOrLessThan(this ReadOnlySpan<byte> data, uint value, uint lessThan)
    {
        if (data.Length == 0)
        {
            return -1;
        }

        ref byte searchSpace = ref MemoryMarshal.GetReference(data);

        nint index = (IntPtr)0;
        nint length = (IntPtr)data.Length;

        if (Vector.IsHardwareAccelerated && data.Length >= Vector<byte>.Count * 2)
        {
            int unaligned = (int)Unsafe.AsPointer(ref searchSpace) & (Vector<byte>.Count - 1);
            length = (IntPtr)((Vector<byte>.Count - unaligned) & (Vector<byte>.Count - 1));
        }

        SequentialScan:
        uint lookUp;
        while ((byte*)length >= (byte*)8)
        {
            length -= 8;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp == value || lookUp < lessThan)
            {
                goto Found;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 1);
            if (lookUp == value || lookUp < lessThan)
            {
                goto Found1;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 2);
            if (lookUp == value || lookUp < lessThan)
            {
                goto Found2;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 3);
            if (lookUp == value || lookUp < lessThan)
            {
                goto Found3;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 4);
            if (lookUp == value || lookUp < lessThan)
            {
                goto Found4;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 5);
            if (lookUp == value || lookUp < lessThan)
            {
                goto Found5;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 6);
            if (lookUp == value || lookUp < lessThan)
            {
                goto Found6;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 7);
            if (lookUp == value || lookUp < lessThan)
            {
                goto Found7;
            }

            index += 8;
        }

        if ((byte*)length >= (byte*)4)
        {
            length -= 4;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp == value || lookUp < lessThan)
            {
                goto Found;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 1);
            if (lookUp == value || lookUp < lessThan)
            {
                goto Found1;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 2);
            if (lookUp == value || lookUp < lessThan)
            {
                goto Found2;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 3);
            if (lookUp == value || lookUp < lessThan)
            {
                goto Found3;
            }

            index += 4;
        }

        while ((byte*)length > (byte*)0)
        {
            length -= 1;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp == value || lookUp < lessThan)
            {
                goto Found;
            }

            length += 1;
        }

        if (Vector.IsHardwareAccelerated && ((int)(byte*)index < data.Length))
        {
            length = (IntPtr)((data.Length - (int)(byte*)index) & ~(Vector<byte>.Count - 1));

            var valuesLessThan = new Vector<byte>((byte)lessThan);

            while ((byte*)length > (byte*)index)
            {
                var vData = Unsafe.ReadUnaligned<Vector<byte>>(ref Unsafe.AddByteOffset(ref searchSpace, index));
                var vMatches = Vector.LessThan(vData, valuesLessThan);

                if (Vector<byte>.Zero.Equals(vMatches))
                {
                    index += Vector<byte>.Count;
                    continue;
                }

                return (int)(byte*)index + LocateFirstFoundByte(vMatches);
            }

            if ((int)(byte*)index < data.Length)
            {
                length -= (int)(byte*)index;
                goto SequentialScan;
            }
        }

        return -1;

        Found: // Workaround for https://github.com/dotnet/runtime/issues/8795
        return (int)(byte*)index;
        Found1:
        return (int)(byte*)(index + 1);
        Found2:
        return (int)(byte*)(index + 2);
        Found3:
        return (int)(byte*)(index + 3);
        Found4:
        return (int)(byte*)(index + 4);
        Found5:
        return (int)(byte*)(index + 5);
        Found6:
        return (int)(byte*)(index + 6);
        Found7:
        return (int)(byte*)(index + 7);
    }

    /// <summary>
    /// Returns the index of the first byte(s) that equal <paramref name="valueA"/> or <paramref name="valueB"/>,
    /// or are less than the value specified in <paramref name="lessThan"/>.
    /// </summary>
    /// <param name="data">The data to search.</param>
    /// <param name="valueA">The first valid value to find the index of.</param>
    /// <param name="valueB">The second valid value to find the index of.</param>
    /// <param name="lessThan">The upper exclusive valid value.</param>
    /// <returns>The index if found, -1 otherwise.</returns>
    /// <remarks>
    /// This method uses vectorized searching using intrinsics so should be preferred over iterating each
    /// byte.
    /// </remarks>
    public static unsafe int IndexOfOrLessThan(this ReadOnlySpan<byte> data, uint valueA, uint valueB, uint lessThan)
    {
        if (data.Length == 0)
        {
            return -1;
        }

        ref byte searchSpace = ref MemoryMarshal.GetReference(data);

        nint index = (IntPtr)0;
        nint length = (IntPtr)data.Length;

        if (Vector.IsHardwareAccelerated && data.Length >= Vector<byte>.Count * 2)
        {
            int unaligned = (int)Unsafe.AsPointer(ref searchSpace) & (Vector<byte>.Count - 1);
            length = (IntPtr)((Vector<byte>.Count - unaligned) & (Vector<byte>.Count - 1));
        }

        SequentialScan:
        uint lookUp;
        while ((byte*)length >= (byte*)8)
        {
            length -= 8;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp == valueA || lookUp == valueB || lookUp < lessThan)
            {
                goto Found;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 1);
            if (lookUp == valueA || lookUp == valueB || lookUp < lessThan)
            {
                goto Found1;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 2);
            if (lookUp == valueA || lookUp == valueB || lookUp < lessThan)
            {
                goto Found2;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 3);
            if (lookUp == valueA || lookUp == valueB || lookUp < lessThan)
            {
                goto Found3;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 4);
            if (lookUp == valueA || lookUp == valueB || lookUp < lessThan)
            {
                goto Found4;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 5);
            if (lookUp == valueA || lookUp == valueB || lookUp < lessThan)
            {
                goto Found5;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 6);
            if (lookUp == valueA || lookUp == valueB || lookUp < lessThan)
            {
                goto Found6;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 7);
            if (lookUp == valueA || lookUp == valueB || lookUp < lessThan)
            {
                goto Found7;
            }

            index += 8;
        }

        if ((byte*)length >= (byte*)4)
        {
            length -= 4;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp == valueA || lookUp == valueB || lookUp < lessThan)
            {
                goto Found;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 1);
            if (lookUp == valueA || lookUp == valueB || lookUp < lessThan)
            {
                goto Found1;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 2);
            if (lookUp == valueA || lookUp == valueB || lookUp < lessThan)
            {
                goto Found2;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 3);
            if (lookUp == valueA || lookUp == valueB || lookUp < lessThan)
            {
                goto Found3;
            }

            index += 4;
        }

        while ((byte*)length > (byte*)0)
        {
            length -= 1;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp == valueA || lookUp == valueB || lookUp < lessThan)
            {
                goto Found;
            }

            length += 1;
        }

        if (Vector.IsHardwareAccelerated && ((int)(byte*)index < data.Length))
        {
            length = (IntPtr)((data.Length - (int)(byte*)index) & ~(Vector<byte>.Count - 1));

            var valuesLessThan = new Vector<byte>((byte)lessThan);

            while ((byte*)length > (byte*)index)
            {
                var vData = Unsafe.ReadUnaligned<Vector<byte>>(ref Unsafe.AddByteOffset(ref searchSpace, index));
                var vMatches = Vector.LessThan(vData, valuesLessThan);

                if (Vector<byte>.Zero.Equals(vMatches))
                {
                    index += Vector<byte>.Count;
                    continue;
                }

                return (int)(byte*)index + LocateFirstFoundByte(vMatches);
            }

            if ((int)(byte*)index < data.Length)
            {
                length -= (int)(byte*)index;
                goto SequentialScan;
            }
        }

        return -1;

        Found: // Workaround for https://github.com/dotnet/runtime/issues/8795
        return (int)(byte*)index;
        Found1:
        return (int)(byte*)(index + 1);
        Found2:
        return (int)(byte*)(index + 2);
        Found3:
        return (int)(byte*)(index + 3);
        Found4:
        return (int)(byte*)(index + 4);
        Found5:
        return (int)(byte*)(index + 5);
        Found6:
        return (int)(byte*)(index + 6);
        Found7:
        return (int)(byte*)(index + 7);
    }

    /// <summary>
    /// Returns the index of the first byte(s) that equal <paramref name="value"/> or are greater than the value
    /// specified in <paramref name="greaterThan"/>.
    /// </summary>
    /// <param name="data">The data to search.</param>
    /// <param name="value">A valid value to find the index of.</param>
    /// <param name="greaterThan">The lower exclusive valid value.</param>
    /// <returns>The index if found, -1 otherwise.</returns>
    /// <remarks>
    /// This method uses vectorized searching using intrinsics so should be preferred over iterating each
    /// byte.
    /// </remarks>
    public static unsafe int IndexOfOrGreaterThan(this ReadOnlySpan<byte> data, uint value, uint greaterThan)
    {
        if (data.Length == 0)
        {
            return -1;
        }

        ref byte searchSpace = ref MemoryMarshal.GetReference(data);

        nint index = (IntPtr)0;
        nint length = (IntPtr)data.Length;

        if (Vector.IsHardwareAccelerated && data.Length >= Vector<byte>.Count * 2)
        {
            int unaligned = (int)Unsafe.AsPointer(ref searchSpace) & (Vector<byte>.Count - 1);
            length = (IntPtr)((Vector<byte>.Count - unaligned) & (Vector<byte>.Count - 1));
        }

        SequentialScan:
        uint lookUp;
        while ((byte*)length >= (byte*)8)
        {
            length -= 8;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp == value || lookUp > greaterThan)
            {
                goto Found;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 1);
            if (lookUp == value || lookUp > greaterThan)
            {
                goto Found1;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 2);
            if (lookUp == value || lookUp > greaterThan)
            {
                goto Found2;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 3);
            if (lookUp == value || lookUp > greaterThan)
            {
                goto Found3;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 4);
            if (lookUp == value || lookUp > greaterThan)
            {
                goto Found4;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 5);
            if (lookUp == value || lookUp > greaterThan)
            {
                goto Found5;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 6);
            if (lookUp == value || lookUp > greaterThan)
            {
                goto Found6;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 7);
            if (lookUp == value || lookUp > greaterThan)
            {
                goto Found7;
            }

            index += 8;
        }

        if ((byte*)length >= (byte*)4)
        {
            length -= 4;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp == value || lookUp > greaterThan)
            {
                goto Found;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 1);
            if (lookUp == value || lookUp > greaterThan)
            {
                goto Found1;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 2);
            if (lookUp == value || lookUp > greaterThan)
            {
                goto Found2;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 3);
            if (lookUp == value || lookUp > greaterThan)
            {
                goto Found3;
            }

            index += 4;
        }

        while ((byte*)length > (byte*)0)
        {
            length -= 1;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp == value || lookUp > greaterThan)
            {
                goto Found;
            }

            length += 1;
        }

        if (Vector.IsHardwareAccelerated && ((int)(byte*)index < data.Length))
        {
            length = (IntPtr)((data.Length - (int)(byte*)index) & ~(Vector<byte>.Count - 1));

            var valuesLessThan = new Vector<byte>((byte)greaterThan);

            while ((byte*)length > (byte*)index)
            {
                var vData = Unsafe.ReadUnaligned<Vector<byte>>(ref Unsafe.AddByteOffset(ref searchSpace, index));
                var vMatches = Vector.GreaterThan(vData, valuesLessThan);

                if (Vector<byte>.Zero.Equals(vMatches))
                {
                    index += Vector<byte>.Count;
                    continue;
                }

                return (int)(byte*)index + LocateFirstFoundByte(vMatches);
            }

            if ((int)(byte*)index < data.Length)
            {
                length -= (int)(byte*)index;
                goto SequentialScan;
            }
        }

        return -1;

        Found: // Workaround for https://github.com/dotnet/runtime/issues/8795
        return (int)(byte*)index;
        Found1:
        return (int)(byte*)(index + 1);
        Found2:
        return (int)(byte*)(index + 2);
        Found3:
        return (int)(byte*)(index + 3);
        Found4:
        return (int)(byte*)(index + 4);
        Found5:
        return (int)(byte*)(index + 5);
        Found6:
        return (int)(byte*)(index + 6);
        Found7:
        return (int)(byte*)(index + 7);
    }

    /// <summary>
    /// Returns the index of the first byte(s) that equal <paramref name="valueA"/> or <paramref name="valueB"/> or are greater than the value
    /// specified in <paramref name="greaterThan"/>.
    /// </summary>
    /// <param name="data">The data to search.</param>
    /// <param name="valueA">The first valid value to find the index of.</param>
    /// <param name="valueB">The second valid value to find the index of.</param>
    /// <param name="greaterThan">The lower exclusive valid value.</param>
    /// <returns>The index if found, -1 otherwise.</returns>
    /// <remarks>
    /// This method uses vectorized searching using intrinsics so should be preferred over iterating each
    /// byte.
    /// </remarks>
    public static unsafe int IndexOfOrGreaterThan(this ReadOnlySpan<byte> data, uint valueA, uint valueB, uint greaterThan)
    {
        if (data.Length == 0)
        {
            return -1;
        }

        ref byte searchSpace = ref MemoryMarshal.GetReference(data);

        nint index = (IntPtr)0;
        nint length = (IntPtr)data.Length;

        if (Vector.IsHardwareAccelerated && data.Length >= Vector<byte>.Count * 2)
        {
            int unaligned = (int)Unsafe.AsPointer(ref searchSpace) & (Vector<byte>.Count - 1);
            length = (IntPtr)((Vector<byte>.Count - unaligned) & (Vector<byte>.Count - 1));
        }

        SequentialScan:
        uint lookUp;
        while ((byte*)length >= (byte*)8)
        {
            length -= 8;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp == valueA || lookUp == valueB || lookUp > greaterThan)
            {
                goto Found;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 1);
            if (lookUp == valueA || lookUp == valueB || lookUp > greaterThan)
            {
                goto Found1;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 2);
            if (lookUp == valueA || lookUp == valueB || lookUp > greaterThan)
            {
                goto Found2;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 3);
            if (lookUp == valueA || lookUp == valueB || lookUp > greaterThan)
            {
                goto Found3;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 4);
            if (lookUp == valueA || lookUp == valueB || lookUp > greaterThan)
            {
                goto Found4;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 5);
            if (lookUp == valueA || lookUp == valueB || lookUp > greaterThan)
            {
                goto Found5;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 6);
            if (lookUp == valueA || lookUp == valueB || lookUp > greaterThan)
            {
                goto Found6;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 7);
            if (lookUp == valueA || lookUp == valueB || lookUp > greaterThan)
            {
                goto Found7;
            }

            index += 8;
        }

        if ((byte*)length >= (byte*)4)
        {
            length -= 4;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp == valueA || lookUp == valueB || lookUp > greaterThan)
            {
                goto Found;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 1);
            if (lookUp == valueA || lookUp == valueB || lookUp > greaterThan)
            {
                goto Found1;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 2);
            if (lookUp == valueA || lookUp == valueB || lookUp > greaterThan)
            {
                goto Found2;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 3);
            if (lookUp == valueA || lookUp == valueB || lookUp > greaterThan)
            {
                goto Found3;
            }

            index += 4;
        }

        while ((byte*)length > (byte*)0)
        {
            length -= 1;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp == valueA || lookUp == valueB || lookUp > greaterThan)
            {
                goto Found;
            }

            length += 1;
        }

        if (Vector.IsHardwareAccelerated && ((int)(byte*)index < data.Length))
        {
            length = (IntPtr)((data.Length - (int)(byte*)index) & ~(Vector<byte>.Count - 1));

            var valuesLessThan = new Vector<byte>((byte)greaterThan);

            while ((byte*)length > (byte*)index)
            {
                var vData = Unsafe.ReadUnaligned<Vector<byte>>(ref Unsafe.AddByteOffset(ref searchSpace, index));
                var vMatches = Vector.GreaterThan(vData, valuesLessThan);

                if (Vector<byte>.Zero.Equals(vMatches))
                {
                    index += Vector<byte>.Count;
                    continue;
                }

                return (int)(byte*)index + LocateFirstFoundByte(vMatches);
            }

            if ((int)(byte*)index < data.Length)
            {
                length -= (int)(byte*)index;
                goto SequentialScan;
            }
        }

        return -1;

        Found: // Workaround for https://github.com/dotnet/runtime/issues/8795
        return (int)(byte*)index;
        Found1:
        return (int)(byte*)(index + 1);
        Found2:
        return (int)(byte*)(index + 2);
        Found3:
        return (int)(byte*)(index + 3);
        Found4:
        return (int)(byte*)(index + 4);
        Found5:
        return (int)(byte*)(index + 5);
        Found6:
        return (int)(byte*)(index + 6);
        Found7:
        return (int)(byte*)(index + 7);
    }

    /// <summary>
    /// Returns the index of the first byte(s) that does not match the value specified in <paramref name="value"/>.
    /// </summary>
    /// <param name="data">The data to search.</param>
    /// <param name="value">The value to skip.</param>
    /// <returns>The index if found, -1 otherwise.</returns>
    /// <remarks>
    /// This method uses vectorized searching using intrinsics so should be preferred over iterating each
    /// byte.
    /// </remarks>
    public static unsafe int IndexOfNot(this ReadOnlySpan<byte> data, uint value)
    {
        if (data.Length == 0)
        {
            return -1;
        }

        ref byte searchSpace = ref MemoryMarshal.GetReference(data);

        nint index = (IntPtr)0;
        nint length = (IntPtr)data.Length;

        if (Vector.IsHardwareAccelerated && data.Length >= Vector<byte>.Count * 2)
        {
            int unaligned = (int)Unsafe.AsPointer(ref searchSpace) & (Vector<byte>.Count - 1);
            length = (IntPtr)((Vector<byte>.Count - unaligned) & (Vector<byte>.Count - 1));
        }

        SequentialScan:
        uint lookUp;
        while ((byte*)length >= (byte*)8)
        {
            length -= 8;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp != value)
            {
                goto Found;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 1);
            if (lookUp != value)
            {
                goto Found1;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 2);
            if (lookUp != value)
            {
                goto Found2;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 3);
            if (lookUp != value)
            {
                goto Found3;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 4);
            if (lookUp != value)
            {
                goto Found4;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 5);
            if (lookUp != value)
            {
                goto Found5;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 6);
            if (lookUp != value)
            {
                goto Found6;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 7);
            if (lookUp != value)
            {
                goto Found7;
            }

            index += 8;
        }

        if ((byte*)length >= (byte*)4)
        {
            length -= 4;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp != value)
            {
                goto Found;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 1);
            if (lookUp != value)
            {
                goto Found1;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 2);
            if (lookUp != value)
            {
                goto Found2;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 3);
            if (lookUp != value)
            {
                goto Found3;
            }

            index += 4;
        }

        while ((byte*)length > (byte*)0)
        {
            length -= 1;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp != value)
            {
                goto Found;
            }

            length += 1;
        }

        if (Vector.IsHardwareAccelerated && ((int)(byte*)index < data.Length))
        {
            length = (IntPtr)((data.Length - (int)(byte*)index) & ~(Vector<byte>.Count - 1));

            var vValue = new Vector<byte>((byte)value);

            while ((byte*)length > (byte*)index)
            {
                var vData = Unsafe.ReadUnaligned<Vector<byte>>(ref Unsafe.AddByteOffset(ref searchSpace, index));
                var vGtMatches = Vector.GreaterThan(vData, vValue);

                if (Vector<byte>.Zero.Equals(vGtMatches))
                {
                    var vLtMatches = Vector.LessThan(vData, vValue);
                    if (Vector<byte>.Zero.Equals(vLtMatches))
                    {
                        index += Vector<byte>.Count;
                    }
                    else
                    {
                        return (int)(byte*)index + LocateFirstFoundByte(vLtMatches);
                    }
                }
                else
                {
                    return (int)(byte*)index + LocateFirstFoundByte(vGtMatches);
                }
            }

            if ((int)(byte*)index < data.Length)
            {
                length -= (int)(byte*)index;
                goto SequentialScan;
            }
        }

        return -1;

        Found: // Workaround for https://github.com/dotnet/runtime/issues/8795
        return (int)(byte*)index;
        Found1:
        return (int)(byte*)(index + 1);
        Found2:
        return (int)(byte*)(index + 2);
        Found3:
        return (int)(byte*)(index + 3);
        Found4:
        return (int)(byte*)(index + 4);
        Found5:
        return (int)(byte*)(index + 5);
        Found6:
        return (int)(byte*)(index + 6);
        Found7:
        return (int)(byte*)(index + 7);
    }

    /// <summary>
    /// Returns the index of the first byte(s) that does not match the value specified in <paramref name="valueA"/>
    /// or <paramref name="valueB"/>.
    /// </summary>
    /// <param name="data">The data to search.</param>
    /// <param name="valueA">The first value to skip.</param>
    /// <param name="valueB">The second value to skip.</param>
    /// <returns>The index if found, -1 otherwise.</returns>
    /// <remarks>
    /// This method uses vectorized searching using intrinsics so should be preferred over iterating each
    /// byte.
    /// </remarks>
    public static unsafe int IndexOfNot(this ReadOnlySpan<byte> data, uint valueA, uint valueB)
    {
        if (data.Length == 0)
        {
            return -1;
        }

        ref byte searchSpace = ref MemoryMarshal.GetReference(data);

        nint index = (IntPtr)0;
        nint length = (IntPtr)data.Length;

        if (Vector.IsHardwareAccelerated && data.Length >= Vector<byte>.Count * 2)
        {
            int unaligned = (int)Unsafe.AsPointer(ref searchSpace) & (Vector<byte>.Count - 1);
            length = (IntPtr)((Vector<byte>.Count - unaligned) & (Vector<byte>.Count - 1));
        }

        SequentialScan:
        uint lookUp;
        while ((byte*)length >= (byte*)8)
        {
            length -= 8;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp != valueA && lookUp != valueB)
            {
                goto Found;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 1);
            if (lookUp != valueA && lookUp != valueB)
            {
                goto Found1;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 2);
            if (lookUp != valueA && lookUp != valueB)
            {
                goto Found2;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 3);
            if (lookUp != valueA && lookUp != valueB)
            {
                goto Found3;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 4);
            if (lookUp != valueA && lookUp != valueB)
            {
                goto Found4;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 5);
            if (lookUp != valueA && lookUp != valueB)
            {
                goto Found5;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 6);
            if (lookUp != valueA && lookUp != valueB)
            {
                goto Found6;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 7);
            if (lookUp != valueA && lookUp != valueB)
            {
                goto Found7;
            }

            index += 8;
        }

        if ((byte*)length >= (byte*)4)
        {
            length -= 4;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp != valueA && lookUp != valueB)
            {
                goto Found;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 1);
            if (lookUp != valueA && lookUp != valueB)
            {
                goto Found1;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 2);
            if (lookUp != valueA && lookUp != valueB)
            {
                goto Found2;
            }
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index + 3);
            if (lookUp != valueA && lookUp != valueB)
            {
                goto Found3;
            }

            index += 4;
        }

        while ((byte*)length > (byte*)0)
        {
            length -= 1;
            lookUp = Unsafe.AddByteOffset(ref searchSpace, index);
            if (lookUp != valueA && lookUp != valueB)
            {
                goto Found;
            }

            length += 1;
        }

        if (Vector.IsHardwareAccelerated && ((int)(byte*)index < data.Length))
        {
            length -= (int)(byte*)index;
            goto SequentialScan;
        }

        return -1;

        Found: // Workaround for https://github.com/dotnet/runtime/issues/8795
        return (int)(byte*)index;
        Found1:
        return (int)(byte*)(index + 1);
        Found2:
        return (int)(byte*)(index + 2);
        Found3:
        return (int)(byte*)(index + 3);
        Found4:
        return (int)(byte*)(index + 4);
        Found5:
        return (int)(byte*)(index + 5);
        Found6:
        return (int)(byte*)(index + 6);
        Found7:
        return (int)(byte*)(index + 7);
    }

    /// <summary>
    /// Returns the index of the first control byte found in the sequence.
    /// </summary>
    /// <param name="data">The data to search.</param>
    /// <returns>The index if found, -1 otherwise.</returns>
    /// <remarks>
    /// This method uses vectorized searching using intrinsics so should be preferred over iterating each
    /// byte.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOfControl(this ReadOnlySpan<byte> data)
        => IndexOfLessThan(data, 32);

    /// <summary>
    /// Returns the index of the first whitespace byte found in the sequence.
    /// </summary>
    /// <param name="data">The data to search.</param>
    /// <returns>The index if found, -1 otherwise.</returns>
    /// <remarks>
    /// This method uses vectorized searching using intrinsics so should be preferred over iterating each
    /// byte.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOfWhiteSpace(this ReadOnlySpan<byte> data)
        => IndexOf(data, _whiteSpaceByte);

    /// <summary>
    /// Returns the index of the first non-whitespace byte found in the sequence.
    /// </summary>
    /// <param name="data">The data to search.</param>
    /// <param name="tabIsWhiteSpace">Whether the tab control byte should be counted as whitespace.</param>
    /// <returns>The index if found, -1 otherwise.</returns>
    /// <remarks>
    /// This method uses vectorized searching using intrinsics so should be preferred over iterating each
    /// byte.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOfNotWhiteSpace(this ReadOnlySpan<byte> data, bool tabIsWhiteSpace = true)
    {
        return tabIsWhiteSpace
            ? IndexOfNot(data, _whiteSpaceByte, '\t')
            : IndexOfNot(data, _whiteSpaceByte);
    }

    // Vector sub-search adapted from https://github.com/aspnet/KestrelHttpServer/pull/1138
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int LocateFirstFoundByte(Vector<byte> match)
    {
        var vector64 = Vector.AsVectorUInt64(match);
        ulong candidate = 0;
        int i = 0;

        // Pattern unrolled by jit https://github.com/dotnet/coreclr/pull/8001
        for (; i < Vector<ulong>.Count; i++)
        {
            candidate = vector64[i];
            if (candidate != 0)
            {
                break;
            }
        }

        // Single LEA instruction with jitted const (using function result)
        return i * 8 + LocateFirstFoundByte(candidate);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int LocateFirstFoundByte(ulong match)
    {
        // Flag least significant power of two bit
        ulong powerOfTwoFlag = match ^ (match - 1);

        // Shift all powers of two into the high byte and extract
        return (int)((powerOfTwoFlag * _xorPowerOfTwoToHighByte) >> 57);
    }

    /// <summary>
    /// Replaces all instances of <paramref name="oldValue"/> in <paramref name="span"/>
    /// with <paramref name="newValue"/>.
    /// </summary>
    /// <param name="span">The span to search.</param>
    /// <param name="oldValue">The value to replace.</param>
    /// <param name="newValue">The new value.</param>
    public static void Replace(this Span<byte> span, byte oldValue, byte newValue)
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
    public static void Replace<T>(in Span<T?> span, T? oldValue, T? newValue)
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

    /// <summary>
    /// Returns the similarity percentage of two <see cref="ReadOnlySpan{T}"/>.
    /// </summary>
    /// <param name="span">The first text instance.</param>
    /// <param name="other">The second text instance.</param>
    /// <param name="comparison">How to compare two chars.</param>
    /// <returns>A number between 0 and 1 that represents a percentage similarity between the two strings.</returns>
    /// <remarks>
    /// If <paramref name="other"/> is <see langword="null"/>, 0 will be returned. If both arguments are empty,
    /// 1 will be returned.
    /// </remarks>
    public static double SimilarityTo(this ReadOnlySpan<char> span, string? other, StringComparison comparison = StringComparison.Ordinal)
    {
        return other is not null
            ? SimilarityTo(span, other.AsSpan(), comparison)
            : 0;
    }

    /// <summary>
    /// Returns the similarity percentage of two <see cref="ReadOnlySpan{T}"/>.
    /// </summary>
    /// <param name="span">The first text instance.</param>
    /// <param name="other">The second text instance.</param>
    /// <param name="comparison">How to compare two chars.</param>
    /// <returns>A number between 0 and 1 that represents a percentage similarity between the two strings.</returns>
    /// <remarks>
    /// If both arguments are empty, 1 will be returned.
    /// </remarks>
    public static double SimilarityTo(this ReadOnlySpan<char> span, ReadOnlySpan<char> other, StringComparison comparison = StringComparison.Ordinal)
    {
        if (span.Length == 0 && other.Length == 0)
        {
            return 1;
        }

        // Runes are still a bit clunky to work with. Since there is no clean way to get the count
        // head of time we need to rent out a buffer using StackList instead of stackalloc-ing
        // directly.

        var textRunes = new List<Rune>(1);
        var otherRunes = new List<Rune>(1);

        foreach (var rune in span.EnumerateRunes())
        {
            textRunes.Add(rune);
        }

        foreach (var rune in other.EnumerateRunes())
        {
            otherRunes.Add(rune);
        }

        return RuneHelper.SimilarityTo(textRunes, otherRunes, comparison);
    }
}
