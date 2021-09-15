namespace AniNexus;

/// <summary>
/// The role a person plays on a piece of media.
/// </summary>
public enum EPersonRole : byte
{
    /// <summary>
    /// The person is the original creator.
    /// </summary>
    OriginalCreator = 1,

    /// <summary>
    /// The person was an editor.
    /// </summary>
    Editor = 2,

    /// <summary>
    /// The person was a director.
    /// </summary>
    Director = 3,

    /// <summary>
    /// The person was a producer.
    /// </summary>
    Producer = 4,

    /// <summary>
    /// The person was a director of animation.
    /// </summary>
    AnimationDirector = 5,

    /// <summary>
    /// The person was a sound director.
    /// </summary>
    SoundDirector = 6,

    /// <summary>
    /// The person directed one or more episodes.
    /// </summary>
    EpisodeDirector = 7,

    /// <summary>
    /// This person helped with storyboarding.
    /// </summary>
    Storyboard = 8,

    /// <summary>
    /// This person helped writing the script.
    /// </summary>
    ScriptWriter = 9,

    /// <summary>
    /// This person helped develop the character designs.
    /// </summary>
    CharacterDesign = 10,

    /// <summary>
    /// This person created the original character designs.
    /// </summary>
    OriginalCharacterDesign = 11,

    /// <summary>
    /// This person helped with key animation.
    /// </summary>
    KeyAnimation = 12,

    /// <summary>
    /// This person helped with planning.
    /// </summary>
    Planning = 13,

    /// <summary>
    /// This person performed a theme song.
    /// </summary>
    ThemeSongPerformance = 14,

    /// <summary>
    /// This person was a programmer.
    /// </summary>
    Programmer
}
