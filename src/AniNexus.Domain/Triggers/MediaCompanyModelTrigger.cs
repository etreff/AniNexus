using AniNexus.Domain.Models;
using EntityFrameworkCore.Triggered;

namespace AniNexus.Domain.Triggers;

public class MediaCompanyModelTrigger : IBeforeSaveTrigger<MediaCompanyModel>
{
    private readonly ApplicationDbContext DbContext;

    public MediaCompanyModelTrigger(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public Task BeforeSave(ITriggerContext<MediaCompanyModel> context, CancellationToken cancellationToken)
    {
        if (context.ChangeType == ChangeType.Deleted)
        {
            DeleteCreatorMapOnCreatorDelete(context);
        }

        return Task.CompletedTask;
    }

    private void DeleteCreatorMapOnCreatorDelete(ITriggerContext<MediaCompanyModel> context)
    {
        DbContext.Set<MediaCompanyRelatedMapModel>().RemoveRange(r => r.CompanyId == context.Entity.Id || r.RelatedCompanyId == context.Entity.Id);
    }
}
