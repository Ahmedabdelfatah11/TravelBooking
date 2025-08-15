using TravelBooking.Core.Models;
using TravelBooking.Core.DTOS.CarRentalCompanies;
using TravelBooking.Core.DTOS.Cars;
using TravelBooking.APIs.DTOS.Booking;
using TravelBooking.APIs.DTOS.Booking.RoomBooking;
using TravelBooking.APIs.DTOS.Booking.CarBooking;
using TravelBooking.APIs.DTOS.Booking.FlightBooking;
using TravelBooking.APIs.DTOS.Booking.TourBooking;
using AutoMapper;
using TravelBooking.APIs.DTOS.Tours;
using TravelBooking.APIs.DTOS.TourCompany;
using TravelBooking.Core.Models;
using TravelBooking.Models;
using TravelBooking.APIs.DTOS.HotelCompany;
using TravelBooking.APIs.DTOS.Rooms;
using TravelBooking.APIs.DTOS.TourTickets;

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
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.TourImages))
                .ForMember(dest => dest.IncludedItems, opt => opt.MapFrom(src => src.IncludedItems))
                .ForMember(dest => dest.ExcludedItems, opt => opt.MapFrom(src => src.ExcludedItems))
                .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions))
                    .ForMember(dest => dest.Tickets, opt => opt.MapFrom(src => src.TourTickets));

            CreateMap<TourCreateDto, Tour>();
            CreateMap<TourUpdateDto, Tour>();
            CreateMap<TourQuestion, TourQuestionDto>().ReverseMap();

            //  HotelCompany
            CreateMap<HotelCompany, HotelCompanyReadDTO>();
            CreateMap<HotelCompanyCreateDTO, HotelCompany>();
            CreateMap<HotelCompanyUpdateDTO, HotelCompany>();

            #region Room and RoomImage  
            // Room
            CreateMap<Room, RoomToReturnDTO>()
                 .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.RoomType.ToString()))
                .ForMember(dest => dest.HotelCompanyName, opt => opt.MapFrom(src => src.Hotel != null ? src.Hotel.Name : string.Empty))
                .ForMember(dest => dest.HotelCompanyId, opt => opt.MapFrom(src => src.HotelId))
                .ForMember(dest => dest.RoomImages, opt => opt.MapFrom(src => src.Images));
            CreateMap<Room, RoomToReturnDTO>()
                  .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.RoomType.ToString()))
                     .ForMember(dest => dest.RoomImages, opt => opt.MapFrom(src => src.Images))
                   .ForMember(dest => dest.HotelCompanyId, opt => opt.MapFrom(src => src.HotelId))  
                  .ForMember(dest => dest.HotelCompanyName, opt => opt.MapFrom(src => src.Hotel.Name));
            CreateMap<RoomCreateDTO, Room>()
                  .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.RoomImages));

           

            //  RoomImage Mappings

            CreateMap<RoomImageCreateDTO, RoomImage>();
            CreateMap<RoomImage, RoomImageReadDTO>();
            #endregion


            //Car Mappings
            CreateMap<Car, CarDto>()
                .ForMember(dest => dest.Name,
               opt => opt.MapFrom(src => src.RentalCompany != null ? src.RentalCompany.Name : string.Empty));
            CreateMap<CarDto, Car>();
            CreateMap<CarCreateUpdateDto, Car>();
              
            //CarRental
            CreateMap<CarRentalCompany, CarRentalDto>()
                 .ForMember(dest => dest.Cars, opt => opt.MapFrom(src => src.Cars))
                     .ReverseMap();

            CreateMap<CarRentalCompany, CarRentalWithCarsDto>()
                .ForMember(dest => dest.Cars, opt => opt.MapFrom(src => src.Cars));

            CreateMap<SaveCarRentalDto, CarRentalCompany>();
            CreateMap<CarCreateUpdateDto, CarRentalCompany>();
           

            CreateMap<RegisterModel, ApplicationUser>();
            CreateMap<LoginModel, ApplicationUser>();

            //Booking
            CreateMap<ApplicationUser, UserDto>();
            CreateMap<Booking, BookingDto>()
            .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.Payment.PaymentStatus.ToString()))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src =>src.Payment.Amount))
            ;



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
                .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.Room.RoomType.ToString()))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
                ;

            // Car Booking
            CreateMap<CarBookingDto, Booking>()
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.CarId, opt => opt.Ignore())
                .ForMember(dest => dest.BookingType, opt => opt.Ignore());
            CreateMap<Booking, CarBookingResultDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.TotalPrice))

                .ForMember(dest => dest.CarModel, opt => opt.MapFrom(src => src.Car != null ? src.Car.Model : string.Empty));


            // Flight Booking
            CreateMap<FlightBookingDto, Booking>()
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.FlightId, opt => opt.Ignore())
                .ForMember(dest => dest.BookingType, opt => opt.Ignore())
                .ForMember(dest => dest.SeatClass, opt => opt.MapFrom(src => src.SeatClass))
                ;
            CreateMap<Booking, FlightBookingResultDto>()
                .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.DepartureTime, opt => opt.MapFrom(src => src.Flight.DepartureTime))
                .ForMember(dest => dest.ArrivalTime, opt => opt.MapFrom(src => src.Flight.ArrivalTime))
                .ForMember(dest => dest.DepartureAirport, opt => opt.MapFrom(src => src.Flight.DepartureAirport))
                .ForMember(dest => dest.ArrivalAirport, opt => opt.MapFrom(src => src.Flight.ArrivalAirport))
                .ForMember(dest => dest.FlightId, opt => opt.MapFrom(src => src.FlightId)) 

                .ForMember(dest => dest.SeatClass, opt => opt.MapFrom(src => src.SeatClass)) 
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.TotalPrice))
                ;

            // Tour Booking
            CreateMap<TourBookingDto, Booking>();

            CreateMap<Booking, TourBookingResultDto>()
      .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.Id))
      .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
      .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice)) 
      .ForMember(dest => dest.TourId, opt => opt.MapFrom(src => src.TourId))
      .ForMember(dest => dest.TourName, opt => opt.MapFrom(src => src.Tour != null ? src.Tour.Name : string.Empty))
      .ForMember(dest => dest.Destination, opt => opt.MapFrom(src => src.Tour != null ? src.Tour.Destination : string.Empty))
      .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Tour.Category))
      .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
      .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
      .ForMember(dest => dest.Tickets, opt => opt.MapFrom(src => src.BookingTickets)); 

            CreateMap<TourBookingTicket, TourTicketSummaryDto>()
    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Ticket.Type))
    .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
    .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Ticket.Price));
            
            //Tour Ticket
            CreateMap<TourTicket, TourTicketDto>().ReverseMap();
            CreateMap<TourTicketCreateDto, TourTicket>();
        }
    }
}
