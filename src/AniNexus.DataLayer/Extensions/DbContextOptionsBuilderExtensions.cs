using AniNexus.Data.Triggers;
using EntityFrameworkCore.Triggered;
using Microsoft.Extensions.DependencyInjection;

namespace AniNexus.Data;

/// <summary>
/// <see cref="DbContextOptionsBuilder"/> extensions.
/// </summary>
public static class DbContextOptionsBuilderExtensions
{
    /// <summary>
    /// Indicates that AniNexus DB save triggers should be used.
    /// </summary>
    /// <param name="builder">The DB context options builder.</param>
    /// <returns>The builder for chaining.</returns>
    public static DbContextOptionsBuilder UseAniNexusTriggers(this DbContextOptionsBuilder builder)
    {
        return builder.UseTriggers(static triggerOptions =>
        {
            foreach (var triggerType in typeof(BeforeSaveTrigger<>).Assembly.GetTypes().Where(IsTriggerType))
            {
                // Perf - The call to AddTrigger does the same check for IsTriggerType but will throw
                //        an exception if it is not a trigger type. The logic is duplicated, but it is
                //        likely faster than exception handling on all the irrelevant types.
                triggerOptions.AddTrigger(triggerType, ServiceLifetime.Scoped);
            }
        });
    }

    private static bool IsTriggerType(Type type)
    {
        if (!type.IsClass || type.IsAbstract)
        {
            return false;
        }

        foreach (var i in type.GetInterfaces().Where(static t => t.IsGenericType))
        {
            var genericTypeDefinition = i.GetGenericTypeDefinition();
            if (genericTypeDefinition == typeof(IBeforeSaveTrigger<>) ||
                genericTypeDefinition == typeof(IAfterSaveTrigger<>))
            {
                return true;
            }
        }

        return false;
    }
}
