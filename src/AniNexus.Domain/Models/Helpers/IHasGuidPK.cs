namespace AniNexus.Domain.Models
{
    /// <summary>
    /// When added to a class, the model will have a GUID as the primary key.
    /// </summary>
    public interface IHasGuidPK
    {
        /// <summary>
        /// The Id of the entity.
        /// </summary>
        public Guid Id { get; set; }
    }
}
