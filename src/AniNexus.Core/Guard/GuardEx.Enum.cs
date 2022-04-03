using System.Runtime.CompilerServices;

namespace Microsoft.Toolkit.Diagnostics;

public static partial class GuardEx
{
    /// <summary>
    /// Asserts that the input value is valid for an enum of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="value">The input value to test.</param>
    /// <param name="name">The name of the input parameter being tested.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IsValid<T>(T value, string name)
        where T : struct, Enum
    {
        if (value.IsValid())
        {
            return;
        }

        ThrowHelperEx.ThrowArgumentExceptionForIsValidForEnum<T>(value, name);
    }

    /// <summary>
    /// Asserts that the input value is valid for an enum of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="value">The input value to test.</param>
    /// <param name="name">The name of the input parameter being tested.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IsValid<T>(T? value, string name)
        where T : struct, Enum
    {
        if (!value.HasValue || value.Value.IsValid())
        {
            return;
        }

        ThrowHelperEx.ThrowArgumentExceptionForIsValidForEnum(value, name);
    }
}
