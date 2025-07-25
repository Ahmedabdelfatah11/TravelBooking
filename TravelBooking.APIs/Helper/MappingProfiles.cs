using TravelBooking.APIs.Dtos.HotelCompany;
using TravelBooking.APIs.Dtos.Rooms;
using TravelBooking.Core.Models;
using TravelBooking.Core.DTOS.CarRentalCompanies;
using TravelBooking.Core.DTOS.Cars;
using TravelBooking.APIs.DTOS.Booking;
using TravelBooking.APIs.DTOS.Booking.RoomBooking;
using TravelBooking.APIs.DTOS.Booking.CarBooking;
using TravelBooking.APIs.DTOS.Booking.FlightBooking;
using TravelBooking.APIs.DTOS.Booking.TourBooking;
using AutoMapper;
using TravelBooking.Models;
using TravelBooking.APIs.DTOS.Tours;
using TravelBooking.APIs.DTOS.TourCompany;

namespace TravelBooking.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //TourCompany Mapping
            CreateMap<TourCompany, TourCompanyReadDto>()
                     .ForMember(dest => dest.Tours, opt => opt.MapFrom(src => src.Tours));
            CreateMap<Tour, TourSummaryDto>()
                     .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()));
            CreateMap<TourCompanyCreateDto, TourCompany>();
            CreateMap<TourCompanyUpdateDto, TourCompany>();
            CreateMap<Tour, TourSummaryDto>()
          .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()));

            //Tour Mapping  
            CreateMap<TourImage, string>().ConvertUsing(src => src.ImageUrl);
            CreateMap<Tour, TourReadDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()))
                .ForMember(dest => dest.TourCompanyName, opt => opt.MapFrom(src => src.TourCompany.Name))
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.TourImages));

            CreateMap<TourCreateDto, Tour>();
            CreateMap<TourUpdateDto, Tour>();

            //  HotelCompany
            CreateMap<HotelCompany, HotelCompanyReadDTO>();
            CreateMap<HotelCompanyCreateDTO, HotelCompany>();
            CreateMap<HotelCompanyUpdateDTO, HotelCompany>();


            // Room
            CreateMap<Room, RoomToReturnDTO>()
                .ForMember(dest => dest.RoomImages, opt => opt.MapFrom(src => src.Images));
            CreateMap<RoomCreateDTO, Room>()
                .ForMember(dest => dest.Images, opt => opt.Ignore()); // We'll map manually


            // RoomImage
            CreateMap<RoomImage, RoomImageReadDTO>();

            #region Room and RoomImage  
            //  Room Mappings
            CreateMap<Room, RoomToReturnDTO>()
                .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.RoomType.ToString()))
                .ForMember(dest => dest.RoomImages, opt => opt.MapFrom(src => src.Images));

            CreateMap<RoomCreateDTO, Room>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.RoomImages));

            CreateMap<RoomUpdateDTO, Room>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.RoomImages));

            //  RoomImage Mappings
            CreateMap<RoomImage, RoomImageReadDTO>();
            CreateMap<RoomImageCreateDTO, RoomImage>();
            #endregion

            //Car Mappings
            CreateMap<Car, CarDto>().ReverseMap();
            CreateMap<CarCreateUpdateDto, Car>()
    .ForMember(dest => dest.RentalCompanyId, opt => opt.Ignore());
            //CarRental
            CreateMap<CarRentalCompany, CarRentalDto>().ReverseMap();

            CreateMap<CarRentalCompany, CarRentalWithCarsDto>()
                .ForMember(dest => dest.Cars, opt => opt.MapFrom(src => src.Cars));

            CreateMap<SaveCarRentalDto, CarRentalCompany>();
            CreateMap<CarCreateUpdateDto, CarRentalCompany>();


            CreateMap<RegisterModel, ApplicationUser>();
            CreateMap<LoginModel, ApplicationUser>();

            //Booking
            CreateMap<ApplicationUser, UserDto>();
            CreateMap<Booking, BookingDto>();

            // Room Booking
            CreateMap<RoomBookingDto, Booking>()
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.RoomId, opt => opt.Ignore())
                .ForMember(dest => dest.BookingType, opt => opt.Ignore());
            CreateMap<Booking, RoomBookingResultDto>()
                .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Room != null ? src.Room.Price : 0))
                .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.RoomId))
                .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.Room.RoomType.ToString()));

            // Car Booking
            CreateMap<CarBookingDto, Booking>()
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.CarId, opt => opt.Ignore())
                .ForMember(dest => dest.BookingType, opt => opt.Ignore());
            CreateMap<Booking, CarBookingResultDto>()
                .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Car != null ? src.Car.Price : 0))
                .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
                .ForMember(dest => dest.CarModel, opt => opt.MapFrom(src => src.Car != null ? src.Car.Model : string.Empty));


            // Flight Booking
            CreateMap<FlightBookingDto, Booking>()
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.FlightId, opt => opt.Ignore())
                .ForMember(dest => dest.BookingType, opt => opt.Ignore());
            CreateMap<Booking, FlightBookingResultDto>()
                .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Flight.Price))
                .ForMember(dest => dest.DepartureTime, opt => opt.MapFrom(src => src.Flight.DepartureTime))
                .ForMember(dest => dest.ArrivalTime, opt => opt.MapFrom(src => src.Flight.ArrivalTime))
                .ForMember(dest => dest.DepartureAirport, opt => opt.MapFrom(src => src.Flight.DepartureAirport))
                .ForMember(dest => dest.ArrivalAirport, opt => opt.MapFrom(src => src.Flight.ArrivalAirport))
                .ForMember(dest => dest.FlightId, opt => opt.MapFrom(src => src.FlightId));

            // Tour Booking
            CreateMap<TourBookingDto, Booking>();

            CreateMap<Booking, TourBookingResultDto>()
                .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Tour != null ? src.Tour.Price : 0))
                .ForMember(dest => dest.TourId, opt => opt.MapFrom(src => src.TourId))
                .ForMember(dest => dest.TourName, opt => opt.MapFrom(src => src.Tour != null ? src.Tour.Name : string.Empty))
                .ForMember(dest => dest.Destination, opt => opt.MapFrom(src => src.Tour != null ? src.Tour.Destination : string.Empty));

        }
    }
}