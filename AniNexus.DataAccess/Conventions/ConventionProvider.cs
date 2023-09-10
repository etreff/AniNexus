using System.Reflection;
using AniNexus.Reflection;

namespace AniNexus.DataAccess.Conventions;

internal static class ConventionProvider
{
    public static IEnumerable<Type> GetEntityConventions()
        => GetEntityConventions(typeof(IPreConfigureEntityConvention).Assembly);

    public static IEnumerable<Type> GetEntityConventions(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes().Where(IsEntityConventionType))
        {
            yield return type;
        }
    }

    public static IEnumerable<Type> GetEntityConventions(IEnumerable<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
        {
            foreach (var convention in GetEntityConventions(assembly))
            {
                yield return convention;
            }
        }
    }

    public static IEnumerable<IEntityConvention> CreateEntityConventions()
        => CreateEntityConventions(GetEntityConventions());

    public static IEnumerable<IEntityConvention> CreateEntityConventions(params Type[] types)
        => CreateEntityConventions(types as IEnumerable<Type>);

    public static IEnumerable<IEntityConvention> CreateEntityConventions(IEnumerable<Type> types)
    {
        return CreateEntityConventions(Activator.CreateInstance!, types);
    }

    public static IEnumerable<IEntityConvention> CreateEntityConventions(IServiceProvider serviceProvider, params Type[] types)
        => CreateEntityConventions(serviceProvider, types as IEnumerable<Type>);

    public static IEnumerable<IEntityConvention> CreateEntityConventions(IServiceProvider serviceProvider, IEnumerable<Type> types)
    {
        return CreateEntityConventions(serviceProvider.CreateService, types);
    }

    private static IEnumerable<IEntityConvention> CreateEntityConventions(Func<Type, object[], object> factory, IEnumerable<Type> types)
    {
        foreach (var type in types)
        {
            if (!IsEntityConventionType(type))
            {
                throw new ArgumentException($"Type {type.Name} must be, concrete, non-abstract, and implement {nameof(IPreConfigureEntityConvention)} or {nameof(IPostConfigureEntityConvention)}.", nameof(types));
            }
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
        foreach (var type in assembly.GetTypes().Where(IsTypeConventionType))
        {
            yield return type;
        }
    }

    public static IEnumerable<Type> GetTypeConventions(IEnumerable<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
        {
            foreach (var convention in GetTypeConventions(assembly))
            {
                yield return convention;
            }
        }
    }

    public static IEnumerable<ITypeConvention> CreateTypeConventions()
        => CreateTypeConventions(GetTypeConventions());

    public static IEnumerable<ITypeConvention> CreateTypeConventions(params Type[] types)
        => CreateTypeConventions(types as IEnumerable<Type>);

    public static IEnumerable<ITypeConvention> CreateTypeConventions(IEnumerable<Type> types)
    {
        return CreateTypeConventions(Activator.CreateInstance!, types);
    }

    public static IEnumerable<ITypeConvention> CreateTypeConventions(IServiceProvider serviceProvider, params Type[] types)
        => CreateTypeConventions(serviceProvider, types as IEnumerable<Type>);

    public static IEnumerable<ITypeConvention> CreateTypeConventions(IServiceProvider serviceProvider, IEnumerable<Type> types)
    {
        return CreateTypeConventions(serviceProvider.CreateService, types);
    }

    private static IEnumerable<ITypeConvention> CreateTypeConventions(Func<Type, object[], object> factory, IEnumerable<Type> types)
    {
        foreach (var type in types)
        {
            if (!IsTypeConventionType(type))
            {
                throw new ArgumentException($"Type {type.Name} must be, concrete, non-abstract, and implement {nameof(ITypeConvention)}.", nameof(types));
            };

            yield return (ITypeConvention)factory(type, Array.Empty<object>());
        }
    }

    private static bool IsTypeConventionType(Type t)
    {
        return t.IsTypeOf<ITypeConvention>() && !t.IsAbstract && !t.IsInterface;
    }
}
