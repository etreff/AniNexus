using AniNexus.Domain;
using AniNexus.Domain.Models;
using AniNexus.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AniNexus.Infrastructure.Internal
{
    internal partial class DefaultUserRepository : IUserRepository
    {
        private readonly ApplicationDbContext Context;
        private readonly ILogger Logger;

        public DefaultUserRepository(ApplicationDbContext dbContext, ILogger<DefaultUserRepository> logger)
        {
            Context = dbContext;
            Logger = logger;
        }

        public async Task ClearMFAAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await Context.Users.FindAsync(new object[] { userId }, cancellationToken);
            if (user is null)
            {
                return;
            }

            LogMFADisabled(Logger, user.Username);

            user.TwoFactorEnabled = false;
            user.TwoFactorKey = null;
        }

        public async Task<string?> GetMFAKeyAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await Context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            if (user is null)
            {
                return null;
            }

            return user.TwoFactorKey;
        }

        public async Task<string?> GetMFAKeyAsync(string username, CancellationToken cancellationToken)
        {
            var user = await Context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
            if (user is null)
            {
                return null;
            }

            return user.TwoFactorKey;
        }

        public async Task<UserDTO?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await Context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            return MapUser(user);
        }

        public async Task<UserDTO?> GetUserByNameAsync(string username, CancellationToken cancellationToken)
        {
            var user = await Context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username, cancellationToken);

            return MapUser(user);
        }

        public async Task SetMFAEnabledAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await Context.Users.FindAsync(new object[] { userId }, cancellationToken);
            if (user is null)
            {
                return;
            }

            user.TwoFactorEnabled = true;
        }

        public async Task SetMFAKeyAsync(Guid userId, string key, CancellationToken cancellationToken)
        {
            var user = await Context.Users.FindAsync(new object[] { userId }, cancellationToken);
            if (user is null)
            {
                return;
            }

            user.TwoFactorKey = key;
        }

        [return: NotNullIfNotNull("user")]
        private static UserDTO? MapUser(UserModel? user)
        {
            if (user is null)
            {
                return null;
            }

            return new UserDTO(
                user.Id,
                user.Username
            );
        }

        [LoggerMessage(EventId = LoggerEvents.MFADisabling, Level = LogLevel.Information, Message = "Disabling MFA for user {Username}")]
        static partial void LogMFADisabled(ILogger logger, string username);
    }
}
