using AniNexus.Models;
using HotChocolate.Types;

namespace AniNexus.GraphQL.Types;

public class AnimeRelationType : EnumType<EMediaRelationType>
{
    protected override void Configure(IEnumTypeDescriptor<EMediaRelationType> descriptor)
    {
        descriptor.BindValuesImplicitly();
    }
}
