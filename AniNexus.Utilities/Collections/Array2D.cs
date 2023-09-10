using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Helpers;

namespace AniNexus.Collections;

/// <summary>
/// A wrapper around a 1D array to make it behave like a 2D array.
/// </summary>
public readonly struct Array2D<T> : IEquatable<Array2D<T>>
{
    /// <summary>
    /// An empty <see cref="Array2D{T}"/> instance.
    /// </summary>
    public static Array2D<T> Empty => default;

    /// <summary>
    /// The width of the 2D array.
    /// </summary>
    public readonly int Width { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }

    /// <summary>
    /// The height of the 2D array.
    /// </summary>
    public readonly int Height { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }

    /// <summary>
    /// Whether this <see cref="Array2D{T}"/> instance has a size of 0.
    /// </summary>
    public readonly bool IsEmpty { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => Width == 0 || Height == 0; }

    /// <summary>
    /// The number of elements in this <see cref="Array2D{T}"/> instance.
    /// </summary>
    public readonly int Length { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => Width * Height; }

    /// <summary>
    /// A span for the current <see cref="Array2D{T}"/>.
    /// </summary>
    public readonly Span<T> Span
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (_array is not null)
            {
                ref var r0 = ref ObjectMarshal.DangerousGetObjectDataReferenceAt<T>(_array, _offset);
                return MemoryMarshal.CreateSpan(ref r0, Length);
            }

            return default;
        }
    }

    private readonly object? _array;
    private readonly IntPtr _offset;

    /// <summary>
    /// Constructs a new <see cref="Array2D{T}"/> instance.
    /// </summary>
    /// <param name="array">The 1D array to wrap.</param>
    /// <param name="width">The width of the 2D array.</param>
    /// <param name="height">The height of the 2D array.</param>
    /// <exception cref="InvalidOperationException">The product of <paramref name="width"/> and <paramref name="height"/> is less than the length of <paramref name="array"/>.</exception>
    public Array2D(T[] array, int width, int height)
    {
        if (default(T) is null && array.GetType() != typeof(T[]))
        {
            throw new ArrayTypeMismatchException();
        }

        if (width * height > array.Length)
        {
            throw new InvalidOperationException($"The product of {nameof(width)} and {nameof(height)} must be less than or equal to the length of the memory region.");
        }

        Width = width;
        Height = height;
        _array = array;
        _offset = ObjectMarshal.DangerousGetObjectDataByteOffset(array, ref array.DangerousGetReferenceAt(0));
    }

    /// <summary>
    /// Constructs a new <see cref="Array2D{T}"/> instance.
    /// </summary>
    /// <param name="array">The 2D array to wrap.</param>
    public Array2D(T[,]? array)
    {
        if (array is null)
        {
            this = default;
            return;
        }

        if (default(T) is null && array.GetType() != typeof(T[]))
        {
            throw new ArrayTypeMismatchException();
        }

        Width = array.GetLength(0);
        Height = array.GetLength(1);
        _array = array;

        var arr = new T[1, 1];
        _offset = ObjectMarshal.DangerousGetObjectDataByteOffset(arr, ref arr[0, 0]);
    }

    /// <summary>
    /// Constructs a new <see cref="Array2D{T}"/> instance.
    /// </summary>
    /// <param name="memory">The 1D memory to wrap.</param>
    /// <param name="width">The width of the 2D array.</param>
    /// <param name="height">The height of the 2D array.</param>
    /// <exception cref="InvalidOperationException">The product of <paramref name="width"/> and <paramref name="height"/> is less than the length of <paramref name="memory"/>.</exception>
    public Array2D(Memory<T> memory, int width, int height)
    {
        if (width == 0 || height == 0)
        {
            this = default;
            return;
        }

        if (width * height > memory.Length)
        {
            throw new InvalidOperationException($"The product of {nameof(width)} and {nameof(height)} must be less than or equal to the length of the memory region.");
        }

        // https://github.com/CommunityToolkit/dotnet/blob/main/CommunityToolkit.HighPerformance/Memory/Memory2D%7BT%7D.cs#L454

        // Check if the Memory<T> instance wraps a string. This is possible in case
        // consumers do an unsafe cast for the entire Memory<T> object, and while not
        // really safe it is still supported in CoreCLR too, so we're following suit here.
        if (typeof(T) == typeof(char) &&
            MemoryMarshal.TryGetString(Unsafe.As<Memory<T>, Memory<char>>(ref memory), out string? text, out int textStart, out _))
        {
            ref char r0 = ref text.DangerousGetReferenceAt(textStart);

            _array = text;
            _offset = ObjectMarshal.DangerousGetObjectDataByteOffset(text, ref r0);
        }
        else if (MemoryMarshal.TryGetArray(memory, out ArraySegment<T> segment))
        {
            // Check if the input Memory<T> instance wraps an array we can access.
            // This is fine, since Memory<T> on its own doesn't control the lifetime
            // of the underlying array anyway, and this Array2D<T> type would do the same.
            T[] array = segment.Array!;

            _array = array;
            _offset = ObjectMarshal.DangerousGetObjectDataByteOffset(array, ref array.DangerousGetReferenceAt(segment.Offset));
        }
        else
        {
            ThrowForUnsupportedType();
            _array = null;
            _offset = IntPtr.Zero;
        }

        Width = width;
        Height = height;
    }

    /// <summary>
    /// Gets or sets the element at the specified coordinates.
    /// </summary>
    /// <param name="x">The 0-based X coordinate.</param>
    /// <param name="y">The 0-based Y coordinate.</param>
    public T this[int x, int y]
    {
        get => Span[x * Width + y];
        set => Span[x * Width + y] = value;
    }

    /// <summary>
    /// Copies the contents of this <see cref="Array2D{T}"/> into a destination <see cref="Span{T}"/>.
    /// </summary>
    /// <param name="other">The span to copy this <see cref="Array2D{T}"/> instance into.</param>
    /// <exception cref="ArgumentException"><paramref name="other"/> is shorter than the length of the <see cref="Array2D{T}"/> instance.</exception>
    public readonly void CopyTo(Span<T> other)
    {
        Span.CopyTo(other);
    }

    /// <summary>
    /// Copies the contents of this <see cref="Array2D{T}"/> into a destination <see cref="Span{T}"/>.
    /// </summary>
    /// <param name="other">The span to copy this <see cref="Array2D{T}"/> instance into.</param>
    /// <exception cref="ArgumentException"><paramref name="other"/> is shorter than the length of the <see cref="Array2D{T}"/> instance.</exception>
    public readonly void TryCopyTo(Span<T> other)
    {
        Span.TryCopyTo(other);
    }

    /// <summary>
    /// Copies the contents of this <see cref="Array2D{T}"/> into a destination <see cref="Memory{T}"/>.
    /// </summary>
    /// <param name="other">The memory to copy this <see cref="Array2D{T}"/> instance into.</param>
    /// <exception cref="ArgumentException"><paramref name="other"/> is shorter than the length of the <see cref="Array2D{T}"/> instance.</exception>
    public readonly void CopyTo(Memory<T> other)
    {
        Span.CopyTo(other.Span);
    }

    /// <summary>
    /// Copies the contents of this <see cref="Array2D{T}"/> into a destination <see cref="Memory{T}"/>.
    /// </summary>
    /// <param name="other">The memory to copy this <see cref="Array2D{T}"/> instance into.</param>
    /// <exception cref="ArgumentException"><paramref name="other"/> is shorter than the length of the <see cref="Array2D{T}"/> instance.</exception>
    public readonly void TryCopyTo(Memory<T> other)
    {
        Span.TryCopyTo(other.Span);
    }

    /// <summary>
    /// Copies the contents of the current <see cref="Array2D{T}"/> instance into a new native 2D array.
    /// </summary>
    public readonly T[,] ToArray()
    {
        var result = new T[Height, Width];

        ref T r0 = ref Unsafe.As<byte, T>(ref MemoryMarshal.GetArrayDataReference(result));
        var span = MemoryMarshal.CreateSpan(ref r0, Length);

        CopyTo(span);

        return result;
    }

    /// <summary>
    /// Indicates whether two <see cref="Array2D{T}"/> instances are equal.
    /// </summary>
    public static bool operator ==(Array2D<T> left, Array2D<T> right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Indicates whether two <see cref="Array2D{T}"/> instances are not equal.
    /// </summary>
    public static bool operator !=(Array2D<T> left, Array2D<T> right)
    {
        return !(left == right);
    }

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Array2D<T> a && Equals(a);
    }

    /// <inheritdoc/>
    public readonly bool Equals(Array2D<T> other)
    {
        return _array == other._array &&
               _offset == other._offset &&
               Height == other.Height &&
               Width == other.Width;
    }

    /// <inheritdoc/>
    public readonly override int GetHashCode()
    {
        return _array is not null
            ? HashCode.Combine(RuntimeHelpers.GetHashCode(_array), _offset, Height, Width)
            : 0;
    }

    /// <inheritdoc/>
    public override readonly string ToString()
    {
        return $"AniNexus.Collections.Array2D<{typeof(T)}>[{Height}, {Width}]";
    }

    /// <summary>
    /// Implicitly creates an <see cref="Array2D{T}"/> instance from a native 2D array.
    /// </summary>
    public static implicit operator Array2D<T>(T[,]? array)
    {
        return new(array);
    }

    private static void ThrowForUnsupportedType()
    {
        throw new ArgumentException("The specified object type is not supported.");
    }
}
