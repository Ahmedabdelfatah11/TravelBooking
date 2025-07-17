using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Core.Models
{
    public class Flight : BaseEntity
    {
        [Required(ErrorMessage = "Departure time is required.")]
        [FutureDate(ErrorMessage = "Departure time must be in the future.")]
        public DateTime DepartureTime { get; set; }

        [Required(ErrorMessage = "Arrival time is required.")]
        [GreaterThan("DepartureTime", ErrorMessage = "Arrival time must be after departure time.")]
        public DateTime ArrivalTime { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Standard seats are required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Available standard seats cannot be negative.")]
        public int AvailableSeats { get; set; }
        
        //[Required(ErrorMessage = "Business class seats are required.")]
        //[Range(0, int.MaxValue, ErrorMessage = "Available business class seats cannot be negative.")]
        //public int AvailableBusinessSeats { get; set; }

        //[Required(ErrorMessage = "First class seats are required.")]
        //[Range(0, int.MaxValue, ErrorMessage = "Available first class seats cannot be negative.")]
        //public int AvailableFirstClassSeats { get; set; }

        [Required(ErrorMessage = "Departure airport is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Departure airport name must be between 3 and 100 characters.")]
        public string DepartureAirport { get; set; } = string.Empty;

        [Required(ErrorMessage = "Arrival airport is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Arrival airport name must be between 3 and 100 characters.")]
        public string ArrivalAirport { get; set; } = string.Empty;

        [ForeignKey("FlightCompany")]
        public int FlightId { get; set; }
        public FlightCompany? FlightCompany { get; set; }
    }
}
