namespace AniNexus.Authentication;

/// <summary>
/// A One-Time-Password provider.
/// </summary>
public interface IOtpProvider
{
    /// <summary>
    /// Returns the QRCode URI that a user can use to add the OTP generator to their authentication application.
    /// </summary>
    /// <param name="userSecret">The user secret to use.</param>
    string GetQRCodeUri(string userSecret);

    /// <summary>
    /// Returns the QR code as a PNG byte array that a user can scan to add the OTP generator to their authentication application.
    /// </summary>
    /// <param name="userSecret"></param>
    /// <returns></returns>
    byte[] GetQRCode(string userSecret);

    /// <summary>
    /// Returns whether the specified code is valid for the specified user secret.
    /// </summary>
    /// <param name="userSecret">The user secret.</param>
    /// <param name="code">The code that the user has provided.</param>
    bool IsValidCode(string userSecret, string code);
}
