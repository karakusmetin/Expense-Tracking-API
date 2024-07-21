using AutoMapper;
using ET.Data.Entities;
using ET.Schema;

namespace ET.Business.Mapper
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<ApplicationUserRequest, ApplicationUser>().ReverseMap();
            CreateMap<ApplicationUserResponse, ApplicationUser>().ReverseMap();
        }
    }
}
