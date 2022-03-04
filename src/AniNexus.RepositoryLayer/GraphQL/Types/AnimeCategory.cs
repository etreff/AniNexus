using AniNexus.Models;
using HotChocolate.Types;

namespace AniNexus.Data.GraphQL.Types;

public class AnimeCategoryType : EnumType<EAnimeCategory>
{
    protected override void Configure(IEnumTypeDescriptor<EAnimeCategory> descriptor)
    {
        descriptor.BindValuesImplicitly();
    }
}
