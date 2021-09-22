using AniNexus.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AniNexus.Domain;

public partial class ApplicationDbContext
{
    /// <summary>
    /// A collection of media series.
    /// </summary>
    public DbSet<MediaSeriesModel> Series => Set<MediaSeriesModel>();
}
