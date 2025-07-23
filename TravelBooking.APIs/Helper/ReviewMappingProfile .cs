using AutoMapper;
using Jwt.Helper;
using TravelBooking.APIs.DTOS.Review;
using TravelBooking.Core.Models;

namespace TravelBooking.APIs.Helper
{
    public class ReviewMappingProfile : Profile
    {
        public ReviewMappingProfile()
        {
            CreateMap<Review, ReviewDto>()
               .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => GetCompanyName(src)))
               .ForMember(dest => dest.CompanyDescription, opt => opt.MapFrom(src => GetCompanyDescription(src)))
               .ForMember(dest => dest.CompanyImageUrl, opt => opt.MapFrom(src => GetCompanyImageUrl(src)))
               .ForMember(dest => dest.CompanyLocation, opt => opt.MapFrom(src => GetCompanyLocation(src)))
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : null))
               .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User != null ? src.User.Email : null));

            CreateMap<CreateReviewDto, Review>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<UpdateReviewDto, Review>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CompanyType, opt => opt.Ignore())
                .ForMember(dest => dest.HotelCompanyId, opt => opt.Ignore())
                .ForMember(dest => dest.FlightCompanyId, opt => opt.Ignore())
                .ForMember(dest => dest.CarRentalCompanyId, opt => opt.Ignore())
                .ForMember(dest => dest.TourCompanyId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        }

        private static string? GetCompanyName(Review review)
        {
            return review.CompanyType.ToLower() switch
            {
                "hotel" => review.HotelCompany?.Name,
                "flight" => review.FlightCompany?.Name,
                "carrental" => review.CarRentalCompany?.Name,
                "tour" => review.TourCompany?.Name,
                _ => null
            };
        }

        private static string? GetCompanyDescription(Review review)
        {
            return review.CompanyType.ToLower() switch
            {
                "hotel" => review.HotelCompany?.Description,
                "flight" => review.FlightCompany?.Description,
                "carrental" => review.CarRentalCompany?.description,
                "tour" => review.TourCompany?.Description,
                _ => null
            };
        }

        private static string? GetCompanyImageUrl(Review review)
        {
            return review.CompanyType.ToLower() switch
            {
                "hotel" => review.HotelCompany?.ImageUrl,
                "flight" => review.FlightCompany?.ImageUrl,
                "carrental" => review.CarRentalCompany?.ImageUrl,
                "tour" => review.TourCompany?.ImageUrl,
                _ => null
            };
        }

        private static string? GetCompanyLocation(Review review)
        {
            return review.CompanyType.ToLower() switch
            {
                "hotel" => review.HotelCompany?.Location,
                "flight" => review.FlightCompany?.Location,
                "carrental" => review.CarRentalCompany?.Location,
                "tour" => review.TourCompany?.Location,
                _ => null
            };
        }
    }
}

