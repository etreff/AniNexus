using Microsoft.EntityFrameworkCore;

namespace AniNexus.Domain
{
    public static class DbSetExtensions
    {
        /// <summary>
        /// Removes entities from a <see cref="DbSet{TEntity}"/>.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="dbSet">The <see cref="DbSet{TEntity}"/> to remove the entities from.</param>
        /// <param name="predicate">The predicate indicating which entities to remove.</param>
        public static void RemoveRange<T>(this DbSet<T> dbSet, Func<T, bool> predicate)
            where T : class
        {
            dbSet.RemoveRange(dbSet.Where(predicate));
        }
    }
}
