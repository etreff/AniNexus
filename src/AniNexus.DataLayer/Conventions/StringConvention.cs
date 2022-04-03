using Microsoft.EntityFrameworkCore.Metadata;

namespace AniNexus.Data.Conventions;

/// <summary>
/// A convention that automatically sets the collation to <see cref="Collation.Japanese"/>
/// if the property is a <see cref="string"/> and has certaining naming conventions.
/// </summary>
internal sealed class StringConvention : IPreConfigureEntityConvention
{
    public void PreConfigure(ModelBuilder builder, IMutableEntityType entityType)
    {
        foreach (var entityProperty in entityType.GetProperties())
        {
            if (entityProperty.ClrType == typeof(string))
            {
                // Default to English ASCII characters.
                entityProperty.SetIsUnicode(false);

                // Descriptors should be unicode since they can contain foreign characters.
                if (entityProperty.Name.StartsWith("Native") ||
                    entityProperty.Name.Equals("Synopsis") ||
                    entityProperty.Name.Equals("Description") ||
                    entityProperty.Name.Equals("Comment"))
                {
                    entityProperty.SetIsUnicode(true);
                    entityProperty.SetCollation(Collation.Japanese);
                }

                // Urls can only contain ascii characters.
                // Uris may refer to something on disk, so we do not enforce Unicode.
                if (entityProperty.Name.EndsWith("Url"))
                {
                    entityProperty.SetIsUnicode(false);
                }
            }
        }
    }
}
