using System;
using System.Globalization;

namespace AniNexus;

/// <summary>
/// Marks extra metadata associated with an enum member.
/// </summary>
/// <remarks>
/// This does not create any database columns.
/// </remarks>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class EnumMetadataAttribute : Attribute
{
    /// <summary>
    /// The key.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// The value.
    /// </summary>
    public string Value { get; }

    public EnumMetadataAttribute(string key, string value)
    {
        Key = key;
        Value = value;
    }

    public EnumMetadataAttribute(string key, int value)
    {
        Key = key;
        Value = value.ToString(CultureInfo.InvariantCulture);
    }
}