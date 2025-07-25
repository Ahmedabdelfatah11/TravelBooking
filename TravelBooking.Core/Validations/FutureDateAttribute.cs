using System.ComponentModel.DataAnnotations;

public class FutureDateAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime date && date <= DateTime.Now)
        {
            return new ValidationResult(ErrorMessage ?? "Date must be in the future.");
        }
        return ValidationResult.Success!;
    }
}