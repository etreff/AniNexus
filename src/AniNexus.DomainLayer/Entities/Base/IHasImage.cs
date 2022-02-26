namespace AniNexus.Domain.Entities;

/// <summary>
/// When added to a class, indicates that it may have an image to represent the entity.
/// </summary>
public interface IHasImage
{
    /// <summary>
    /// The Id of the image.
    /// </summary>
    Guid? ImageId { get; set; }
}
