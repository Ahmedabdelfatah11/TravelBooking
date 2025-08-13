// BuildVectorStore.cs
using System.Text.Json;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using TravelBooking.Core.Models;

namespace TravelBooking.Service.Tools
{
    public class BuildVectorStore
    {
        private const string GeminiApiKey = "AIzaSyAqFzYqZLqr_OoNFa1juLEvSS2wImfdpVU";
        private const string EmbeddingModel = "embedding-001";

        public static async Task RunForAllTypesAsync()
        {
            await RunForCarsAsync();
            await RunForRoomsAsync();
            await RunForFlightsAsync();
        }

        public static async Task RunForCarsAsync()
        {
            Console.WriteLine("⏳ Loading cars...");
            var json = await File.ReadAllTextAsync("Data/DataSeed/CarRentalAndCars.json");
            var cars = JsonSerializer.Deserialize<List<Car>>(json) ?? new();

            using var client = new HttpClient();
            var results = new List<Car>();

            foreach (var car in cars)
            {
                var input = $"{car.Model} {car.Location} {car.Description} and {car.Capacity} capacity, priced at ${car.Price}";
                car.Embedding = await GetEmbeddingAsync(client, input);
                results.Add(car);
                Console.WriteLine($"✅ Embedded: {car.Model}");
            }

            await SaveResultsAsync(results, "Data/Embedding/cars_with_embeddings.json");
        }

        public static async Task RunForRoomsAsync()
        {
            Console.WriteLine("⏳ Loading rooms...");
            var json = await File.ReadAllTextAsync("Data/DataSeed/HotelData.json");
            var hotelData = JsonSerializer.Deserialize<HotelData>(json) ?? new();
            var rooms = hotelData.Rooms;

            using var client = new HttpClient();
            var results = new List<Room>();

            foreach (var room in rooms)
            {
                var hotel = hotelData.HotelCompanies.FirstOrDefault(h => h.Id == room.HotelId);
                var input = $"{room.RoomType} Room at {hotel?.Name} in {hotel?.Location}: {room.Description} | Price: {room.Price} | Available: {room.From} to {room.To}";
                room.Embedding = await GetEmbeddingAsync(client, input);
                results.Add(room);
                Console.WriteLine($"✅ Embedded: {room.RoomType} Room at {hotel?.Name}");
            }

            await SaveResultsAsync(results, "Data/Embedding/rooms_with_embeddings.json");
        }

        public static async Task RunForFlightsAsync()
        {
            Console.WriteLine("⏳ Loading flights...");
            var json = await File.ReadAllTextAsync("Data/DataSeed/FlightData.json");
            var flightData = JsonSerializer.Deserialize<FlightData>(json) ?? new();
            var flights = flightData.Flights;

            using var client = new HttpClient();
            var results = new List<Flight>();

            foreach (var flight in flights)
            {
                var company = flightData.FlightCompanies.FirstOrDefault(fc => fc.Id == flight.FlightCompanyId);
                var input = $"{company?.Name} flight from {flight.DepartureAirport} to {flight.ArrivalAirport} | Depart: {flight.DepartureTime} | Arrive: {flight.ArrivalTime} | Economy Price: {flight.EconomyPrice}";
                flight.Embedding = await GetEmbeddingAsync(client, input);
                results.Add(flight);
                Console.WriteLine($"✅ Embedded: Flight {flight.Id} by {company?.Name}");
            }

            await SaveResultsAsync(results, "Data/Embedding/flights_with_embeddings.json");
        }

        private static async Task SaveResultsAsync<T>(List<T> items, string outputPath)
        {
            var outputJson = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(outputPath, outputJson);
            Console.WriteLine($"🎉 All embeddings saved to: {outputPath}");
        }

        private static async Task<float[]> GetEmbeddingAsync(HttpClient client, string text)
        {
            var url = $"https://generativelanguage.googleapis.com/v1/models/{EmbeddingModel}:embedContent?key={GeminiApiKey}";
            var requestBody = new
            {
                content = new
                {
                    parts = new[] { new { text = text } }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"❌ Gemini Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");

            var resultJson = await response.Content.ReadAsStringAsync();
            using var json = JsonDocument.Parse(resultJson);
            return json.RootElement
                .GetProperty("embedding")
                .GetProperty("values")
                .EnumerateArray()
                .Select(v => v.GetSingle())
                .ToArray();
        }
    }

    // Helper classes for JSON deserialization
    public class HotelData
    {
        public List<HotelCompany> HotelCompanies { get; set; } = new();
        public List<Room> Rooms { get; set; } = new();
    }

    public class FlightData
    {
        public List<FlightCompany> FlightCompanies { get; set; } = new();
        public List<Flight> Flights { get; set; } = new();
    }
}