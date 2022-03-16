using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Microsoft.Toolkit.Diagnostics;

/// <summary>
/// Helper methods not included in <see cref="Guard"/> to verify conditions when running code.
/// </summary>
[DebuggerStepThrough]
public static partial class GuardEx
{
    /// <summary>
    /// Asserts that the input value is not <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value type being tested.</typeparam>
    /// <param name="value">The input value to test.</param>
    /// <param name="name">The name of the input parameter being tested.</param>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IsNotNull<T>(T? value, string name)
    {
        if (value is null)
        {
            ThrowHelper.ThrowArgumentNullException(name);
        }
    }
}
