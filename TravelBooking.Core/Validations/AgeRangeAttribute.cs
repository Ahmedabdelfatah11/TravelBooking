namespace TravelBooking.APIs.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AgeRangeAttribute : ValidationAttribute
    {
        public int MinimumAge { get; }
        public int MaximumAge { get; }

        public AgeRangeAttribute(int minAge, int maxAge)
        {
            MinimumAge = minAge;
            MaximumAge = maxAge;
            ErrorMessage = $"Age must be between {MinimumAge} and {MaximumAge} years.";
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;
            if (value is not DateTime dateOfBirth)
                return new ValidationResult("Invalid date format.");

            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;

            if (dateOfBirth > today.AddYears(-age)) age--; // adjust for birthdate not reached this year

            return (age >= MinimumAge && age <= MaximumAge)
                ? ValidationResult.Success
                : new ValidationResult(ErrorMessage);
        }
    }
}
