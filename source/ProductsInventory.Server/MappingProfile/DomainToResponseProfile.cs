using AutoMapper;
using ProductsInventory.DB.Domain;
using ProductsInventory.Persistence.V1.Responses;

namespace ProductsInventory.Server.MappingProfile
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Category, CategoryResponse>()
                .ForMember(dest => dest.Products, opt =>
                    opt.MapFrom(src => src.Products)).ReverseMap();
            CreateMap<Product, ProductResponse>().ReverseMap();            
        }
    }
}