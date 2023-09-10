using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace AniNexus.Authentication;

internal sealed class PasswordHasher : IPasswordHasher
{
    private const KeyDerivationPrf _v1Prf = KeyDerivationPrf.HMACSHA512;

    private readonly int _iterations;

    public PasswordHasher(IOptions<PasswordHashingOptions> options)
    {
        var o = options.Value;

        _iterations = o.Iterations;
    }

    public string GeneratePasswordResetToken()
    {
        byte[] bytes = RandomNumberGenerator.GetBytes(128);
        return WebEncoders.Base64UrlEncode(bytes);
    }

    public string HashPassword(string password)
    {
        return Convert.ToBase64String(HashPasswordV1(password));
    }

    private byte[] HashPasswordV1(string password)
    {
        // https://github.com/dotnet/aspnetcore/blob/main/src/Identity/Extensions.Core/src/PasswordHasher.cs#L141
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
        byte[] subkey = KeyDerivation.Pbkdf2(password, salt, _v1Prf, _iterations, 256 / 8);

        byte[] outputBytes = new byte[13 + salt.Length + subkey.Length];
        outputBytes[0] = 0x01; // format marker

        WriteNetworkByteOrder(outputBytes, 1, (uint)_v1Prf);
        WriteNetworkByteOrder(outputBytes, 5, (uint)_iterations);
        WriteNetworkByteOrder(outputBytes, 9, (uint)salt.Length);

        Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
        Buffer.BlockCopy(subkey, 0, outputBytes, 13 + salt.Length, subkey.Length);

        return outputBytes;
    }

    public PasswordVerificationResult VerifyPassword(string hashedPassword, string password)
    {
        byte[] hashBytes = Convert.FromBase64String(hashedPassword);

        if (hashBytes.Length == 0)
        {
            return PasswordVerificationResult.Failed;
        }

        switch (hashBytes[0])
        {
            // Version 1
            case 0x01:
                if (VerifyPasswordV1(hashBytes, password, out int iterations, out KeyDerivationPrf prf))
                {
                    if (iterations != _iterations)
                    {
                        return PasswordVerificationResult.SuccessRehashNeeded;
                    }

                    if (prf != _v1Prf)
                    {
                        return PasswordVerificationResult.SuccessRehashNeeded;
                    }

                    return PasswordVerificationResult.Success;
                }

                return PasswordVerificationResult.Failed;
            // Unknown format marker
            default:
                return PasswordVerificationResult.Failed;
        }
    }

    private static bool VerifyPasswordV1(byte[] hashBytes, string password, out int iterations, out KeyDerivationPrf prf)
    {
        iterations = default;
        prf = default;

        try
        {
            prf = (KeyDerivationPrf)ReadNetworkByteOrder(hashBytes, 1);
            iterations = (int)ReadNetworkByteOrder(hashBytes, 5);
            int saltLength = (int)ReadNetworkByteOrder(hashBytes, 9);

            // Salt must be >= 128 bits.
            if (saltLength < 128 / 8)
            {
                return false;
            }

            byte[] salt = new byte[saltLength];
            Buffer.BlockCopy(hashBytes, 13, salt, 0, saltLength);

            int subkeyLength = hashBytes.Length - 13 - salt.Length;
            // Subkey must be >= 128 bits.
            if (subkeyLength < 128 / 8)
            {
                return false;
            }

            byte[] expectedSubkey = new byte[subkeyLength];
            Buffer.BlockCopy(hashBytes, 13 + salt.Length, expectedSubkey, 0, subkeyLength);

            byte[] actualSubkey = KeyDerivation.Pbkdf2(password, salt, prf, iterations, subkeyLength);
            return CryptographicOperations.FixedTimeEquals(actualSubkey, expectedSubkey);
        }
        catch
        {
            // Bad payload, you shall not pass.
            return false;
        }
    }

    private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
    {
        return ((uint)(buffer[offset + 0]) << 24)
            | ((uint)(buffer[offset + 1]) << 16)
            | ((uint)(buffer[offset + 2]) << 8)
            | ((uint)(buffer[offset + 3]));
    }

    private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
    {
        buffer[offset + 0] = (byte)(value >> 24);
        buffer[offset + 1] = (byte)(value >> 16);
        buffer[offset + 2] = (byte)(value >> 8);
        buffer[offset + 3] = (byte)(value >> 0);
    }
}
