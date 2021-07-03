using AutoMapper;
using NSubstitute;
using ProductsInventory.Persistence.Interfaces.Services;
using ProductsInventory.Server.Controllers.V1;

namespace ProductInventory.Tests.Common.Builders.Controllers
{
    public class CategoriesControllerBuilder
    {
        public CategoriesControllerBuilder()
        {
            Mapper = Substitute.For<IMapper>();
            UriService = Substitute.For<IUriService>();
            CategoryService = Substitute.For<ICategoryService>();            
        }

        public IMapper Mapper { get; private set; }
        public IUriService UriService { get; private set; }
        public ICategoryService CategoryService { get; private set; }        

        public CategoriesControllerBuilder WithMapper(IMapper mapper)
        {
            Mapper = mapper;
            return this;
        }

        public CategoriesControllerBuilder WithUriService(IUriService uriService)
        {
            UriService = uriService;
            return this;
        }

        public CategoriesControllerBuilder WithCategoryService(ICategoryService categoryService)
        {
            CategoryService = categoryService;
            return this;
        }

        public CategoriesController Build()
        {
            return new CategoriesController(CategoryService, Mapper, UriService);
        }
    }
}
