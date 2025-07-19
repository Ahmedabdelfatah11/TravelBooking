using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Core.Models
{
    public class TourImage:BaseEntity
    {
        public string ImageUrl { get; set; } = string.Empty;
        public int TourId { get; set; }
        public Tour? Tour { get; set; }
    }
}
