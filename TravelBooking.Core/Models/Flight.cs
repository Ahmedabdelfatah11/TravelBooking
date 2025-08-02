using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelBooking.Core.Models;

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
    public int EconomySeats { get; set; }

    public int BusinessSeats { get; set; }
    public int FirstClassSeats { get; set; }

    public decimal EconomyPrice { get; set; }
    public decimal BusinessPrice { get; set; }
    public decimal FirstClassPrice { get; set; }

    [Required(ErrorMessage = "Departure airport is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Departure airport name must be between 3 and 100 characters.")]
    public string DepartureAirport { get; set; } = string.Empty;

    [Required(ErrorMessage = "Arrival airport is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Arrival airport name must be between 3 and 100 characters.")]
    public string ArrivalAirport { get; set; } = string.Empty;

    public int FlightCompanyId { get; set; }

    [ForeignKey("FlightCompanyId")]
    public FlightCompany? FlightCompany { get; set; }

    public SeatClass? SelectedSeatClass { get; set; }

    public decimal GetPrice(SeatClass? seatClass)
    {
        return seatClass switch
        {
            SeatClass.Economy => EconomyPrice,
            SeatClass.Business => BusinessPrice,
            SeatClass.FirstClass => FirstClassPrice,
            _ => 0
        };
    }
}
