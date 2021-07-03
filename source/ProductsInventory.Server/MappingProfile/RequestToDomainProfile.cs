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
            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<UpdateCategoryRequest, Category>();            
        }
    }
}