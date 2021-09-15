using Microsoft.EntityFrameworkCore;

namespace AniNexus.Domain.Models
{
    /// <summary>
    /// When added to a class, the model will have soft delete functionality.
    /// </summary>
    public interface IHasSoftDelete
    {
        /// <summary>
        /// Whether this entry is soft-deleted. It will not be included in queries unless
        /// <see cref="EntityFrameworkQueryableExtensions.IgnoreQueryFilters{TEntity}(IQueryable{TEntity})"/>
        /// is invoked.
        /// </summary>
        public bool IsSoftDeleted { get; set; }
    }
}
