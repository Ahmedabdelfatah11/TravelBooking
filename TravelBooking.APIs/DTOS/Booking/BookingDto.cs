
using TravelBooking.Core.Models;

namespace TravelBooking.APIs.DTOS.Booking
{
    public class BookingDto 
    {
        //public int? RoomId { get; set; }  // or CarId, FlightId, etc., depending on service
        public string? Status { get; set; }
        //public string? BookingType { get; set; }
   
        //public string? UserId { get; set; }
        public object? AgencyDetails { get; set; }  // Polymorphic DTO: HotelDto, CarRentalDto, etc.
    }

}
