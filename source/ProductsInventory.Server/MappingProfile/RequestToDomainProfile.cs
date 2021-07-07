using AutoMapper;
using ProductsInventory.DB.Domain;
using ProductsInventory.Persistence.V1.Requests;
using ProductsInventory.Persistence.V1.Requests.Queries;

namespace ProductsInventory.Server.MappingProfile
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<PaginationQuery, PaginationFilter>();
            CreateMap<CreateCategoryRequest, Category>()
                   .ForMember(dest => dest.Products, opt =>
                    opt.Ignore());
            CreateMap<CreateProductRequest, Product>();
            CreateMap<UpdateCategoryRequest, Category>()
                    .ForMember(dest => dest.Products, opt =>
                    opt.Ignore());
            CreateMap<UpdateProductRequest, Product>()
                   .ForMember(dest => dest.Category, opt =>
                    opt.MapFrom(src => src.Category))
                .ReverseMap();
        }
    }
}