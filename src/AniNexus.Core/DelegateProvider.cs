using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace AniNexus;

/// <summary>
/// A class that contains cached no-op delegates.
/// </summary>
public static class Noop
{
    /// <summary>
    /// An action that does nothing and takes no arguments.
    /// </summary>
    public static Action Instance { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; } = static () => { };
}

/// <summary>
/// A class that contains cached no-op delegates.
/// </summary>
public static class Noop<T>
{
    /// <summary>
    /// An action that does nothing and takes 1 argument of type <typeparamref name="T"/>.
    /// </summary>
    public static Action<T> Instance { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; } = static _ => { };
}

/// <summary>
/// A class that contains cached no-op delegates.
/// </summary>
public static class Noop<T1, T2>
{
    /// <summary>
    /// An action that does nothing and takes 2 arguments of type <typeparamref name="T1"/> and <typeparamref name="T2"/>.
    /// </summary>
    public static Action<T1, T2> Instance { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; } = static (_, __) => { };
}

/// <summary>
/// A class that contains cached, default-returning delegates.
/// </summary>
public static class FuncProvider<T>
{
    /// <summary>
    /// A <see cref="Func{T}"/> that accepts no arguments and always returns the default value of <typeparamref name="T"/>.
    /// </summary>
    public static Func<T> Null { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; } = new(static () => default!);

    /// <summary>
    /// A <see cref="Func{T, TResult}"/> that accepts an argument and returns that argument as the result.
    /// </summary>
    public static Func<T, T> ReturnSelf { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; } = new(static v => v);

    /// <summary>
    /// A <see cref="Func{T, TResult}"/> that takes an <see cref="IEnumerable{T}"/> and returns an <see cref="IReadOnlyCollection{T}"/>.
    /// </summary>
    public static Func<IEnumerable<T>, IReadOnlyCollection<T>> EnumerableToReadOnlyCollection { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; } = static c =>
    {
        return c is IList<T> list
            ? new ReadOnlyCollection<T>(list)
            : new ReadOnlyCollection<T>(c.ToArray());
    };

    /// <summary>
    /// A <see cref="Func{T, TResult}"/> that takes an <see cref="IEnumerable{T}"/> and returns an <see cref="IReadOnlyList{T}"/>.
    /// </summary>
    public static Func<IEnumerable<T>, IReadOnlyList<T>> EnumerableToReadOnlyList { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; } = static c =>
    {
        return c is IList<T> list
            ? new ReadOnlyCollection<T>(list)
            : new ReadOnlyCollection<T>(c.ToList());
    };
}

/// <summary>
/// A class that contains cached, default-returning delegates.
/// </summary>
public static class FuncProvider<TArg, TResult>
{
    /// <summary>
    /// A <see cref="Func{TArg, TResult}"/> that accepts no arguments and always returns the default value of <typeparamref name="TResult"/>.
    /// </summary>
    public static Func<TArg, TResult> Null { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; } = new(static _ => default!);

    /// <summary>
    /// A <see cref="Func{T, TResult}"/> that returns the <see cref="IGrouping{TKey, TElement}.Key"/> element.
    /// </summary>
    public static Func<IGrouping<TArg, TResult>, TArg> ReturnKeyFromGrouping { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; } = new(static g => g.Key);
}

/// <summary>
/// A class that contains cached, default-returning delegates.
/// </summary>
public static class FuncProvider<TArg1, TArg2, TResult>
{
    /// <summary>
    /// A <see cref="Func{TArg, TResult}"/> that accepts no arguments and always returns the default value of <typeparamref name="TResult"/>.
    /// </summary>
    public static Func<TArg1, TArg2, TResult> Null { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; } = new(static (_, __) => default!);
}

/// <summary>
/// A class that contains cached predicate delegates.
/// </summary>
public static class PredicateProvider<T>
{
    /// <summary>
    /// A predicate that returns <see langword="true"/> regardless of input.
    /// </summary>
    public static Predicate<T> True { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; } = static _ => true;

    /// <summary>
    /// A predicate that returns <see langword="true"/> regardless of input.
    /// </summary>
    public static Func<T, bool> TrueFunc { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; } = new(True);

    /// <summary>
    /// A predicate that returns <see langword="false"/> regardless of input.
    /// </summary>
    public static Predicate<T> False { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; } = static _ => false;

    /// <summary>
    /// A predicate that returns <see langword="false"/> regardless of input.
    /// </summary>
    public static Func<T, bool> FalseFunc { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; } = new(False);
}

/// <summary>
/// Cached delegates for common methods.
/// </summary>
public static class Delegates
{
    /// <summary>
    /// <see cref="string"/> related delegates.
    /// </summary>
    public static class String
    {
        /// <summary>
        /// <see cref="string.IsNullOrEmpty(string?)"/>
        /// </summary>
        public static Func<string?, bool> IsNullOrEmpty { get; } = new(string.IsNullOrEmpty);

        /// <summary>
        /// <see cref="string.IsNullOrWhiteSpace(string?)"/>
        /// </summary>
        public static Func<string?, bool> IsNullOrWhiteSpace { get; } = new(string.IsNullOrWhiteSpace);

        /// <summary>
        /// <see cref="string.IsNullOrEmpty(string?)"/>
        /// </summary>
        public static Func<string?, bool> IsNotNullOrEmpty { get; } = new(static s => !string.IsNullOrEmpty(s));

        /// <summary>
        /// <see cref="string.IsNullOrWhiteSpace(string?)"/>
        /// </summary>
        public static Func<string?, bool> IsNotNullOrWhiteSpace { get; } = new(static s => !string.IsNullOrWhiteSpace(s));
    }

    /// <summary>
    /// Delegates that take an element of item <typeparamref name="T"/> and return a <see cref="string"/>.
    /// </summary>
    public static class String<T>
    {
        private static readonly Lazy<Func<T, string>> _lazyToString = new(() => new(static v => v?.ToString()!), LazyThreadSafetyMode.PublicationOnly);
        private static readonly Lazy<Func<T, string>> _lazyToStringFormattableInvariant = new(static () => new Func<T, string>(v => (v as IFormattable)?.ToString(null, CultureInfo.InvariantCulture)!), LazyThreadSafetyMode.PublicationOnly);

        private static readonly Lazy<Func<T, string>> _toStringInvariantLazy = typeof(T).GetInterfaces().Contains(typeof(IFormattable))
            ? _lazyToStringFormattableInvariant
            : _lazyToString;

        /// <summary>
        /// A delegate that calls <see cref="object.ToString"/> on an object.
        /// </summary>
        public static Func<T, string> ToStringInvariant => _toStringInvariantLazy.Value;
    }
}
