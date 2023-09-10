using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.DataAccess;

/// <summary>
/// Database collations.
/// </summary>
public static class Collation
{
    /// <summary>
    /// The unicode char set.
    /// </summary>
    public const string UnicodeCharSet = "utf8mb4";

    /// <summary>
    /// Case sensitive ASCII collation.
    /// </summary>
    public const string CaseSensitive = "latin1_general_cs";

    /// <summary>
    /// Case insensitive ASCII collation.
    /// </summary>
    public const string CaseInsensitive = "latin1_general_ci";

    /// <summary>
    /// Case sensitive unicode collation.
    /// </summary>
	public const string UnicodeCaseSensitive = "utf8mb4_unicode_cs";

    /// <summary>
    /// Case insensitive unicode collation.
    /// </summary>
    public const string UnicodeCaseInsensitive = "utf8mb4_unicode_ci";

    /// <summary>
    /// Configures the property to use a Unicode collation.
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="caseSensitive">Whether the property is case-sensitive.</param>
    public static PropertyBuilder<string> UseUnicode(this PropertyBuilder<string> builder, bool caseSensitive = false)
    {
        return builder
            .IsUnicode()
            .UseCollation(caseSensitive ? UnicodeCaseSensitive : UnicodeCaseInsensitive);
    }
}
