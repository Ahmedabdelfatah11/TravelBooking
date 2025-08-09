using AutoMapper;
using TravelBooking.APIs.DTOS.Favoritet;
using TravelBooking.Core.Models;
using TravelBooking.Helper;

namespace TravelBooking.APIs.Helper
{
    public class FavoritetMappingProfile : Profile
    {
        public FavoritetMappingProfile()
        {
            CreateMap<Favoritet, FavoritetDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => GetCompanyName(src)))
                .ForMember(dest => dest.CompanyDescription, opt => opt.MapFrom(src => GetCompanyDescription(src)))
                .ForMember(dest => dest.CompanyImageUrl, opt => opt.MapFrom(src => GetCompanyImageUrl(src)))
                .ForMember(dest => dest.CompanyLocation, opt => opt.MapFrom(src => GetCompanyLocation(src)))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : null))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User != null ? src.User.Email : null));

            CreateMap<CreateFavoriteTDto, Favoritet>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        }

        private static string? GetCompanyName(Favoritet favoritet)
        {
            return favoritet.CompanyType.ToLower() switch
            {
                "hotel" => favoritet.HotelCompany?.Name,
                "flight" => favoritet.Flight?.FlightCompany?.Name,
                "carrental" => favoritet.CarRentalCompany?.Name,
                "tour" => favoritet.TourCompany?.Name,
                _ => null
            };
        }

        private static string? GetCompanyDescription(Favoritet favoritet)
        {
            return favoritet.CompanyType.ToLower() switch
            {
                "hotel" => favoritet.HotelCompany?.Description,
                "flight" => favoritet.Flight?.FlightCompany?.Description,
                "carrental" => favoritet.CarRentalCompany?.description,
                "tour" => favoritet.TourCompany?.Description,
                _ => null
            };
        }

        private static string? GetCompanyImageUrl(Favoritet favoritet)
        {
            return favoritet.CompanyType.ToLower() switch
            {
                "hotel" => favoritet.HotelCompany?.ImageUrl,
                "flight" => favoritet.Flight?.FlightCompany?.ImageUrl,
                "carrental" => favoritet.CarRentalCompany?.ImageUrl,
                "tour" => favoritet.TourCompany?.ImageUrl,
                _ => null
            };
        }

        private static string? GetCompanyLocation(Favoritet favoritet)
        {
            return favoritet.CompanyType.ToLower() switch
            {
                "hotel" => favoritet.HotelCompany?.Location,
                "flight" => favoritet.Flight?.FlightCompany?.Location,
                "carrental" => favoritet.CarRentalCompany?.Location,
                "tour" => favoritet.TourCompany?.Location,
                _ => null
            };
        }
    }
}
