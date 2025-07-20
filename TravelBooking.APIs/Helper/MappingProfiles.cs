using AutoMapper;
using TravelBooking.Core.DTOS;
using TravelBooking.Core.Models;
using TravelBooking.Service.Dto;

namespace TravelBooking.Helper
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<CarDTO, CarDto>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable))
                .ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.Capacity))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .AfterMap((src,dest) =>
                {
                    dest.companyName = src.RentalCompany.Name;
                });
            CreateMap<CarCreateUpdateDto, CarDTO>();
            //CreateMap<CarRentalCompany, CarRentalDto>().ReverseMap();

            CreateMap<CarRentalCompany, CarRentalDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Cars, opt => opt.MapFrom(src => src.Cars));



            //CreateMap<SaveCarRentalDto, CarRentalCompany>();

        }
    }
}
