using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Core.Models
{
    public class User:BaseEntity
    {
        public string FirstName { get; set; } 
        public string LastName { get; set; } 
        public string Email { get; set; } 
        public string Password{ get; set; }   
        public string Phone { get; set; }

        public ICollection<Booking> bookings { get; set; } = new List<Booking>(); 
    } 
}
