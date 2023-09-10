namespace AniNexus.Authentication;

internal sealed class OtpOptions
{
    /// <summary>
    /// The OTP code issuer.
    /// </summary>
    public string Issuer { get; set; } = default!;

    /// <summary>
    /// The name of the OTP code.
    /// </summary>
    /// <remarks>
    /// This will be displayed in the user's authentication app.
    /// </remarks>
    public string Name { get; set; } = default!;
}
