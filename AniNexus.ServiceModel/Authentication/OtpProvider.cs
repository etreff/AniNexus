using Microsoft.Extensions.Options;
using OtpNet;
using QRCoder;

namespace AniNexus.Authentication;

internal sealed class OtpProvider : IOtpProvider
{
    private readonly OtpOptions _options;

    public OtpProvider(IOptions<OtpOptions> options)
    {
        _options = options.Value;
    }

    public string GetQRCodeUri(string userSecret)
    {
        return $"otpauth://totp/{Uri.EscapeDataString(_options.Name)}?secret={userSecret}&issuer={Uri.EscapeDataString(_options.Issuer)}";
    }

    public byte[] GetQRCode(string userSecret)
    {
        using var generator = new QRCodeGenerator();
        using var codeData = generator.CreateQrCode(GetQRCodeUri(userSecret), QRCodeGenerator.ECCLevel.Q);
        using var code = new PngByteQRCode(codeData);
        return code.GetGraphic(20);
    }

    public bool IsValidCode(string userSecret, string code)
    {
        var totp = new Totp(Base32Encoding.ToBytes(userSecret));
        // We allow one code in the past/future to account for latency.
        return totp.VerifyTotp(code, out _, new VerificationWindow(1, 1));
    }
}
