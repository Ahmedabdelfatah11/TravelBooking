using AutoMapper; 
using System.Runtime;  
using TravelBooking.Models;

namespace Jwt.Helper
{
    public class MappingFile : Profile
    {
        public MappingFile()
        {
            CreateMap<RegisterModel, ApplicationUser>();
            CreateMap<LoginModel, ApplicationUser>();
        }
    }
}
