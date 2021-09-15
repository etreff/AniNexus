﻿using AniNexus.Models;
using HotChocolate.Types;

namespace AniNexus.GraphQL.Types;

public class AnimeListStatusType : EnumType<EAnimeListStatus>
{
    protected override void Configure(IEnumTypeDescriptor<EAnimeListStatus> descriptor)
    {
        descriptor.BindValuesImplicitly();
    }
}
