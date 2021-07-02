using AutoMapper;
using ProductsInventory.DB.Domain;
using ProductsInventory.Persistence.V1.Requests.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsInventory.Server.MappingProfile
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<PaginationQuery, PaginationFilter>();     
        }
    }
}