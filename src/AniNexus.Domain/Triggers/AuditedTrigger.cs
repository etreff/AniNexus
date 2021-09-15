using AniNexus.Domain.Models;
using EntityFrameworkCore.Triggered;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AniNexus.Domain.Triggers;

public class AuditedTrigger : IAfterSaveTrigger<IHasAudit>
{
    private readonly ApplicationDbContext DbContext;

    public AuditedTrigger(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task AfterSave(ITriggerContext<IHasAudit> context, CancellationToken cancellationToken)
    {
        var entry = DbContext.Entry((object)context.Entity);
        var entityType = DbContext.Model.FindEntityType(context.Entity.GetType());

        // We can't get any useful information out of this.
        if (entityType is null)
        {
            //TODO: Do we want to log this?
            return;
        }

        string? tableName = entityType.GetTableName();
        var primaryKey = entityType.FindPrimaryKey();

        if (string.IsNullOrEmpty(tableName) && primaryKey is null)
        {
            // We can't get any useful information out of this.
            return;
        }

        string? affectedKeys = null;
        if (!string.IsNullOrWhiteSpace(tableName) && primaryKey is not null)
        {
            var table = StoreObjectIdentifier.Table(tableName);
            affectedKeys = string.Join(',', primaryKey.Properties.Select(p => $"{p.GetColumnName(table)}={entry.Property(p.Name).CurrentValue}"));
        }
        else if (primaryKey is not null)
        {
            affectedKeys = string.Join(',', primaryKey.Properties.Select(p => entry.Property(p.Name).CurrentValue));
        }

        var auditModel = new AuditModel
        {
            AffectedKeys = affectedKeys,
            Action = context.ChangeType.ToString(),
            Date = context.ChangeType == ChangeType.Deleted ? DateTime.UtcNow : context.Entity.DateUpdated,
            Table = tableName ?? "Unknown Table",
            //TODO: Grab the userId from somewhere.
            UserId = null
        };

        DbContext.Audits.Add(auditModel);
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}
