﻿using Microsoft.Toolkit.Diagnostics;
using System.IO;

namespace AniNexus;

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
    public static IAsyncEnumerable<string> ReadAllLinesAsync(this StreamReader streamReader)
    {
        Guard.IsNotNull(streamReader, nameof(streamReader));

        return _(); async IAsyncEnumerable<string> _()
        {
            string? line;
            while ((line = await streamReader.ReadLineAsync().ConfigureAwait(false)) is not null)
            {
                yield return line;
            }
        }
    }
}

