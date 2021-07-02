using AutoMapper;
using ProductsInventory.DB.Domain;
using ProductsInventory.Persistence.V1.Responses;

namespace ProductsInventory.Server.MappingProfile
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<User, UserResponse>().ReverseMap();
        }
    }
}