using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AniNexus.Domain.Validation
{
    internal static class UriValidator
    {
        public static bool Validate(string? uriString, string memberName, [NotNullWhen(false)] out ValidationResult? result)
        {
            if (uriString is not null && !Uri.TryCreate(uriString, UriKind.Absolute, out _))
            {
                result = new ValidationResult($"{memberName} is not a valid URI.", new[] { memberName });
                return false;
            }

            result = null;
            return true;
        }
    }
}
