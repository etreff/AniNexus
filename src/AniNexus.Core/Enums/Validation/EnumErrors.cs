using System.Runtime.CompilerServices;

namespace AniNexus;

internal static class EnumErrors<TEnum>
    where TEnum : struct, Enum
{
    private static readonly Type _enumType = typeof(TEnum);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string InvalidEnumValue(TEnum value)
    {
        return $"{value} is not a valid value for the enum type {_enumType}.";
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string InvalidFlagEnumValue(TEnum value)
    {
        return $"{value} is not a valid flag mask for the enum type {_enumType}.";
    }
}

