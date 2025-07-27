using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TravelBooking.Core.Models;

namespace TravelBooking.Repository.Data
{
    public class FlightContextSeed
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            };

            // Seed Flight Companies
            if (!context.FlightCompanies.Any())
            {
                var flightData = await File.ReadAllTextAsync("../TravelBooking.Repository/Data/DataSeed/FlightData.json");
                var seedData = JsonSerializer.Deserialize<FlightSeedDataModel>(flightData, options);

                if (seedData?.FlightCompanies is not null && seedData.FlightCompanies.Count > 0)
                {
                    var flightCompanies = seedData.FlightCompanies.Select(fc => new FlightCompany
                    {
                        Name = fc.Name,
                        Description = fc.Description,
                        Location = fc.Location,
                        ImageUrl = fc.ImageUrl,
                        Rating = fc.Rating
                    }).ToList();

                    foreach (var company in flightCompanies)
                        await context.Set<FlightCompany>().AddAsync(company);

                    await context.SaveChangesAsync();
                }
            }

            // Seed Flights
            if (!context.Flights.Any())
            {
                var flightData = await File.ReadAllTextAsync("../TravelBooking.Repository/Data/DataSeed/FlightData.json");
                var seedData = JsonSerializer.Deserialize<FlightSeedDataModel>(flightData, options);

                if (seedData?.Flights is not null && seedData.Flights.Count > 0)
                {
                    var flights = seedData.Flights.Select(f => new Flight
                    {
                        DepartureTime = f.DepartureTime,
                        ArrivalTime = f.ArrivalTime,
                        EconomySeats = f.EconomySeats,
                        BusinessSeats = f.BusinessSeats,
                        FirstClassSeats = f.FirstClassSeats,
                        EconomyPrice = f.EconomyPrice,
                        BusinessPrice = f.BusinessPrice,
                        FirstClassPrice = f.FirstClassPrice,
                        DepartureAirport = f.DepartureAirport,
                        ArrivalAirport = f.ArrivalAirport,
                        FlightCompanyId = f.FlightCompanyId
                    }).ToList();

                    foreach (var flight in flights)
                        await context.Set<Flight>().AddAsync(flight);

                    await context.SaveChangesAsync();
                }
            }
        }
    }

    // Data Models for Deserialization
    public class FlightSeedDataModel
    {
        [JsonPropertyName("flightCompanies")]
        public List<FlightCompanySeedModel> FlightCompanies { get; set; } = new();

        [JsonPropertyName("flights")]
        public List<FlightSeedModel> Flights { get; set; } = new();
    }

    public class FlightCompanySeedModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; } = string.Empty;

        [JsonPropertyName("location")]
        public string Location { get; set; } = string.Empty;

        [JsonPropertyName("rating")]
        public int Rating { get; set; }
    }

    public class FlightSeedModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("departureTime")]
        public DateTime DepartureTime { get; set; }

        [JsonPropertyName("arrivalTime")]
        public DateTime ArrivalTime { get; set; }

        [JsonPropertyName("economySeats")]
        public int EconomySeats { get; set; }

        [JsonPropertyName("businessSeats")]
        public int BusinessSeats { get; set; }

        [JsonPropertyName("firstClassSeats")]
        public int FirstClassSeats { get; set; }

        [JsonPropertyName("economyPrice")]
        public decimal EconomyPrice { get; set; }

        [JsonPropertyName("businessPrice")]
        public decimal BusinessPrice { get; set; }

        [JsonPropertyName("firstClassPrice")]
        public decimal FirstClassPrice { get; set; }

        [JsonPropertyName("departureAirport")]
        public string DepartureAirport { get; set; } = string.Empty;

        [JsonPropertyName("arrivalAirport")]
        public string ArrivalAirport { get; set; } = string.Empty;

        [JsonPropertyName("flightCompanyId")]
        public int FlightCompanyId { get; set; }
    }
}