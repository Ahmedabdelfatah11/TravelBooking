using AutoMapper;
using TravelBooking.APIs.DTOs;
using TravelBooking.Core.Models;
using TravelBooking.Helper;

namespace TravelBooking.APIs.Helper
{
    public class FlightCompanyProfile : MappingProfiles
    {
        public FlightCompanyProfile()
        {
            CreateMap<FlightCompany, FlightCompanyDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
                .ForMember(dest => dest.FlightCount, opt => opt.MapFrom(src => src.Flights.Count.ToString()))
                    ;
            CreateMap<FlightCompany, FlightCompanyDetailsDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
                .ForMember(dest => dest.FlightCount, opt => opt.MapFrom(src => src.Flights.Count.ToString()))
                .ForMember(dest => dest.Flights, opt => opt.MapFrom(src => src.Flights))
                ;
        }
    }
}
