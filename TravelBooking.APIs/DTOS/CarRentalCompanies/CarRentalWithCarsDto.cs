using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.DTOS.Cars;

namespace TravelBooking.Core.DTOS.CarRentalCompanies
{
    public class CarRentalWithCarsDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? Location { get; set; }
        public string? Rating { get; set; }
        public string? AdminId { get; set; }
        public ICollection<CarDto>? Cars { get; set; }
    }
}
