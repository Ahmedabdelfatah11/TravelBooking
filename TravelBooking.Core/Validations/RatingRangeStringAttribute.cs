

using System.ComponentModel.DataAnnotations;

namespace TravelBooking.Service.Validators
{
    public class RatingRangeStringAttribute: ValidationAttribute
    {
        private static readonly string[] allowedRatings = { "1", "2", "3", "4", "5" };

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return new ValidationResult("Rating is required");

            string? ratingStr = value.ToString();

            if (!allowedRatings.Contains(ratingStr!))
            {
                return new ValidationResult("Rating must be a string from 1 to 5");
            }

            return ValidationResult.Success;
        }
    }
}

