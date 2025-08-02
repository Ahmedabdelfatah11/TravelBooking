using AutoMapper;
using TravelBooking.APIs.DTOS.Flight;
using TravelBooking.APIs.DTOS.Tours;
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
                .ForMember(dest => dest.DepartureTime, opt => opt.MapFrom(src => src.DepartureTime))
                .ForMember(dest => dest.ArrivalTime, opt => opt.MapFrom(src => src.ArrivalTime))
                .ForMember(dest => dest.FlightId, opt => opt.MapFrom(src => src.Id))
                .AfterMap((src, dest) =>
                {
                    if (src.FlightCompany != null)
                    {
                        dest.flightCompanyId = src.FlightCompanyId;
                        dest.AirlineName = src.FlightCompany.Name;
                        dest.ImageUrl = src.FlightCompany.ImageUrl;
                    }
                    else
                    {
                        dest.AirlineName = null;
                        dest.ImageUrl = null;
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
               .ForMember(dest => dest.FlightNumber, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EconomyPrice, opt => opt.MapFrom(src => src.EconomyPrice))
                .ForMember(dest => dest.BusinessPrice, opt => opt.MapFrom(src => src.BusinessPrice))
                .ForMember(dest => dest.FirstClassPrice, opt => opt.MapFrom(src => src.FirstClassPrice))
                .ForMember(dest => dest.EconomySeats, opt => opt.MapFrom(src => src.EconomySeats))
                .ForMember(dest => dest.BusinessSeats, opt => opt.MapFrom(src => src.BusinessSeats))
                .ForMember(dest => dest.FirstClassSeats, opt => opt.MapFrom(src => src.FirstClassSeats))

                .ForMember(dest => dest.FlightCompany, opt => opt.MapFrom(src => src.FlightCompany))

                   ;


            CreateMap<FlightDTO, Flight>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FlightId))
                .ForMember(dest => dest.DepartureAirport, opt => opt.MapFrom(src => src.DepartureAirport))
                .ForMember(dest => dest.ArrivalAirport, opt => opt.MapFrom(src => src.ArrivalAirport))
                .ForMember(dest => dest.DepartureTime, opt => opt.MapFrom(src => src.DepartureTime))
                .ForMember(dest => dest.ArrivalTime, opt => opt.MapFrom(src => src.ArrivalTime))
                .ForMember(dest => dest.FlightCompanyId, opt => opt.MapFrom(src => src.flightCompanyId))
                .ForPath(dest => dest.FlightCompany.Name, opt => opt.MapFrom(src=>src.AirlineName))
                .ForPath(dest => dest.FlightCompany.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                ;
            CreateMap<Flight, FlightCreateDTO>()
                .ForMember(dest => dest.DepartureAirport, opt => opt.MapFrom(src => src.DepartureAirport))
                 .ForMember(dest => dest.ArrivalAirport, opt => opt.MapFrom(src => src.ArrivalAirport))
                .ForMember(dest => dest.DepartureTime, opt => opt.MapFrom(src => src.DepartureTime))
                .ForMember(dest => dest.ArrivalTime, opt => opt.MapFrom(src => src.ArrivalTime))
                .ForMember(dest => dest.FlightCompanyId, opt => opt.MapFrom(src => src.FlightCompanyId))
                 .ForMember(dest => dest.EconomyPrice, opt => opt.MapFrom(src => src.EconomyPrice))
                .ForMember(dest => dest.BusinessPrice, opt => opt.MapFrom(src => src.BusinessPrice))
                .ForMember(dest => dest.FirstClassPrice, opt => opt.MapFrom(src => src.FirstClassPrice))
                .ForMember(dest => dest.EconomySeats, opt => opt.MapFrom(src => src.EconomySeats))
                .ForMember(dest => dest.BusinessSeats, opt => opt.MapFrom(src => src.BusinessSeats))
                .ForMember(dest => dest.FirstClassSeats, opt => opt.MapFrom(src => src.FirstClassSeats))
                ;
            CreateMap< FlightCreateDTO, Flight>();
        }
    }
}
