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
            // Seed Car Rental Companies
            if (!context.CarRentalCompanies.Any())
            {
                var carsData = await File.ReadAllTextAsync("../TravelBooking.Repository/Data/DataSeed/CarRentalAndCars.json");
                var seedData = JsonSerializer.Deserialize<SeedDataModel>(carsData, options);

                if (seedData?.CarRentalCompanies is not null && seedData.CarRentalCompanies.Count > 0)
                {
                    var carRentalCompanies = seedData.CarRentalCompanies.Select(crc => new CarRentalCompany
                    {
                        Name = crc.Name,
                        description = crc.description,
                        Location = crc.Location,
                        ImageUrl = crc.ImageUrl,
                        Rating = crc.Rating
                    }).ToList();

                    foreach (var company in carRentalCompanies)
                        await context.Set<CarRentalCompany>().AddAsync(company);

                    await context.SaveChangesAsync();
                }
            }

            // Seed Cars
            if (!context.Cars.Any())
            {
                var carsData = await File.ReadAllTextAsync("../TravelBooking.Repository/Data/DataSeed/CarRentalAndCars.json");
                var seedData = JsonSerializer.Deserialize<SeedDataModel>(carsData, options);

                if (seedData?.Cars is not null && seedData.Cars.Count > 0)
                {
                    var cars = seedData.Cars.Select(c => new Car
                    {
                        Model = c.Model,
                        Price = c.Price,
                        Description = c.Description,
                        IsAvailable = c.IsAvailable,
                        DepartureTime = c.DepartureTime,
                        ArrivalTime = c.ArrivalTime,
                        Location = c.Location,
                        ImageUrl = c.ImageUrl,
                        Capacity = c.Capacity,
                        RentalCompanyId = c.RentalCompanyId
                    }).ToList();

                    foreach (var car in cars)
                        await context.Set<Car>().AddAsync(car);

                    await context.SaveChangesAsync();
                }
            }
            if (!context.HotelCompanies.Any())
            {
                var hotelData = await File.ReadAllTextAsync("../TravelBooking.Repository/Data/DataSeed/HotelData.json");
                var seedData = JsonSerializer.Deserialize<HotelSeedDataModel>(hotelData, options);

                if (seedData?.HotelCompanies is not null && seedData.HotelCompanies.Count > 0)
                {
                    var hotelCompanies = seedData.HotelCompanies.Select(hc => new HotelCompany
                    {
                        Name = hc.Name,
                        Description = hc.Description,
                        Location = hc.Location,
                        ImageUrl = hc.ImageUrl,
                        Rating = hc.Rating
                    }).ToList();

                    foreach (var company in hotelCompanies)
                        await context.Set<HotelCompany>().AddAsync(company);

                    await context.SaveChangesAsync();
                }
            }

            // Seed Rooms with Images
            if (!context.Rooms.Any())
            {
                var hotelData = await File.ReadAllTextAsync("../TravelBooking.Repository/Data/DataSeed/HotelData.json");
                var seedData = JsonSerializer.Deserialize<HotelSeedDataModel>(hotelData, options);

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
                            From = roomData.From,
                            To = roomData.To,
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
            // Seed Tour Companies
            if (!context.TourCompanies.Any())
            {
                var tourJson = await File.ReadAllTextAsync("../TravelBooking.Repository/Data/DataSeed/TourData.json");
                var tourSeed = JsonSerializer.Deserialize<TourSeedDataModel>(tourJson, options);

                if (tourSeed?.TourCompanies is not null && tourSeed.TourCompanies.Count > 0)
                {
                    foreach (var company in tourSeed.TourCompanies)
                    {
                        var tourCompany = new TourCompany
                        {
                            Name = company.Name,
                            Description = company.Description,
                            ImageUrl = company.ImageUrl,
                            Location = company.Location,
                            Rating = company.rating,
                            //AdminId = company.AdminId
                        };

                        await context.Set<TourCompany>().AddAsync(tourCompany);
                    }

                    await context.SaveChangesAsync();
                }
            }

            // Seed Tours
            if (!context.Tours.Any())
            {
                var tourJson = await File.ReadAllTextAsync("../TravelBooking.Repository/Data/DataSeed/TourData.json");
                var tourSeed = JsonSerializer.Deserialize<TourSeedDataModel>(tourJson, options);

                if (tourSeed?.Tours is not null && tourSeed.Tours.Count > 0)
                {
                    foreach (var tourData in tourSeed.Tours)
                    {
                        if (!Enum.TryParse<TourCategory>(tourData.Category, out var parsedCategory))
                            continue; // skip invalid category

                        var tour = new Tour
                        {
                            Name = tourData.Name,
                            StartDate = tourData.StartDate,
                            EndDate = tourData.EndDate,
                            Description = tourData.Description,
                            Destination = tourData.Destination,
                            MaxGuests = tourData.MaxGuests,
                            Price = tourData.Price,
                            Category = parsedCategory,
                            TourCompanyId = tourData.TourCompanyId,
                            TourImages = tourData.TourImages.Select(img => new TourImage
                            {
                                ImageUrl = img.ImageUrl
                            }).ToList()
                        };

                        await context.Set<Tour>().AddAsync(tour);
                    }

                    await context.SaveChangesAsync();
                }
            }

        }

    }
}

