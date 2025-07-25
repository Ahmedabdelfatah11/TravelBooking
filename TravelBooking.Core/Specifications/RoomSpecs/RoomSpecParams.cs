using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.Specifications.RoomSpecs
{
    public class RoomSpecParams : CarSpecParams
    {
        public RoomType? RoomType { get; set; }
        public string? Search { get; set; } //RoomType
        public bool? IsAvailable { get; set; }  // filter 
        public string? Sort { get; set; } //   price 
    }
}
