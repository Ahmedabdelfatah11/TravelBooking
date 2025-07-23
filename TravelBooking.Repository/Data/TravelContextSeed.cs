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
    public class TravelContextSeed
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            };

            // Seed Hotel Companies
            if (!context.HotelCompanies.Any())
            {
                var roomsData = await File.ReadAllTextAsync("../TravelBooking.Repository/Data/DataSeed/HotelAndRoom.json");

                var seedData = JsonSerializer.Deserialize<SeedDataModel>(roomsData, options);

                if (seedData?.HotelCompanies is not null && seedData.HotelCompanies.Count > 0)
                {
                    var hotelCompanies = seedData.HotelCompanies.Select(h => new HotelCompany
                    {
                        Name = h.Name,
                        Description = h.Description,
                        Location = h.Location,
                        ImageUrl = h.ImageUrl,
                        Rating = h.Rating
                    }).ToList();

                    foreach (var hotel in hotelCompanies)
                        await context.Set<HotelCompany>().AddAsync(hotel);

                    await context.SaveChangesAsync();
                }
            }

            // Seed Rooms with Images
            if (!context.Rooms.Any())
            {
                var roomsData = await File.ReadAllTextAsync("../TravelBooking.Repository/Data/DataSeed/HotelAndRoom.json");
                var seedData = JsonSerializer.Deserialize<SeedDataModel>(roomsData, options); // ✅ Now using options

                if (seedData?.Rooms is not null && seedData.Rooms.Count > 0)
                {
                    var rooms = new List<Room>();

                    foreach (var roomData in seedData.Rooms)
                    {
                        var room = new Room
                        {
                            Price = roomData.Price,
                            IsAvailable = roomData.IsAvailable,
                            RoomType = roomData.RoomType,
                            Description = roomData.Description,
                            HotelId = roomData.HotelId,
                            Images = roomData.Images?.Select(img => new RoomImage
                            {
                                ImageUrl = img.ImageUrl
                            }).ToList() ?? new List<RoomImage>()
                        };

                        rooms.Add(room);
                    }

                    foreach (var room in rooms)
                        await context.Set<Room>().AddAsync(room);

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}

