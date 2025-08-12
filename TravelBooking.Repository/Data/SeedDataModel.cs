using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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
    public class HotelSeedDataModel
    {
        [JsonPropertyName("hotelCompanies")]
        public List<HotelCompanySeedModel> HotelCompanies { get; set; } = new();

        [JsonPropertyName("rooms")]
        public List<RoomSeedModel> Rooms { get; set; } = new();
    }

    public class HotelCompanySeedModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("location")]
        public string Location { get; set; } = string.Empty;

        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; } = string.Empty;

        [JsonPropertyName("rating")]
        public string Rating { get; set; } = string.Empty;
    }

    public class RoomSeedModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("isAvailable")]
        public bool IsAvailable { get; set; }

        [JsonPropertyName("roomType")]
        public RoomType RoomType { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("from")]
        public DateTime From { get; set; }

        [JsonPropertyName("to")]
        public DateTime To { get; set; }

        [JsonPropertyName("hotelId")]
        public int HotelId { get; set; }

        [JsonPropertyName("images")]
        public List<RoomImageSeedModel> Images { get; set; } = new();
    }

    public class RoomImageSeedModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; } = string.Empty;

        [JsonPropertyName("roomId")]
        public int RoomId { get; set; }
    }

    public class TourSeedDataModel
    {
        [JsonPropertyName("TourCompanies")]
        public List<TourCompanySeedModel> TourCompanies { get; set; } = new();

        [JsonPropertyName("Tours")]
        public List<TourSeedModel> Tours { get; set; } = new();
    }

    public class TourCompanySeedModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("imageUrl")]
        public string? ImageUrl { get; set; }
        [JsonPropertyName("location")]
        public string? Location { get; set; } // Location of the tour company, e.g., "Rome, Italy"
        [JsonPropertyName("rating")]
        public int? rating { get; set; }
        public string? AdminId { get; set; } // FK to AspNetUsers

    }

    public class TourSeedModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]

        public string Name { get; set; }
        [JsonPropertyName("StartDate")]

        public DateTime StartDate { get; set; }
        [JsonPropertyName("EndDate")]

        public DateTime EndDate { get; set; }
        [JsonPropertyName("Description")]

        public string? Description { get; set; } = string.Empty;
        public string? Destination { get; set; } = string.Empty;
        public int MaxGuests { get; set; }
        public int MinGroupSize { get; set; } // Minimum allowed group size
        public int MaxGroupSize { get; set; }
        [JsonPropertyName("price")]

        public decimal Price { get; set; }
        public string? ImageUrl { get; set; } // Primary image for quick access

        public string Category { get; set; } = string.Empty;
        public string Languages { get; set; }
        public int? TourCompanyId { get; set; }
        public List<TourImageSeedModel> TourImages { get; set; } = new();
        public List<string> IncludedItems { get; set; } = new();
        public List<string> ExcludedItems { get; set; } = new();
        public List<TourQuestionSeedModel> Questions { get; set; } = new();
        public List<TourTicketSeedModel> TourTickets { get; set; } = new();

    }

    public class TourQuestionSeedModel
    {
        public string QuestionText { get; set; } = string.Empty;
        public string AnswerText { get; set; } = string.Empty;
    }


    public class TourImageSeedModel
    {
        public string ImageUrl { get; set; } = string.Empty;
    }
    public class TourTicketSeedModel
    {
        [JsonPropertyName("Type")]
        public string TicketType { get; set; }
        public decimal Price { get; set; }
        public int MaxQuantity { get; set; }
        public bool IsActive { get; set; } = true;
    }

}
