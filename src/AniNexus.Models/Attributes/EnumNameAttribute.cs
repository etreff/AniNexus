using System;

namespace AniNexus.Models
{
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

        public EnumNameAttribute(string name)
        {
            Name = name;
        }
    }
}
