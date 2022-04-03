using System.Reflection;
using AniNexus.Reflection;
using Microsoft.Toolkit.Diagnostics;

namespace AniNexus.Data.Conventions;

internal static class ConventionProvider
{
    public static IEnumerable<Type> GetEntityConventions()
        => GetEntityConventions(typeof(IPreConfigureEntityConvention).Assembly);

    public static IEnumerable<Type> GetEntityConventions(Assembly assembly)
    {
        Guard.IsNotNull(assembly, nameof(assembly));

        foreach (var type in assembly.GetTypes().Where(IsEntityConventionType))
        {
            yield return type;
        }
    }

    public static IEnumerable<Type> GetEntityConventions(IEnumerable<Assembly> assemblies)
    {
        Guard.IsNotNull(assemblies, nameof(assemblies));

        foreach (var assembly in assemblies)
        {
            foreach (var convention in GetEntityConventions(assembly))
            {
                yield return convention;
            }
        }
    }

    public static IEnumerable<IEntityConvention> CreateEntityConventions(params Type[] types)
        => CreateEntityConventions(types as IEnumerable<Type>);

    public static IEnumerable<IEntityConvention> CreateEntityConventions(IEnumerable<Type> types)
    {
        Guard.IsNotNull(types, nameof(types));

        return CreateEntityConventions(Activator.CreateInstance!, types);
    }

    public static IEnumerable<IEntityConvention> CreateEntityConventions(IServiceProvider serviceProvider, params Type[] types)
        => CreateEntityConventions(serviceProvider, types as IEnumerable<Type>);

    public static IEnumerable<IEntityConvention> CreateEntityConventions(IServiceProvider serviceProvider, IEnumerable<Type> types)
    {
        Guard.IsNotNull(serviceProvider, nameof(serviceProvider));
        Guard.IsNotNull(types, nameof(types));

        return CreateEntityConventions(serviceProvider.CreateService, types);
    }

    private static IEnumerable<IEntityConvention> CreateEntityConventions(Func<Type, object[], object> factory, IEnumerable<Type> types)
    {
        foreach (var type in types)
        {
            Guard.IsTrue(IsEntityConventionType(type), nameof(types), $"Type must be, concrete, non-abstract, and implement {nameof(IPreConfigureEntityConvention)} or {nameof(IPostConfigureEntityConvention)}.");
            yield return (IEntityConvention)factory(type, Array.Empty<object>());
        }
    }

    private static bool IsEntityConventionType(Type t)
    {
        return (t.IsTypeOf<IPreConfigureEntityConvention>() || t.IsTypeOf<IPostConfigureEntityConvention>()) && !t.IsAbstract && !t.IsInterface;
    }

    public static IEnumerable<Type> GetTypeConventions()
        => GetTypeConventions(typeof(ITypeConvention<>).Assembly);

    public static IEnumerable<Type> GetTypeConventions(Assembly assembly)
    {
        Guard.IsNotNull(assembly, nameof(assembly));

        foreach (var type in assembly.GetTypes().Where(IsTypeConventionType))
        {
            yield return type;
        }
    }

    public static IEnumerable<Type> GetTypeConventions(IEnumerable<Assembly> assemblies)
    {
        Guard.IsNotNull(assemblies, nameof(assemblies));

        foreach (var assembly in assemblies)
        {
            foreach (var convention in GetTypeConventions(assembly))
            {
                yield return convention;
            }
        }
    }

    public static IEnumerable<ITypeConvention> CreateTypeConventions(params Type[] types)
        => CreateTypeConventions(types as IEnumerable<Type>);

    public static IEnumerable<ITypeConvention> CreateTypeConventions(IEnumerable<Type> types)
    {
        Guard.IsNotNull(types, nameof(types));

        return CreateTypeConventions(Activator.CreateInstance!, types);
    }

    public static IEnumerable<ITypeConvention> CreateTypeConventions(IServiceProvider serviceProvider, params Type[] types)
        => CreateTypeConventions(serviceProvider, types as IEnumerable<Type>);

    public static IEnumerable<ITypeConvention> CreateTypeConventions(IServiceProvider serviceProvider, IEnumerable<Type> types)
    {
        Guard.IsNotNull(serviceProvider, nameof(serviceProvider));
        Guard.IsNotNull(types, nameof(types));

        return CreateTypeConventions(serviceProvider.CreateService, types);
    }

    private static IEnumerable<ITypeConvention> CreateTypeConventions(Func<Type, object[], object> factory, IEnumerable<Type> types)
    {
        foreach (var type in types)
        {
            Guard.IsTrue(IsTypeConventionType(type), nameof(types), $"Type must be, concrete, non-abstract, and implement {nameof(ITypeConvention)}.");
            yield return (ITypeConvention)factory(type, Array.Empty<object>());
        }
    }

    private static bool IsTypeConventionType(Type t)
    {
        return t.IsTypeOf<ITypeConvention>() && !t.IsAbstract && !t.IsInterface;
    }
}
