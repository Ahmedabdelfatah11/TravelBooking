using AutoMapper;
using TravelBooking.APIs.Dtos.HotelCompany;
using TravelBooking.APIs.Dtos.Rooms;
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

            //  HotelCompany
            CreateMap<HotelCompany, HotelCompanyReadDTO>();
            CreateMap<HotelCompanyCreateDTO, HotelCompany>();


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

        }
    }
}