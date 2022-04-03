using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AniNexus.IO;

/// <summary>
/// A buffered <see cref="BinaryReader"/> for better performance.
/// </summary>
/// <remarks>
/// This class does not support char/string operations. To use
/// these features either use <see cref="BinaryReader"/> or
/// handle the value returned from <see cref="ReadBytes(int)"/>
/// directly.
/// </remarks>
public ref struct BufferedBinaryReader
{
    /// <summary>
    /// The default buffer size.
    /// </summary>
    public const int DefaultBufferSize = 4096;

    /// <summary>
    /// The buffer size.
    /// </summary>
    public int BufferSize { get; }

    /// <summary>
    /// The number of unread buffered bytes.
    /// </summary>
    public int BytesAvailable => Math.Max(0, _numBufferedBytes - _currentBufferOffset);

    private readonly Stream _stream;
    private readonly Span<byte> _buffer;

    private int _currentBufferOffset;
    private int _numBufferedBytes;

    /// <summary>
    /// Creates a new <see cref="BufferedBinaryReader"/> instance.
    /// </summary>
    /// <param name="stream">The stream.</param>
    public BufferedBinaryReader(Stream stream)
        : this(stream, DefaultBufferSize)
    {
    }

    /// <summary>
    /// Creates a new <see cref="BufferedBinaryReader"/> instance.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="bufferSize">The buffer size.</param>
    public BufferedBinaryReader(Stream stream, int bufferSize)
        : this(stream, new byte[bufferSize])
    {
    }

    /// <summary>
    /// Creates a new <see cref="BufferedBinaryReader"/> instance.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="buffer">The buffer.</param>
    public BufferedBinaryReader(Stream stream, Span<byte> buffer)
    {
        Guard.IsNotNull(stream, nameof(stream));
        Guard.CanSeek(stream, nameof(stream));
        Guard.IsGreaterThan(buffer.Length, 0, nameof(buffer));

        _stream = stream;
        _buffer = buffer;
        BufferSize = _buffer.Length;
        _currentBufferOffset = BufferSize;
        _numBufferedBytes = 0;
    }

    /// <summary>
    /// Fills the internal buffer.
    /// </summary>
    /// <returns><see langword="true"/> if new bytes are available, <see langword="false"/> otherwise.</returns>
    private bool FillBuffer()
    {
        int numUnreadBytes = BufferSize - _currentBufferOffset;
        int numBytesToRead = BufferSize - numUnreadBytes;

        _currentBufferOffset = 0;
        _numBufferedBytes = numUnreadBytes;

        // If we still have some leftover unread bytes, shift those over to 0 index.
        if (numUnreadBytes > 0)
        {
            CopyBuffer(numBytesToRead, 0, numUnreadBytes);
        }

        bool firstRead = true;
        while (numBytesToRead > 0)
        {
            int numBytesRead = _stream.Read(_buffer.Slice(numUnreadBytes, numBytesToRead));
            if (numBytesRead == 0)
            {
                return !firstRead;
            }

            firstRead = false;
            _numBufferedBytes += numBytesRead;
            numBytesToRead -= numBytesRead;
            numUnreadBytes += numBytesRead;
        }
        return true;
    }

    /// <summary>
    /// Seems to the specified offset.
    /// </summary>
    /// <param name="offset">The offset.</param>
    /// <param name="origin">The origin of the offset.</param>
    public void Seek(long offset, SeekOrigin origin)
    {
        _stream.Seek(offset, origin);
        _currentBufferOffset = BufferSize;
        _numBufferedBytes = 0;
    }

    /// <summary>
    /// Returns the next byte in the stream, or -1 if no bytes
    /// are available.
    /// </summary>
    public int Peek()
    {
        if (BytesAvailable < 1)
        {
            bool b = FillBuffer();
            if (!b || BytesAvailable < 1)
            {
                return -1;
            }
        }

        return _buffer[_currentBufferOffset];
    }

    /// <summary>
    /// Reads a boolean from the buffer.
    /// </summary>
    public bool ReadBoolean()
    {
        int size = CheckAndFillBuffer<bool>();

        byte result = _buffer[_currentBufferOffset];
        _currentBufferOffset += size;

        // Same logic as native BinaryReader.
        return result != 0;
    }

    /// <summary>
    /// Reads a <see cref="byte"/> from the buffer.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte ReadByte()
    {
        return ReadMemory<byte>();
    }

    /// <summary>
    /// Reads a <see cref="sbyte"/> from the buffer.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public sbyte ReadSByte()
    {
        return ReadMemory<sbyte>();
    }

    /// <summary>
    /// Reads a <see cref="short"/> from the buffer.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public short ReadInt16()
    {
        return ReadMemory<short>();
    }

    /// <summary>
    /// Reads an <see cref="ushort"/> from the buffer.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ushort ReadUInt16()
    {
        return ReadMemory<ushort>();
    }

    /// <summary>
    /// Reads an <see cref="int"/> from the buffer.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ReadInt32()
    {
        return ReadMemory<int>();
    }

    /// <summary>
    /// Reads an <see cref="uint"/> from the buffer.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint ReadUInt32()
    {
        return ReadMemory<uint>();
    }

    /// <summary>
    /// Reads a <see cref="long"/> from the buffer.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ReadInt64()
    {
        return ReadMemory<long>();
    }

    /// <summary>
    /// Reads an <see cref="ulong"/> from the buffer.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong ReadUInt64()
    {
        return ReadMemory<ulong>();
    }

    /// <summary>
    /// Reads a <see cref="float"/> from the buffer.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float ReadSingle()
    {
        return ReadMemory<float>();
    }

    /// <summary>
    /// Reads a <see cref="double"/> from the buffer.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double ReadDouble()
    {
        return ReadMemory<double>();
    }

    /// <summary>
    /// Reads a <see cref="decimal"/> from the buffer.
    /// </summary>
    /// <remarks>
    /// This method reads in a well formed C# decimal. This method is not meant
    /// to read a <see cref="float"/> or <see cref="double"/> and cast it to a
    /// <see cref="decimal"/>.
    /// </remarks>
    public decimal ReadDecimal()
    {
        CheckAndFillBuffer<decimal>();

        // Decimal is a managed and doesn't necessarily have the correct binary layout,
        // so we will use the constructor that will guarantee we get the correct value.
        return new decimal(new[] { ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32() });
    }

    /// <summary>
    /// Reads bytes from the underlying buffer to fill <paramref name="buffer"/>.
    /// </summary>
    /// <param name="buffer">The buffer to fill.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ReadBytes(byte[] buffer)
    {
        Guard.IsNotNull(buffer, nameof(buffer));

        ReadBytes(buffer, 0, buffer.Length);
    }

    /// <summary>
    /// Reads bytes from the underlying buffer to fill <paramref name="buffer"/>.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="buffer">The buffer to fill.</param>
    /// <remarks>
    /// This needs to be static. https://github.com/dotnet/roslyn/issues/23433
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReadBytes(BufferedBinaryReader reader, Span<byte> buffer)
    {
        int count = reader.CheckAndFillBuffer(buffer.Length);

        reader.CopyBuffer(reader._currentBufferOffset, buffer);

        reader._currentBufferOffset += count;
    }

    /// <summary>
    /// Reads bytes from the underlying buffer to fill <paramref name="buffer"/>.
    /// </summary>
    /// <param name="buffer">The buffer to fill.</param>
    /// <param name="offset">The offset in <paramref name="buffer"/> to start filling.</param>
    /// <param name="count">The number of bytes to read.</param>
    public void ReadBytes(byte[] buffer, int offset, int count)
    {
        Guard.IsNotNull(buffer, nameof(buffer));

        CheckAndFillBuffer<byte[]>(count);

        CopyBuffer(_currentBufferOffset, buffer, offset, count);

        _currentBufferOffset += count;
    }

    /// <summary>
    /// Reads <paramref name="count"/> bytes from the underlying buffer.
    /// </summary>
    /// <param name="count">The number of bytes to read.</param>
    public byte[] ReadBytes(int count)
    {
        CheckAndFillBuffer<byte[]>(count);

        byte[] result = new byte[count];

        CopyBuffer(_currentBufferOffset, result);

        _currentBufferOffset += count;
        return result;
    }

    /// <summary>
    /// Skips <paramref name="count"/> bytes in the underlying buffer.
    /// </summary>
    /// <param name="count">The number of bytes to read.</param>
    public void SkipBytes(int count)
    {
        Guard.IsGreaterThanOrEqualTo(count, 0, nameof(count));

        if (count < BytesAvailable)
        {
            _currentBufferOffset += count;
        }
        else
        {
            Seek(count, SeekOrigin.Current);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private T ReadMemory<T>()
        where T : struct
    {
        int size = CheckAndFillBuffer<T>();

        var result = MemoryMarshal.Read<T>(_buffer.Slice(_currentBufferOffset, size));
        _currentBufferOffset += size;

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int CheckAndFillBuffer<T>()
        => CheckAndFillBuffer<T>(Unsafe.SizeOf<T>());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int CheckAndFillBuffer<T>(int size)
    {
        if (BytesAvailable < size && (!FillBuffer() || BytesAvailable < size))
        {
            ThrowNotEnoughBytesAvailableException<T>();
        }

        return size;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int CheckAndFillBuffer(int size)
    {
        if (BytesAvailable < size && (!FillBuffer() || BytesAvailable < size))
        {
            ThrowNotEnoughBytesAvailableException();
        }

        return size;
    }

    [DoesNotReturn]
    private static void ThrowNotEnoughBytesAvailableException<T>()
    {
        throw new IOException($"Not enough bytes available to read type {typeof(T).Name}.");
    }

    [DoesNotReturn]
    private static void ThrowNotEnoughBytesAvailableException()
    {
        throw new IOException("Not enough bytes available.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void CopyBuffer(int srcOffset, int dstOffset, int count)
    {
        var source = _buffer[srcOffset..];
        var dest = _buffer.Slice(dstOffset, count);

        source.CopyTo(dest);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void CopyBuffer(int srcOffset, Span<byte> dest)
    {
        var source = _buffer.Slice(srcOffset, dest.Length);
        source.CopyTo(dest);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void CopyBuffer(int srcOffset, Span<byte> dest, int dstOffset, int count)
    {
        var source = _buffer[srcOffset..];
        source.CopyTo(dest.Slice(dstOffset, count));
    }
}
