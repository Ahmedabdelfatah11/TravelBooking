using AutoMapper;
using TravelBooking.APIs.DTOs;
using TravelBooking.Core.Models;
using TravelBooking.Helper;

namespace TravelBooking.APIs.Helper
{
    public class Flightprofile : MappingProfiles
    {
        public Flightprofile() 
        {
            CreateMap<Flight,FlightDTO>()
                //.ForMember(dest => dest.AirlineName, opt => opt.MapFrom(src => src.FlightCompany.Name))
                //.ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.FlightCompany.ImageUrl))
                .ForMember(dest => dest.DepartureAirport, opt => opt.MapFrom(src => src.DepartureAirport))
                .ForMember(dest => dest.ArrivalAirport, opt => opt.MapFrom(src => src.ArrivalAirport))
                .ForMember(dest => dest.departureTime, opt => opt.MapFrom(src => src.DepartureTime))
                .ForMember(dest => dest.arrivalTime, opt => opt.MapFrom(src => src.ArrivalTime))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.FlightNumber, opt => opt.MapFrom(src => src.Id))
                .AfterMap((src, dest) =>
                {
                    if (src.FlightCompany != null)
                    {
                        dest.AirlineName = src.FlightCompany.Name;
                        dest.ImageUrl = src.FlightCompany.ImageUrl;
                        dest.rating = src.FlightCompany.Rating; // Assuming FlightCompany has a Rating property
                    }
                    else
                    {
                        dest.AirlineName = null;
                        dest.ImageUrl = null;
                        dest.rating = null; // Set to null if FlightCompany is not available
                    }
                })
                    ;
            CreateMap<Flight, FlightDetialsDTO>()
               //.ForMember(dest => dest.AirlineName, opt => opt.MapFrom(src => src.FlightCompany.Name))
               //.ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.FlightCompany.ImageUrl))
               .ForMember(dest => dest.DepartureAirport, opt => opt.MapFrom(src => src.DepartureAirport))
               .ForMember(dest => dest.ArrivalAirport, opt => opt.MapFrom(src => src.ArrivalAirport))
               .ForMember(dest => dest.departureTime, opt => opt.MapFrom(src => src.DepartureTime))
               .ForMember(dest => dest.arrivalTime, opt => opt.MapFrom(src => src.ArrivalTime))
               .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
               .ForMember(dest => dest.FlightNumber, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AvailableSeats, opt => opt.MapFrom(src => src.AvailableSeats))
                .ForMember(dest => dest.FlightCompany, opt => opt.MapFrom(src => src.FlightCompany))

                   ;

        }
    }
}
