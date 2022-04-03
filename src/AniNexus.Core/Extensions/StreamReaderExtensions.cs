using System.Runtime.CompilerServices;

namespace System.IO;

/// <summary>
/// <see cref="StreamReader"/> extensions.
/// </summary>
public static class StreamReaderExtensions
{
    /// <summary>
    /// Reads all lines from a <see cref="StreamReader"/>.
    /// </summary>
    /// <param name="streamReader">The reader to read from.</param>
    public static IEnumerable<string> ReadAllLines(this StreamReader streamReader)
    {
        Guard.IsNotNull(streamReader, nameof(streamReader));

        return _(); IEnumerable<string> _()
        {
            string? line;
            while ((line = streamReader.ReadLine()) is not null)
            {
                yield return line;
            }
        }
    }

    /// <summary>
    /// Reads all lines from a <see cref="StreamReader"/>.
    /// </summary>
    /// <param name="streamReader">The reader to read from.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public static IAsyncEnumerable<string> ReadAllLinesAsync(this StreamReader streamReader, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(streamReader, nameof(streamReader));

        return _(cancellationToken); async IAsyncEnumerable<string> _([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string? line;
            while ((line = await streamReader.ReadLineAsync().ConfigureAwait(false)) is not null)
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return line;
            }
        }
    }
}
