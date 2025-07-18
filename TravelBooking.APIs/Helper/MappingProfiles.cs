using AutoMapper;
using TravelBooking.APIs.DTOS;
using TravelBooking.Core.DTOS;
using TravelBooking.Core.Models;

namespace TravelBooking.Helper
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            #region Hotel and RoOM AND RoomImage 
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
            #endregion

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
