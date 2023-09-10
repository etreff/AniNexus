using Microsoft.Extensions.DependencyInjection;

namespace System;

/// <summary>
/// <see cref="IServiceProvider"/> extensions.
/// </summary>
public static class ServiceProviderExtensions
{
    /// <summary>
    /// Instantiate a type with constructor arguments provided directly and/or from an <see cref="IServiceProvider"/>.
    /// </summary>
    /// <typeparam name="T">The type to activate.</typeparam>
    /// <param name="services">The service provider used to resolve dependencies.</param>
    /// <param name="args">Constructor arguments not provided by the provider.</param>
    /// <exception cref="ArgumentNullException"><paramref name="services"/> is <see langword="null"/>.</exception>
    public static T CreateService<T>(this IServiceProvider services, params object[] args)
    {
        return ActivatorUtilities.CreateInstance<T>(services, args);
    }

    /// <summary>
    /// Instantiate a type with constructor arguments provided directly and/or from an <see cref="IServiceProvider"/>.
    /// </summary>
    /// <param name="services">The service provider used to resolve dependencies.</param>
    /// <param name="type">The type of the object to activate.</param>
    /// <param name="args">Constructor arguments not provided by the provider.</param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="services"/> is <see langword="null"/> -or-
    ///     <paramref name="type"/> is <see langword="null"/>.
    /// </exception>
    public static object CreateService(this IServiceProvider services, Type type, params object[] args)
    {
        return ActivatorUtilities.CreateInstance(services, type, args);
    }

    /// <summary>
    /// Instantiate a type with constructor arguments provided directly and/or from an <see cref="IServiceProvider"/>.
    /// </summary>
    /// <typeparam name="T">The type to activate.</typeparam>
    /// <param name="services">The service provider used to resolve dependencies.</param>
    /// <param name="type">The type of the object to activate. Must be assignable to type <typeparamref name="T"/>.</param>
    /// <param name="args">Constructor arguments not provided by the provider.</param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="services"/> is <see langword="null"/> -or-
    ///     <paramref name="type"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidCastException"><paramref name="type"/> is not assignable to type <typeparamref name="T"/>.</exception>
    public static T CreateService<T>(this IServiceProvider services, Type type, params object[] args)
    {
        return (T)ActivatorUtilities.CreateInstance(services, type, args);
    }
}
