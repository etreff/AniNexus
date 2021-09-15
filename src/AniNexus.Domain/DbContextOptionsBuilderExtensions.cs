using AniNexus.Domain.Triggers;
using EntityFrameworkCore.Triggered;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AniNexus.Domain;

public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder<TContext> UseAniNexusTriggers<TContext>(this DbContextOptionsBuilder<TContext> builder)
        where TContext : DbContext
    {
        return builder.UseTriggers(static triggerOptions =>
        {
            foreach (var triggerType in typeof(BeforeSaveTriggerBase<>).Assembly.GetTypes().Where(IsTriggerType))
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
