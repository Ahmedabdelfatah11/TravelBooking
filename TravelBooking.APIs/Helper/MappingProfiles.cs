using AutoMapper;
using TravelBooking.APIs.Dtos.TourCompany;
using TravelBooking.APIs.Dtos.Tours;
using TravelBooking.Core.Models;

namespace TravelBooking.Helper
{
    public class MappingProfiles:Profile
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

        }
    }
}
