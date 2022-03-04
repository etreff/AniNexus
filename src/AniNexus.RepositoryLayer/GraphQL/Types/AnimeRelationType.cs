using AniNexus.Models;
using HotChocolate.Types;

namespace AniNexus.Data.GraphQL.Types;

public class AnimeRelationType : EnumType<EMediaRelationType>
{
    protected override void Configure(IEnumTypeDescriptor<EMediaRelationType> descriptor)
    {
        descriptor.BindValuesImplicitly();
    }
}
