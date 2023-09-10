using System;
using System.Collections.Generic;

namespace AniNexus.Web.Utilities;

/// <summary>
/// A utility class for constructing dynamic styles.
/// </summary>
public sealed class StyleBuilder
{
    private readonly List<string> _styles = new();
    private readonly Action<IStyleBuilder> _builderAction;

    /// <summary>
    /// Creates a new <see cref="StyleBuilder"/> instance.
    /// </summary>
    /// <param name="builderAction">The action to use to rebuild the styles.</param>
    public StyleBuilder(Action<IStyleBuilder> builderAction)
    {
        _builderAction = builderAction;
    }

    /// <summary>
    /// Marks the builder as dirty.
    /// </summary>
    public void Rebuild()
    {
        _styles.Clear();

        var builder = new Builder(_styles);
        _builderAction(builder);
    }

    /// <summary>
    /// Returns the classes stored in this builder as a single CSS string.
    /// </summary>
    public override string ToString()
    {
        return string.Join(';', _styles);
    }

    /// <summary>
    /// A builder for adding styles.
    /// </summary>
    public interface IStyleBuilder
    {
        /// <summary>
        /// Adds a style.
        /// </summary>
        /// <param name="name">The style property name.</param>
        /// <param name="value">The value to set the property to.</param>
        IStyleBuilder AddStyle(string name, string value);

        /// <summary>
        /// Adds a style if <paramref name="condition"/> is <see langword="true"/>.
        /// </summary>
        /// <param name="name">The style property name.</param>
        /// <param name="value">The value to set the property to.</param>
        /// <param name="condition">The condition that must be <see langword="true"/> for the style to be added.</param>
        IStyleBuilder AddStyle(string name, string value, bool condition);
    }

    private sealed class Builder : IStyleBuilder
    {
        private readonly List<string> _styles;

        public Builder(List<string> styles)
        {
            _styles = styles;
        }

        public IStyleBuilder AddStyle(string name, string value)
        {
            _styles.Add($"{name}:{value}");

            return this;
        }

        public IStyleBuilder AddStyle(string name, string value, bool condition)
        {
            if (condition)
            {
                return AddStyle(name, value);
            }

            return this;
        }
    }
}
