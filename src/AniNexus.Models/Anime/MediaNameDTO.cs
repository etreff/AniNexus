namespace AniNexus.Models.Anime
{
    /// <summary>
    /// A name of a piece of media.
    /// </summary>
    public class MediaNameDTO
    {
        /// <summary>
        /// The name in the native language.
        /// </summary>
        public string? NativeName { get; set; }

        /// <summary>
        /// The romanization of the native name.
        /// </summary>
        public string? RomajiName { get; set; }

        /// <summary>
        /// The name in English.
        /// </summary>
        public string? EnglishName { get; set; }
    }
}
