using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;

namespace TravelBooking.Repository.Data
{
    public class SeedDataModel
    {
        public List<HotelCompany> HotelCompanies { get; set; } = new();
        public List<Room> Rooms { get; set; } = new();
    }
}
