using System.ComponentModel.DataAnnotations;

namespace TravelBooking.APIs.DTOs
{
    public class FlightCompanyDTO
    {
        [Required]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
