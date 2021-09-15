namespace AniNexus.Models;

/// <summary>
/// The category of a manga.
/// </summary>
public enum EMangaCategory : byte
{
    /// <summary>
    /// The manga is a manga with more than one chapter.
    /// </summary>
    Manga = 1,

    /// <summary>
    /// The manga is a comic book with more than one chapter.
    /// </summary>
    Comic = 2,

    /// <summary>
    /// The manga is released by an official publisher as one or more novels.
    /// </summary>
    LightNovel = 3,

    /// <summary>
    /// The manga is released on the internet as one or more novels.
    /// </summary>
    WebNovel = 4,

    /// <summary>
    /// The manga is self-published.
    /// </summary>
    Doujinshi = 5,

    /// <summary>
    /// The manga is released by an official publisher or on the internet as a comic or novel
    /// that only contains a single chapter.
    /// </summary>
    OneShot = 6,

    /// <summary>
    /// The manga is a picture book.
    /// </summary>
    PictureBook = 7,

    /// <summary>
    /// The manga falls under a category that is not defined.
    /// </summary>
    Other = 99
}
