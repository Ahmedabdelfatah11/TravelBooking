using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Service.Dto;

namespace TravelBooking.Core.DTOS
{
    public class CarRentalWithCarsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CarDto> Cars { get; set; }
    }
}
