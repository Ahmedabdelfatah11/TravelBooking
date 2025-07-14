using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.ComponentModel.DataAnnotations.Schema; 

namespace TravelBooking.Core.Models
{
    public enum Status
    {
        Pending,
        Confirmed,
        Cancelled
    }

    public enum BookingType
    {
        Hotel,
        CarRental,
        Flight,
        Tour
    } 
    public class Booking:BaseEntity

    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Status status { get; set; }
        public BookingType BookingType { get; set; }

        public int? RefId { get; set; }
        public int? PaymentId { get; set; }
        public Payment? Payment { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; } 
    }
}
