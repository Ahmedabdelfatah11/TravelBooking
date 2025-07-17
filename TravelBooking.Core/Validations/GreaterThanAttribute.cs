using System.ComponentModel.DataAnnotations;

public class GreaterThanAttribute : ValidationAttribute
{
    private readonly string _otherProperty;

    public GreaterThanAttribute(string otherProperty)
    {
        _otherProperty = otherProperty;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var currentValue = (DateTime)value;
        var otherPropertyInfo = validationContext.ObjectType.GetProperty(_otherProperty);

        if (otherPropertyInfo == null)
        {
            return new ValidationResult($"Unknown property: {_otherProperty}");
        }

        var otherValue = (DateTime)otherPropertyInfo.GetValue(validationContext.ObjectInstance)!;

        if (currentValue <= otherValue)
        {
            return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} must be greater than {_otherProperty}.");
        }

        return ValidationResult.Success!;
    }
}