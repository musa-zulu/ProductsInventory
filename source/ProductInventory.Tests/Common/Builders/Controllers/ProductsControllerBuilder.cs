using AutoMapper;
using NSubstitute;
using ProductsInventory.Persistence.Interfaces.Services;
using ProductsInventory.Server.Controllers.V1;

namespace ProductInventory.Tests.Common.Builders.Controllers
{
    public class ProductsControllerBuilder
    {
        public ProductsControllerBuilder()
        {
            Mapper = Substitute.For<IMapper>();
            UriService = Substitute.For<IUriService>();
            ProductService = Substitute.For<IProductService>();
        }

        public IMapper Mapper { get; private set; }
        public IUriService UriService { get; private set; }
        public IProductService ProductService { get; private set; }

        public ProductsControllerBuilder WithMapper(IMapper mapper)
        {
            Mapper = mapper;
            return this;
        }

        public ProductsControllerBuilder WithUriService(IUriService uriService)
        {
            UriService = uriService;
            return this;
        }

        public ProductsControllerBuilder WithProductService(IProductService productService)
        {
            ProductService = productService;
            return this;
        }

        public ProductsController Build()
        {
            return new ProductsController(ProductService, Mapper, UriService);
        }
    }
}
