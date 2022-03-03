using System.Buffers;
using System.Runtime.CompilerServices;

namespace AniNexus.Collections;

/// <summary>
/// A <see cref="List{T}"/> that is allocated on the stack.
/// </summary>
/// <typeparam name="T">The underlying type.</typeparam>
public ref struct StackList<T>
{
    /// <summary>
    /// The number of elements in this list.
    /// </summary>
    public int Length { get; private set; }

    private Span<T> _span;
    private T[]? _arrayFromPool;

    /// <summary>
    /// Returns the element at the specified index.
    /// </summary>
    /// <param name="index">The index of the element to retrieve.</param>
    public ref T this[int index] => ref _span[index];

    /// <summary>
    /// Creates a new <see cref="StackList{T}"/> instance.
    /// </summary>
    /// <param name="initialSize">The initial buffer size.</param>
    public StackList(int initialSize)
    {
        _span = _arrayFromPool = ArrayPool<T>.Shared.Rent(initialSize);
        Length = 0;
    }

    /// <summary>
    /// Creates a new <see cref="StackList{T}"/> instance.
    /// </summary>
    /// <param name="initialSpan">The initial buffer to use.</param>
    public StackList(Span<T> initialSpan)
    {
        _span = initialSpan;
        _arrayFromPool = null;
        Length = 0;
    }

    /// <summary>
    /// Releases the underlying memory used for this <see cref="StackList{T}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        if (_arrayFromPool is not null)
        {
            ArrayPool<T>.Shared.Return(_arrayFromPool);
            _arrayFromPool = null;
        }
    }

    /// <summary>
    /// Appends an item to this list.
    /// </summary>
    /// <param name="item">The item to append to the list.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(T item)
    {
        int pos = Length;
        if (pos >= _span.Length)
        {
            Grow();
        }

        _span[pos] = item;
        Length = pos + 1;
    }

    /// <summary>
    /// Returns a <see cref="ReadOnlySpan{T}"/> over the elements
    /// in this list.
    /// </summary>
    public ReadOnlySpan<T> AsSpan()
    {
        return _span.Length == Length ? _span : _span.Slice(0, Length);
    }

    private void Grow()
    {
        var array = ArrayPool<T>.Shared.Rent(_span.Length * 2);
        if (!_span.TryCopyTo(array))
        {
            throw new InvalidOperationException("Unable to copy span to grown ArrayPool array.");
        }

        var toReturn = _arrayFromPool;
        _span = _arrayFromPool = array;
        if (toReturn is not null)
        {
            ArrayPool<T>.Shared.Return(toReturn);
        }
    }
}
