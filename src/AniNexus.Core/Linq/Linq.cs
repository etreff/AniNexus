namespace AniNexus.Linq;

/// <summary>
/// <see cref="IEnumerable{T}"/> extensions.
/// </summary>
public static partial class Linq
{
    /// <summary>
    /// A random number generator.
    /// </summary>
    private static MersenneTwisterRandom _linqRandom { get; set; } = new MersenneTwisterRandom();

    /// <summary>
    /// Seeds the random number generator used for extensions that
    /// require randomness.
    /// </summary>
    /// <param name="seed">The seed.</param>
    public static void SeedRandom(ulong seed)
    {
        _linqRandom = new MersenneTwisterRandom(seed);
    }

    /// <summary>
    /// Seeds the random number generator used for extensions that
    /// require randomness.
    /// </summary>
    /// <param name="seed">The seed.</param>
    public static void SeedRandom(ulong[] seed)
    {
        _linqRandom = new MersenneTwisterRandom(seed);
    }
}
