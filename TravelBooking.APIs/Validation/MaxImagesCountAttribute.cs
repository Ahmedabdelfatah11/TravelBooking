using System.ComponentModel.DataAnnotations;

namespace TravelBooking.Core.Validations;

public class MaxImagesCountAttribute : ValidationAttribute
{
    private readonly int _maxCount;

    public MaxImagesCountAttribute(int maxCount)
    {
        _maxCount = maxCount;
        ErrorMessage = $"You can only upload up to {_maxCount} images.";
    }

    public override bool IsValid(object? value)
    {
        if (value is null) return true;

        if (value is List<IFormFile> files)
        {
            return files.Count <= _maxCount;
        }

        return false;
    }
}