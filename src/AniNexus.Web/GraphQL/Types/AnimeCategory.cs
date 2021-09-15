using AniNexus.Models;
using HotChocolate.Types;

namespace AniNexus.GraphQL.Types;

public class AnimeCategoryType : EnumType<EAnimeCategory>
{
    protected override void Configure(IEnumTypeDescriptor<EAnimeCategory> descriptor)
    {
        descriptor.BindValuesImplicitly();
    }
}
