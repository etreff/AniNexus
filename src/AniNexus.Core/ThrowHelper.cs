namespace AniNexus;

/// <summary>
/// A helper class for throwing extensions.
/// </summary>
public static class ThrowHelper
{
    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/>.
    /// </summary>
    /// <param name="argumentName">The name of the argument that was <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException">Always.</exception>
    [DoesNotReturn]
    public static void ThrowArgumentNullException(string argumentName)
    {
        throw new ArgumentNullException(argumentName);
    }

    /// <summary>
    /// Throws an <see cref="ArgumentOutOfRangeException"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object that was out of range.</typeparam>
    /// <param name="argumentName">The name of the argument that was out of range.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    [DoesNotReturn]
    public static void ThrowArgumentOutOfRangeException<T>(string argumentName, T min, T max)
    {
        throw new ArgumentOutOfRangeException(argumentName, $"Value must be between {min} and {max}.");
    }

    /// <summary>
    /// Throws an <see cref="InvalidOperationException"/>.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <exception cref="InvalidOperationException"></exception>
    [DoesNotReturn]
    public static void ThrowInvalidOperationException(string message)
    {
        throw new InvalidOperationException(message);
    }

    /// <summary>
    /// Throws an <see cref="InvalidOperationException"/> for an empty collection.
    /// </summary>
    /// <exception cref="InvalidOperationException">Always.</exception>
    [DoesNotReturn]
    public static void ThrowEmptyCollectionError()
    {
        throw new InvalidOperationException("The collection is empty.");
    }

    /// <summary>
    /// Throws an <see cref="ArgumentOutOfRangeException"/> for an invalid enum member.
    /// </summary>
    /// <param name="value">The enum value that is invalid.</param>
    /// <exception cref="ArgumentOutOfRangeException">Always.</exception>
    [DoesNotReturn]
    public static void ThrowInvalidEnumValue<TEnum>(TEnum value)
        where TEnum : struct, Enum => ThrowInvalidEnumValue(value, typeof(TEnum));

    /// <summary>
    /// Throws an <see cref="ArgumentOutOfRangeException"/> for an invalid enum member.
    /// </summary>
    /// <param name="value">The enum value that is invalid.</param>
    /// <param name="enumType">The type of the enum.</param>
    /// <exception cref="ArgumentOutOfRangeException">Always.</exception>
    [DoesNotReturn]
    internal static void ThrowInvalidEnumValue<TEnum>(TEnum value, Type enumType)
        where TEnum : struct, Enum
    {
        throw new ArgumentOutOfRangeException($"{value} is not a valid {enumType.Name} enum value.");
    }

    /// <summary>
    /// Throws a <see cref="FormatException"/> for a malformed enum value.
    /// </summary>
    /// <param name="value">The enum value that is invalid.</param>
    /// <exception cref="FormatException">Always.</exception>
    [DoesNotReturn]
    public static void ThrowInvalidEnumValue<TEnum>(string value)
        where TEnum : struct, Enum => ThrowInvalidEnumValue(value, typeof(TEnum));

    /// <summary>
    /// Throws a <see cref="FormatException"/> for a malformed enum value.
    /// </summary>
    /// <param name="value">The enum value that is invalid.</param>
    /// <param name="enumType">The type of the enum.</param>
    /// <exception cref="FormatException">Always.</exception>
    [DoesNotReturn]
    internal static void ThrowInvalidEnumValue(string value, Type enumType)
    {
        throw new FormatException($"The value {value} is not a member of type {enumType} or is outside the underlying type's range.");
    }
}
