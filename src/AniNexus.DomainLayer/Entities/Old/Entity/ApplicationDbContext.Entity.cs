using AniNexus.Domain.Models;

namespace AniNexus.Domain;

public partial class ApplicationDbContext
{
    /// <summary>
    /// A collection of characters that exist in all pieces of media.
    /// </summary>
    public DbSet<MediaCharacterModel> Characters => Set<MediaCharacterModel>();

    /// <summary>
    /// A collection of media companies.
    /// </summary>
    public DbSet<MediaCompanyModel> Companies => Set<MediaCompanyModel>();

    /// <summary>
    /// A collection of people involved in the creation of a piece of media.
    /// </summary>
    public DbSet<MediaPersonModel> People => Set<MediaPersonModel>();
}
