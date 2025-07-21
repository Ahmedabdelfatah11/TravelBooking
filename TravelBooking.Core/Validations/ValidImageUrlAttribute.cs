

using System.ComponentModel.DataAnnotations;

namespace TravelBooking.Core.Validations
{
    public class ValidImageUrlAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return ValidationResult.Success; // Optional

            string? url = value.ToString();
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult) ||
                !(url!.StartsWith("http") || url.StartsWith("https")))
            {
                return new ValidationResult("Image URL must be a valid HTTP/HTTPS URL.");
            }

            return ValidationResult.Success;
        }
    }
}
