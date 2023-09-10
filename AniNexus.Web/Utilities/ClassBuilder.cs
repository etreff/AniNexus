using System;
using System.Collections.Generic;

namespace AniNexus.Web.Utilities;

/// <summary>
/// A utility class for constructing dynamic classes.
/// </summary>
public sealed class ClassBuilder
{
    private readonly HashSet<string> _classes = new();
    private readonly Action<IClassBuilder> _builderAction;

    /// <summary>
    /// Creates a new <see cref="ClassBuilder"/> instance.
    /// </summary>
    /// <param name="builderAction">The action to use to rebuild the classes.</param>
    public ClassBuilder(Action<IClassBuilder> builderAction)
    {
        _builderAction = builderAction;
    }

    /// <summary>
    /// Marks the builder as dirty.
    /// </summary>
    public void Rebuild()
    {
        _classes.Clear();

        var builder = new Builder(_classes);
        _builderAction(builder);
    }

    /// <summary>
    /// Returns the classes stored in this builder as a single string.
    /// </summary>
    public override string ToString()
    {
        return string.Join(' ', _classes);
    }

    /// <summary>
    /// A builder for adding classes.
    /// </summary>
    public interface IClassBuilder
    {
        /// <summary>
        /// Adds a class.
        /// </summary>
        /// <param name="name">The class name.</param>
        IClassBuilder AddClass(string name);

        /// <summary>
        /// Adds a class if <paramref name="condition"/> is <see langword="true"/>.
        /// </summary>
        /// <param name="name">The class name.</param>
        /// <param name="condition">The condition that must be <see langword="true"/> for the class to be added.</param>
        IClassBuilder AddClass(string name, bool condition);

        /// <summary>
        /// Adds a class if <paramref name="condition"/> is <see langword="true"/>.
        /// </summary>
        /// <param name="name">The class name.</param>
        /// <param name="condition">The condition that must be <see langword="true"/> for the class to be added.</param>
        IClassBuilder AddClass(Func<string> name, bool condition);
    }

    private sealed class Builder : IClassBuilder
    {
        private readonly HashSet<string> _classs;

        public Builder(HashSet<string> classs)
        {
            _classs = classs;
        }

        public IClassBuilder AddClass(string name)
        {
            _classs.Add(name);

            return this;
        }

        public IClassBuilder AddClass(string name, bool condition)
        {
            if (condition)
            {
                return AddClass(name);
            }

            return this;
        }

        public IClassBuilder AddClass(Func<string> name, bool condition)
        {
            if (condition)
            {
                return AddClass(name());
            }

            return this;
        }
    }
}
