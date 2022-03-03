using System;

namespace AniNexus;

/// <summary>
/// When applied to an <see cref="Enum"/> member, the name specified will
/// be stored in the database instead of the member's name.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public sealed class EnumNameAttribute : Attribute
{
    /// <summary>
    /// The name to use instead of the member name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Creates a new <see cref="EnumNameAttribute"/> instance.
    /// </summary>
    /// <param name="name"></param>
    public EnumNameAttribute(string name)
    {
        Name = name;
    }
}
