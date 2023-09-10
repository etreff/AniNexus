using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System;

/// <summary>
/// <see cref="ReadOnlySpan{T}"/> extensions.
/// </summary>
public static partial class MemoryExtensions
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
        private readonly List<int> _separatorIndices;

        private int _index;
        private int _separatorIndex;
        private int _startIndex;
        private int _offset;

        internal SpanSplitEnumerator(ReadOnlySpan<T> span, [MaybeNull] T separator, StringSplitOptions stringSplitOptions)
        {
            _sequence = span;
            _splitOptions = stringSplitOptions;

            var separatorIndices = new List<int>(2);
            for (int i = 0; i < span.Length; ++i)
            {
                if (span[i].Equals(separator))
                {
                    separatorIndices.Add(i);
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
                while (_separatorIndex < _separatorIndices.Count && _index == _separatorIndices[_separatorIndex])
                {
                    _index = _separatorIndices[_separatorIndex] + 1;
                    ++_separatorIndex;
                }
            }

            _startIndex = _index;
            _offset = _separatorIndex < _separatorIndices.Count ? _separatorIndices[_separatorIndex] - _index : _sequence.Length - _startIndex;

            _index = _separatorIndex < _separatorIndices.Count ? _separatorIndices[_separatorIndex] + 1 : _sequence.Length;
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
        private readonly List<int> _separatorIndices;
        private readonly int _separatorLength;

        private int _index;
        private int _separatorIndex;
        private int _startIndex;
        private int _offset;

        internal SpanSplitSequenceEnumerator(ReadOnlySpan<T> span, ReadOnlySpan<T> separator, StringSplitOptions stringSplitOptions)
        {
            _sequence = span;
            _splitOptions = stringSplitOptions;

            var separatorIndices = new List<int>(2);
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
                while (_separatorIndex < _separatorIndices.Count && _index == _separatorIndices[_separatorIndex])
                {
                    _index = _separatorIndices[_separatorIndex] + _separatorLength;
                    ++_separatorIndex;
                }
            }

            _startIndex = _index;
            _offset = _separatorIndex < _separatorIndices.Count ? _separatorIndices[_separatorIndex] - _index : _sequence.Length - _startIndex;

            _index = _separatorIndex < _separatorIndices.Count ? _separatorIndices[_separatorIndex] + _separatorLength : _sequence.Length;
            ++_separatorIndex;

            return true;
        }
    }
}
