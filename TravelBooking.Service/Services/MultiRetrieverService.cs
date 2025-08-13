// MultiRetrieverService.cs
using System.Text.Json;
using TravelBooking.Core.Models;

namespace TravelBooking.Service.Services
{
    public class MultiRetrieverService
    {
        private readonly List<Car> _carData;
        private readonly List<Room> _roomData;
        private readonly List<Flight> _flightData;

        public MultiRetrieverService()
        {
            // Load car data
            var carJsonPath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "cars_with_embeddings.json");
            _carData = File.Exists(carJsonPath) ?
                JsonSerializer.Deserialize<List<Car>>(File.ReadAllText(carJsonPath)) ?? new List<Car>() :
                new List<Car>();

            // Load room data (you'll need to create rooms_with_embeddings.json)
            var roomJsonPath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "rooms_with_embeddings.json");
            _roomData = File.Exists(roomJsonPath) ?
                JsonSerializer.Deserialize<List<Room>>(File.ReadAllText(roomJsonPath)) ?? new List<Room>() :
                new List<Room>();

            // Load flight data (you'll need to create flights_with_embeddings.json)
            var flightJsonPath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "flights_with_embeddings.json");
            _flightData = File.Exists(flightJsonPath) ?
                JsonSerializer.Deserialize<List<Flight>>(File.ReadAllText(flightJsonPath)) ?? new List<Flight>() :
                new List<Flight>();
        }

        public List<Car> GetTopCarMatches(float[] queryEmbedding, int count = 5)
        {
            return _carData
                .Where(c => c.Embedding != null)
                .Select(c => new
                {
                    Car = c,
                    Score = ComputeCosineSimilarity(queryEmbedding, c.Embedding!)
                })
                .OrderByDescending(x => x.Score)
                .Take(count)
                .Select(x => x.Car)
                .ToList();
        }

        public List<Room> GetTopRoomMatches(float[] queryEmbedding, int count = 5)
        {
            return _roomData
                .Where(r => r.Embedding != null)
                .Select(r => new
                {
                    Room = r,
                    Score = ComputeCosineSimilarity(queryEmbedding, r.Embedding!)
                })
                .OrderByDescending(x => x.Score)
                .Take(count)
                .Select(x => x.Room)
                .ToList();
        }

        public List<Flight> GetTopFlightMatches(float[] queryEmbedding, int count = 5)
        {
            return _flightData
                .Where(f => f.Embedding != null)
                .Select(f => new
                {
                    Flight = f,
                    Score = ComputeCosineSimilarity(queryEmbedding, f.Embedding!)
                })
                .OrderByDescending(x => x.Score)
                .Take(count)
                .Select(x => x.Flight)
                .ToList();
        }

        private float ComputeCosineSimilarity(float[] a, float[] b)
        {
            if (a.Length != b.Length) return 0;

            float dot = 0, magA = 0, magB = 0;
            for (int i = 0; i < a.Length; i++)
            {
                dot += a[i] * b[i];
                magA += a[i] * a[i];
                magB += b[i] * b[i];
            }

            if (magA == 0 || magB == 0) return 0;
            return dot / (float)(Math.Sqrt(magA) * Math.Sqrt(magB));
        }
        public List<Flight> GetAllFlights()
        {
            return _flightData; // Your existing flight data
        }
    }
}