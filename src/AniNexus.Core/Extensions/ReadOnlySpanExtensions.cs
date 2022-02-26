using System.Runtime.CompilerServices;
using System.Text;
using AniNexus.Collections;
using AniNexus.Helpers;

namespace AniNexus;

/// <summary>
/// <see cref="ReadOnlySpan{T}"/> extensions.
/// </summary>
public static class ReadOnlySpanExtensions
{
    /// <summary>
    /// Splits the contents of <paramref name="span"/> on whitespace characters.
    /// </summary>
    /// <param name="span">The chars to split.</param>
    /// <param name="stringSplitOptions">The split options.</param>
    public static SpanSplitEnumerator<char> Split(this ReadOnlySpan<char> span, StringSplitOptions stringSplitOptions = StringSplitOptions.None)
        => Split(span, ' ', stringSplitOptions);

    /// <summary>
    /// Splits the contents of <paramref name="span"/> on the specified separator.
    /// </summary>
    /// <param name="span">The chars to split.</param>
    /// <param name="separator">The character to split on.</param>
    /// <param name="stringSplitOptions">The split options.</param>
    public static SpanSplitEnumerator<char> Split(this ReadOnlySpan<char> span, char separator, StringSplitOptions stringSplitOptions = StringSplitOptions.None)
        => new(span, separator, stringSplitOptions);

    /// <summary>
    /// Splits the contents of <paramref name="span"/> on the specified separator.
    /// </summary>
    /// <param name="span">The chars to split.</param>
    /// <param name="separator">The consecutive characters (string) to split on.</param>
    /// <param name="stringSplitOptions">The split options.</param>
    public static SpanSplitSequenceEnumerator<char> Split(this ReadOnlySpan<char> span, ReadOnlySpan<char> separator, StringSplitOptions stringSplitOptions = StringSplitOptions.None)
        => new(span, separator, stringSplitOptions);

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

        var textRunes = new StackList<Rune>(1);
        var otherRunes = new StackList<Rune>(1);

        try
        {
            foreach (var rune in span.EnumerateRunes())
            {
                textRunes.Append(rune);
            }

            foreach (var rune in other.EnumerateRunes())
            {
                otherRunes.Append(rune);
            }

            return RuneHelper.SimilarityTo(ref textRunes, ref otherRunes, comparison);
        }
        finally
        {
            // Intellisense is still giving a false warning about disposal
            // when using is used inline with the declaration, so for now
            // we will wrap in try/finally.
            textRunes.Dispose();
            otherRunes.Dispose();
        }
    }

    /// <summary>
    /// An enumerator for splitting spans.
    /// </summary>
    /// <remarks>https://gist.github.com/bbartels/87c7daae28d4905c60ae77724a401b20</remarks>
    public ref struct SpanSplitEnumerator<T>
        where T : IEquatable<T>
    {
        /// <summary>
        /// The current element in the enumerator.
        /// </summary>
        public ReadOnlySpan<T> Current => _sequence.Slice(_startIndex, _offset);

        private readonly ReadOnlySpan<T> _sequence;
        private readonly StringSplitOptions _splitOptions;
        private readonly StackList<int> _separatorIndices;

        private int _index;
        private int _separatorIndex;
        private int _startIndex;
        private int _offset;

        internal SpanSplitEnumerator(ReadOnlySpan<T> span, [MaybeNull] T separator, StringSplitOptions stringSplitOptions)
        {
            _sequence = span;
            _splitOptions = stringSplitOptions;

            var separatorIndices = new StackList<int>(2);
            for (int i = 0; i < span.Length; ++i)
            {
                if (span[i].Equals(separator))
                {
                    separatorIndices.Append(i);
                }
            }
            _separatorIndices = separatorIndices;

            _index = 0;
            _separatorIndex = 0;
            _startIndex = 0;
            _offset = 0;
        }

        /// <summary>
        /// Returns the enumerator.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SpanSplitEnumerator<T> GetEnumerator()
            => this;

        /// <summary>
        /// Moves the enumerator to the next element in the sequence.
        /// </summary>
        public bool MoveNext()
        {
            if (_index >= _sequence.Length)
            {
                return false;
            }

            if (_splitOptions == StringSplitOptions.RemoveEmptyEntries)
            {
                while (_separatorIndex < _separatorIndices.Length && _index == _separatorIndices[_separatorIndex])
                {
                    _index = _separatorIndices[_separatorIndex] + 1;
                    ++_separatorIndex;
                }
            }

            _startIndex = _index;
            _offset = _separatorIndex < _separatorIndices.Length ? _separatorIndices[_separatorIndex] - _index : _sequence.Length - _startIndex;

            _index = _separatorIndex < _separatorIndices.Length ? _separatorIndices[_separatorIndex] + 1 : _sequence.Length;
            ++_separatorIndex;

            return true;
        }
    }

    /// <summary>
    /// An enumerator for splitting spans.
    /// </summary>
    /// <remarks>https://gist.github.com/bbartels/87c7daae28d4905c60ae77724a401b20</remarks>
    public ref struct SpanSplitSequenceEnumerator<T>
        where T : IEquatable<T>
    {
        /// <summary>
        /// The current element in the enumerator.
        /// </summary>
        public ReadOnlySpan<T> Current => _sequence.Slice(_startIndex, _offset);

        private readonly ReadOnlySpan<T> _sequence;
        private readonly StringSplitOptions _splitOptions;
        private readonly StackList<int> _separatorIndices;
        private readonly int _separatorLength;

        private int _index;
        private int _separatorIndex;
        private int _startIndex;
        private int _offset;

        internal SpanSplitSequenceEnumerator(ReadOnlySpan<T> span, ReadOnlySpan<T> separator, StringSplitOptions stringSplitOptions)
        {
            _sequence = span;
            _splitOptions = stringSplitOptions;

            var separatorIndices = new StackList<int>(2);
            _separatorLength = separator.Length;

            //https://github.com/dotnet/runtime/blob/master/src/libraries/System.Private.CoreLib/src/System/String.Manipulation.cs#L1579
            for (int i = 0; i < span.Length; ++i)
            {
                if (span[i].Equals(separator[0]) && _separatorLength <= span.Length - i)
                {
                    if (_separatorLength == 1 || span.Slice(i, _separatorLength).SequenceEqual(separator))
                    {
                        separatorIndices.Append(i);
                        i += _separatorLength - 1;
                    }
                }
            }
            _separatorIndices = separatorIndices;

            _index = 0;
            _separatorIndex = 0;
            _startIndex = 0;
            _offset = 0;
        }

        /// <summary>
        /// Returns the enumerator.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SpanSplitSequenceEnumerator<T> GetEnumerator()
            => this;

        /// <summary>
        /// Moves the enumerator to the next element in the sequence.
        /// </summary>
        public bool MoveNext()
        {
            if (_index >= _sequence.Length) { return false; }

            if (_splitOptions == StringSplitOptions.RemoveEmptyEntries)
            {
                while (_separatorIndex < _separatorIndices.Length && _index == _separatorIndices[_separatorIndex])
                {
                    _index = _separatorIndices[_separatorIndex] + _separatorLength;
                    ++_separatorIndex;
                }
            }

            _startIndex = _index;
            _offset = _separatorIndex < _separatorIndices.Length ? _separatorIndices[_separatorIndex] - _index : _sequence.Length - _startIndex;

            _index = _separatorIndex < _separatorIndices.Length ? _separatorIndices[_separatorIndex] + _separatorLength : _sequence.Length;
            ++_separatorIndex;

            return true;
        }
    }
}

