using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Service.Dto
{
    public class CarCreateUpdateDto
    {
        public string Model { get; set; }
        public decimal Price { get; set; }     
        public string Description { get; set; }
        public bool IsAvailable { get; set; } = true;
        public string Location { get; set; }
        public string ImageUrl { get; set; }
        public int Capacity { get; set; }
        public int? RentalCompanyId { get; set; }
    }
}
