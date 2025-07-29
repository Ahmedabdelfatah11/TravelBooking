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
        public List<CarRentalCompanySeedData> CarRentalCompanies { get; set; } = new List<CarRentalCompanySeedData>();
        public List<CarSeedData> Cars { get; set; } = new List<CarSeedData>();
        public List<HotelCompany> HotelCompanies { get; set; } = new();
        public List<Room> Rooms { get; set; } = new();
    }
    public class CarRentalCompanySeedData
    {
        public string Name { get; set; }
        public string description { get; set; }
        public string Location { get; set; }
        public string ImageUrl { get; set; }
        public string Rating { get; set; }
    }

    public class CarSeedData
    {
        public string Model { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string Location { get; set; }
        public string ImageUrl { get; set; }
        public int Capacity { get; set; }
        public int RentalCompanyId { get; set; }
    }

}
