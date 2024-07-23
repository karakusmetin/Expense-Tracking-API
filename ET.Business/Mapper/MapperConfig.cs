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
            CreateMap<ApplicationUserResponse, ApplicationUser>().ReverseMap()
                .ForMember(x=>x.UserId, x=>x.MapFrom(x=>x.Id));
            CreateMap<Transaction, TransactionResponse>()
                .ForMember(x => x.TransactionId, x => x.MapFrom(x => x.Id));
            CreateMap<TransactionResponse, Transaction>()
                .ForMember(x => x.Id, x => x.MapFrom(x => x.TransactionId));
            CreateMap<Transaction, TransactionRequest>().ReverseMap()
                .ForMember(x => x.Id, x => x.MapFrom(x => x.TransactionId));

        }
    }
}
