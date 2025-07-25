

using System.ComponentModel.DataAnnotations;
using TravelBooking.Repository.Data;

namespace TravelBooking.Core.Validations
{
    public class UniqueNameAttribute: ValidationAttribute
    {

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var _context = (AppDbContext)validationContext.GetService(typeof(AppDbContext))!;

            if (value is null)
                return new ValidationResult("Name is required");

            string name = value.ToString()!;
            bool exists = _context.HotelCompanies.Any(h => h.Name.ToLower() == name.ToLower());

            if (exists)
                return new ValidationResult("Hotel name must be unique");

            return ValidationResult.Success;
        }
    }
}



