namespace AniNexus.Authentication;

internal sealed class PasswordHashingOptions
{
    /// <summary>
    /// The number of iterations.
    /// </summary>
    public int Iterations { get; set; } = 10_000;
}
