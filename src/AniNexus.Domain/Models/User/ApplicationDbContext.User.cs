using AniNexus.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AniNexus.Domain;

public partial class ApplicationDbContext
{
    /// <summary>
    /// A collection of users.
    /// </summary>
    public DbSet<UserModel> Users => Set<UserModel>();

    /// <summary>
    /// A collection of user claims.
    /// </summary>
    public DbSet<UserClaimModel> UserClaims => Set<UserClaimModel>();
}
