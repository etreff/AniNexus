namespace AniNexus;

/// <summary>
/// The role a character in a piece of media.
/// </summary>
public enum ECharacterRole : byte
{
    /// <summary>
    /// The character played a leading role.
    /// </summary>
    Main = 1,

    /// <summary>
    /// The character played a supporting role or was a background character.
    /// </summary>
    Supporting = 2
}
