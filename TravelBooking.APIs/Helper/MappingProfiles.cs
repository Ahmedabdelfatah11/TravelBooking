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
            CreateMap<Car, CarDto>().ReverseMap();
            CreateMap<CarCreateUpdateDto, Car>();
            CreateMap<CarRentalCompany, CarRentalDto>().ReverseMap();

            CreateMap<CarRentalCompany, CarRentalWithCarsDto>()
                .ForMember(dest => dest.Cars, opt => opt.MapFrom(src => src.Cars));

            CreateMap<SaveCarRentalDto, CarRentalCompany>();

        }
    }
}
