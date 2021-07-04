using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using ProductInventory.Tests.Common.Builders.Controllers;
using ProductInventory.Tests.Common.Builders.Domain;
using ProductInventory.Tests.Common.Builders.V1.Requests;
using ProductsInventory.DB.Domain;
using ProductsInventory.Persistence.Helpers;
using ProductsInventory.Persistence.Interfaces.Services;
using ProductsInventory.Persistence.V1.Requests;
using ProductsInventory.Persistence.V1.Requests.Queries;
using ProductsInventory.Persistence.V1.Responses;
using ProductsInventory.Server.Controllers.V1;
using ProductsInventory.Server.MappingProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProductInventory.Tests.Server.Controllers
{
    [TestFixture]
    public class TestProductsController
    {
        private static IMapper _mapper;

        public TestProductsController()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new RequestToDomainProfile());
                    mc.AddProfile(new DomainToResponseProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() =>
                new ProductsController(Substitute.For<IProductService>(), Substitute.For<IMapper>(), Substitute.For<IUriService>()));
            //---------------Test Result -----------------------
        }

        [Test]
        public void Construct_GivenIProductServiceIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() =>
                    new ProductsController(null, Substitute.For<IMapper>(), Substitute.For<IUriService>()));
            //---------------Test Result -----------------------
            Assert.AreEqual("productService", ex.ParamName);
        }

        [Test]
        public void Construct_GivenIMapperIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() =>
                    new ProductsController(Substitute.For<IProductService>(), null, Substitute.For<IUriService>()));
            //---------------Test Result -----------------------
            Assert.AreEqual("mapper", ex.ParamName);
        }

        [Test]
        public void Construct_GivenIUriServiceIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() =>
                    new ProductsController(Substitute.For<IProductService>(), Substitute.For<IMapper>(), null));
            //---------------Test Result -----------------------
            Assert.AreEqual("uriService", ex.ParamName);
        }

        [Test]
        public void DateTimeProvider_GivenSetDateTimeProvider_ShouldSetDateTimeProviderOnFirstCall()
        {
            //---------------Set up test pack-------------------
            var controller = CreateProductsControllerBuilder().Build();
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            controller.DateTimeProvider = dateTimeProvider;
            //---------------Test Result -----------------------
            Assert.AreSame(dateTimeProvider, controller.DateTimeProvider);
        }

        [Test]
        public void DateTimeProvider_GivenSetDateTimeProviderIsSet_ShouldThrowOnCall()
        {
            //---------------Set up test pack-------------------
            var controller = CreateProductsControllerBuilder().Build();
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            var dateTimeProvider1 = Substitute.For<IDateTimeProvider>();
            controller.DateTimeProvider = dateTimeProvider;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var ex = Assert.Throws<InvalidOperationException>(() => controller.DateTimeProvider = dateTimeProvider1);
            //---------------Test Result -----------------------
            Assert.AreEqual("DateTimeProvider is already set", ex.Message);
        }

        [Test]
        public void GetAll_ShouldHaveHttpGetAttribute()
        {
            //---------------Set up test pack-------------------
            var methodInfo = typeof(ProductsController)
                .GetMethod("GetAll");
            //---------------Assert Precondition----------------
            Assert.IsNotNull(methodInfo);
            //---------------Execute Test ----------------------
            var httpPostAttribute = methodInfo.GetCustomAttribute<HttpGetAttribute>();
            //---------------Test Result -----------------------
            Assert.NotNull(httpPostAttribute);
        }

        [Test]
        public async Task GetAll_ShouldCallMappingEngine()
        {
            //---------------Set up test pack-------------------            
            var mappingEngine = Substitute.For<IMapper>();
            var paginationQuery = CreatePaginationQuery();
            var httpContext = Substitute.For<HttpContext>();

            ClaimsPrincipal claimsPrincipal = GetLoggedInUser();
            httpContext.User = claimsPrincipal;

            var controller = CreateProductsControllerBuilder()
                .WithMapper(mappingEngine)
                .Build();
            //---------------Assert Precondition----------------
            controller.Context = httpContext;
            //---------------Execute Test ----------------------
            var result = await controller.GetAll(paginationQuery);
            //---------------Test Result -----------------------
            mappingEngine.Received(1).Map<PaginationFilter>(paginationQuery);
        }

        [Test]
        public async Task GetAll_ShouldReturnOkResultObject_WhenProductExist()
        {
            //---------------Set up test pack-------------------
            var product = ProductBuilder.BuildRandom();
            List<Product> products = CreateProducts(product);
            var uriService = Substitute.For<IUriService>();
            var productService = Substitute.For<IProductService>();
            var httpContext = Substitute.For<HttpContext>();
            var paginationQuery = CreatePaginationQuery();
            Uri uri = CreateUri();

            ClaimsPrincipal claimsPrincipal = GetLoggedInUser();
            httpContext.User = claimsPrincipal;

            uriService.GetAllUri(Arg.Any<PaginationQuery>()).Returns(uri);
            productService.GetProductsAsync(Arg.Any<PaginationFilter>()).Returns(products);
            var controller = CreateProductsControllerBuilder()
                                   .WithProductService(productService)
                                   .WithMapper(_mapper)
                                   .WithUriService(uriService)
                                   .Build();
            //---------------Assert Precondition----------------
            controller.Context = httpContext;
            //---------------Execute Test ----------------------
            var result = await controller.GetAll(paginationQuery) as OkObjectResult;
            //---------------Test Result -----------------------            
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetAll_ShouldReturnCountOfOne_WhenProductExist()
        {
            //---------------Set up test pack-------------------
            var product = ProductBuilder.BuildRandom();
            List<Product> products = CreateProducts(product);
            var productService = Substitute.For<IProductService>();
            var paginationQuery = CreatePaginationQuery();
            HttpContextSetup(out HttpContext httpContext, out ClaimsPrincipal claimsPrincipal);
            SetUserIdToProduct(product, claimsPrincipal);
            var uriService = Substitute.For<IUriService>();
            Uri uri = CreateUri();

            uriService.GetAllUri(Arg.Any<PaginationQuery>()).Returns(uri);
            productService.GetProductsAsync(Arg.Any<PaginationFilter>()).Returns(products);

            var controller = CreateProductsControllerBuilder()
                                   .WithProductService(productService)
                                   .WithMapper(_mapper)
                                   .WithUriService(uriService)
                                   .Build();
            //---------------Assert Precondition----------------
            controller.Context = httpContext;
            //---------------Execute Test ----------------------
            var result = await controller.GetAll(paginationQuery) as OkObjectResult;
            //---------------Test Result -----------------------
            var pagedResponse = result.Value as PagedResponse<ProductResponse>;
            Assert.IsNotNull(pagedResponse);
            Assert.AreEqual(1, pagedResponse.Data.Count());
        }

        [Test]
        public async Task GetAll_ShouldReturnProduct_WhenProductExist()
        {
            //---------------Set up test pack-------------------
            var product = ProductBuilder.BuildRandom();
            HttpContextSetup(out HttpContext httpContext, out ClaimsPrincipal claimsPrincipal);
            SetUserIdToProduct(product, claimsPrincipal);
            List<Product> products = CreateProducts(product);
            var productService = Substitute.For<IProductService>();
            var paginationQuery = CreatePaginationQuery();
            var uriService = Substitute.For<IUriService>();
            Uri uri = CreateUri();

            uriService.GetAllUri(Arg.Any<PaginationQuery>()).Returns(uri);
            productService.GetProductsAsync(Arg.Any<PaginationFilter>()).Returns(products);

            var controller = CreateProductsControllerBuilder()
                                   .WithProductService(productService)
                                   .WithMapper(_mapper)
                                   .WithUriService(uriService)
                                   .Build();
            //---------------Assert Precondition----------------
            controller.Context = httpContext;
            //---------------Execute Test ----------------------
            var result = await controller.GetAll(paginationQuery) as OkObjectResult;
            //---------------Test Result -----------------------
            var pagedResponse = result.Value as PagedResponse<ProductResponse>;
            Assert.IsNotNull(pagedResponse);
            Assert.AreEqual(product.Name, pagedResponse.Data.FirstOrDefault().Name);
        }

        [Test]
        public void Get_ShouldHaveHttpGetAttribute()
        {
            //---------------Set up test pack-------------------
            var methodInfo = typeof(ProductsController)
                .GetMethod("Get");
            //---------------Assert Precondition----------------
            Assert.IsNotNull(methodInfo);
            //---------------Execute Test ----------------------
            var httpPostAttribute = methodInfo.GetCustomAttribute<HttpGetAttribute>();
            //---------------Test Result -----------------------
            Assert.NotNull(httpPostAttribute);
        }

        [Test]
        public async Task Get_ShouldReturnOkResultObject_WhenProductExist()
        {
            //---------------Set up test pack-------------------
            var product = ProductBuilder.BuildRandom();
            var productId = product.ProductId;
            var productService = Substitute.For<IProductService>();
            HttpContextSetup(out HttpContext httpContext, out ClaimsPrincipal claimsPrincipal);
            SetUserIdToProduct(product, claimsPrincipal);

            productService.GetProductByIdAsync(productId).Returns(product);

            var controller = CreateProductsControllerBuilder()
                                   .WithProductService(productService)
                                   .WithMapper(_mapper)
                                   .Build();
            //---------------Assert Precondition----------------
            controller.Context = httpContext;
            //---------------Execute Test ----------------------
            var result = await controller.Get(productId) as OkObjectResult;
            //---------------Test Result -----------------------            
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Get_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            //---------------Set up test pack-------------------
            HttpContextSetup(out HttpContext httpContext, out _);
            var controller = CreateProductsControllerBuilder().Build();
            //---------------Assert Precondition----------------
            controller.Context = httpContext;
            //---------------Execute Test ----------------------
            var result = await controller.Get(Guid.Empty) as NotFoundResult;
            //---------------Test Result -----------------------            
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        [Test]
        public async Task Get_ShouldCallMappingEngine()
        {
            //---------------Set up test pack-------------------
            var product = ProductBuilder.BuildRandom();
            var productId = product.ProductId;
            var productService = Substitute.For<IProductService>();
            var mappingEngine = Substitute.For<IMapper>();
            HttpContextSetup(out HttpContext httpContext, out ClaimsPrincipal claimsPrincipal);
            SetUserIdToProduct(product, claimsPrincipal);

            productService.GetProductByIdAsync(productId).Returns(product);

            var controller = CreateProductsControllerBuilder()
                .WithMapper(mappingEngine)
                .WithProductService(productService)
                .Build();
            //---------------Assert Precondition----------------
            controller.Context = httpContext;
            //---------------Execute Test ----------------------
            var result = await controller.Get(productId);
            //---------------Test Result -----------------------
            mappingEngine.Received(1).Map<ProductResponse>(product);
        }

        [Test]
        public async Task Get_ShouldReturnProduct_WhenProductExist()
        {
            //---------------Set up test pack-------------------
            var product = ProductBuilder.BuildRandom();
            var productId = product.ProductId;
            var productService = Substitute.For<IProductService>();
            HttpContextSetup(out HttpContext httpContext, out ClaimsPrincipal claimsPrincipal);
            SetUserIdToProduct(product, claimsPrincipal);
            productService.GetProductByIdAsync(productId).Returns(product);

            var controller = CreateProductsControllerBuilder()
                .WithMapper(_mapper)
                .WithProductService(productService)
                .Build();
            //---------------Assert Precondition----------------
            controller.Context = httpContext;
            //---------------Execute Test ----------------------
            var result = await controller.Get(productId) as OkObjectResult;
            //---------------Test Result -----------------------
            var pagedResponse = result.Value as Response<ProductResponse>;
            Assert.IsNotNull(pagedResponse);
            Assert.AreEqual(product.ProductId, pagedResponse.Data.ProductId);
            Assert.AreEqual(product.Name, pagedResponse.Data.Name);
        }


        [Test]
        public void Create_ShouldHaveHttpPostAttribute()
        {
            //---------------Set up test pack-------------------
            var methodInfo = typeof(ProductsController)
                .GetMethod("Create");
            //---------------Assert Precondition----------------
            Assert.IsNotNull(methodInfo);
            //---------------Execute Test ----------------------
            var httpPostAttribute = methodInfo.GetCustomAttribute<HttpPostAttribute>();
            //---------------Test Result -----------------------
            Assert.NotNull(httpPostAttribute);
        }

        [Test]
        public async Task Create_ShouldReturnStatusOf201_GivenAProductHasBeenSaved()
        {
            //---------------Set up test pack-------------------      
            Uri uri = CreateUri();
            HttpContextSetup(out HttpContext httpContext, out _);
            var productRequest = CreateProductRequestBuilder.BuildRandom();
            var uriService = Substitute.For<IUriService>();

            uriService.GetProductUri(Arg.Any<string>()).Returns(uri);

            var controller = CreateProductsControllerBuilder()
                                   .WithUriService(uriService)
                                   .WithMapper(_mapper)
                                   .Build();
            //---------------Assert Precondition----------------
            controller.Context = httpContext;
            //---------------Execute Test ----------------------
            var result = await controller.Create(productRequest) as CreatedResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.Created, result.StatusCode);
        }

        [Test]
        public async Task Create_ShouldSaveProduct_GivenAValidProductObject()
        {
            //---------------Set up test pack-------------------      
            Uri uri = CreateUri();
            var productRequest = CreateProductRequestBuilder.BuildRandom();
            var uriService = Substitute.For<IUriService>();
            HttpContextSetup(out HttpContext httpContext, out ClaimsPrincipal claimsPrincipal);
            uriService.GetProductUri(Arg.Any<string>()).Returns(uri);

            var controller = CreateProductsControllerBuilder()
                                   .WithUriService(uriService)
                                   .WithMapper(_mapper)
                                   .Build();
            //---------------Assert Precondition----------------
            controller.Context = httpContext;
            //---------------Execute Test ----------------------
            var result = await controller.Create(productRequest) as CreatedResult;
            //---------------Test Result -----------------------
            var createdProduct = (result.Value as Response<ProductResponse>).Data;

            Assert.AreEqual(productRequest.Name, createdProduct.Name);
            Assert.AreEqual(productRequest.ProductId, createdProduct.ProductId);            
        }

        [Test]
        public void Update_ShouldHaveHttpPutAttribute()
        {
            //---------------Set up test pack-------------------
            var methodInfo = typeof(ProductsController)
                .GetMethod("Update");
            //---------------Assert Precondition----------------
            Assert.IsNotNull(methodInfo);
            //---------------Execute Test ----------------------
            var httpPostAttribute = methodInfo.GetCustomAttribute<HttpPutAttribute>();
            //---------------Test Result -----------------------
            Assert.NotNull(httpPostAttribute);
        }

        [Test]
        public async Task Update_ShouldReturnBadRequest_GivenAProductIdIsEmpty()
        {
            //---------------Set up test pack-------------------
            var request = new UpdateProductRequest
            {
                ProductId = Guid.Empty
            };
            var controller = CreateProductsControllerBuilder()
                                   .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = await controller.Update(request) as BadRequestObjectResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public async Task Update_ShouldReturnBadRequestWithMessage_GivenAProductIdIsEmpty()
        {
            //---------------Set up test pack-------------------
            var request = new UpdateProductRequest
            {
                ProductId = Guid.Empty
            };
            var controller = CreateProductsControllerBuilder()
                                   .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = await controller.Update(request) as BadRequestObjectResult;
            //---------------Test Result -----------------------
            var response = result.Value as ErrorResponse;
            Assert.AreEqual(1, response.Errors.Count);
            Assert.AreEqual("The product does not exist, or the id is empty.", response.Errors[0].Message);
        }

        [Test]
        public async Task Update_ShouldReturnNotfound_GivenAProductHasNotBeenUpdated()
        {
            //---------------Set up test pack-------------------
            var request = new UpdateProductRequest
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "BBB123"
            };
            var product = ProductBuilder.BuildRandom();
            var productService = Substitute.For<IProductService>();
            await productService.CreateProductAsync(product);
            HttpContextSetup(out HttpContext httpContext, out ClaimsPrincipal claimsPrincipal);
            SetUserIdToProduct(product, claimsPrincipal);
            var controller = CreateProductsControllerBuilder()
                .WithProductService(productService)
                .WithMapper(_mapper).Build();
            //---------------Assert Precondition----------------           
            controller.Context = httpContext;
            //---------------Execute Test ----------------------
            var result = await controller.Update(request) as NotFoundResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        [Test]
        public async Task Update_ShouldReturnOkStatus_GivenAProductHasBeenUpdated()
        {
            //---------------Set up test pack-------------------
            var request = new UpdateProductRequest
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "BBB123"
            };

            var productService = Substitute.For<IProductService>();
            HttpContextSetup(out HttpContext httpContext, out ClaimsPrincipal claimsPrincipal);
            productService.UpdateProductAsync(Arg.Any<Product>()).Returns(true);
            var controller = CreateProductsControllerBuilder()
                .WithProductService(productService)
                .WithMapper(_mapper).Build();
            //---------------Assert Precondition----------------           
            controller.Context = httpContext;
            //---------------Execute Test ----------------------
            var result = await controller.Update(request) as OkObjectResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Test]
        public void Delete_ShouldHaveHttpDeleteAttribute()
        {
            //---------------Set up test pack-------------------
            var methodInfo = typeof(ProductsController)
                .GetMethod("Delete");
            //---------------Assert Precondition----------------
            Assert.IsNotNull(methodInfo);
            //---------------Execute Test ----------------------
            var httpPostAttribute = methodInfo.GetCustomAttribute<HttpDeleteAttribute>();
            //---------------Test Result -----------------------
            Assert.NotNull(httpPostAttribute);
        }

        [Test]
        public async Task Delete_ShouldReturnNoContent_GivenAProductIdIsEmpty()
        {
            //---------------Set up test pack-------------------
            var controller = CreateProductsControllerBuilder()
                                   .Build();
            //---------------Assert Precondition----------------            
            //---------------Execute Test ----------------------
            var result = await controller.Delete(Guid.Empty) as NoContentResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }

        [Test]
        public async Task Delete_ShouldReturnNotFound_GivenAProductHasNotBeenDeleted()
        {
            //---------------Set up test pack-------------------
            var product = ProductBuilder.BuildRandom();
            var productService = Substitute.For<IProductService>();
            productService.GetProductByIdAsync(product.ProductId).Returns(product);
            var controller = CreateProductsControllerBuilder()
                                   .WithProductService(productService)
                                   .Build();
            //---------------Assert Precondition----------------            
            //---------------Execute Test ----------------------
            var result = await controller.Delete(product.ProductId) as NotFoundResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        [Test]
        public async Task Delete_ShouldReturnNoContent_GivenAProductHasBeenDeleted()
        {
            //---------------Set up test pack-------------------
            var product = ProductBuilder.BuildRandom();
            var productService = Substitute.For<IProductService>();
            productService.DeleteProductAsync(product.ProductId).Returns(true);
            var controller = CreateProductsControllerBuilder()
                                   .WithProductService(productService)
                                   .Build();
            //---------------Assert Precondition----------------            
            //---------------Execute Test ----------------------
            var result = await controller.Delete(product.ProductId) as NoContentResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }

        [Test]
        public void HttpContextProvider_GivenSetHttpContextProvider_ShouldHttpContextProviderOnFirstCall()
        {
            //---------------Set up test pack-------------------
            var controller = CreateProductsControllerBuilder().Build();
            var httpContext = Substitute.For<HttpContext>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            controller.Context = httpContext;
            //---------------Test Result -----------------------
            Assert.AreSame(httpContext, controller.Context);
        }

        [Test]
        public void HttpContextProvider_GivenSetHttpContextProviderIsSet_ShouldThrowOnCall()
        {
            //---------------Set up test pack-------------------
            var controller = CreateProductsControllerBuilder().Build();
            var httpContextProvider = Substitute.For<HttpContext>();
            var httpContextProvider1 = Substitute.For<HttpContext>();
            controller.Context = httpContextProvider;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var ex = Assert.Throws<InvalidOperationException>(() => controller.Context = httpContextProvider1);
            //---------------Test Result -----------------------
            Assert.AreEqual("HttpContext is already set", ex.Message);
        }

        private static PaginationQuery CreatePaginationQuery()
        {
            return new PaginationQuery();
        }
        private static Uri CreateUri()
        {
            return new Uri("localhost:4000?pageNumber=1&pageSize=10");
        }
        private static List<Product> CreateProducts(Product product)
        {
            return new List<Product>
            {
                product
            };
        }
        private static void SetUserIdToProduct(Product product, ClaimsPrincipal claimsPrincipal)
        {
            product.UserId = Guid.Parse(claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
        private static ClaimsPrincipal GetLoggedInUser()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "username"),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim("name", "John Doe"),
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            return claimsPrincipal;
        }
        private static void HttpContextSetup(out HttpContext httpContext, out ClaimsPrincipal claimsPrincipal)
        {
            httpContext = Substitute.For<HttpContext>();
            claimsPrincipal = GetLoggedInUser();
            httpContext.User = claimsPrincipal;
        }
        private static ProductsControllerBuilder CreateProductsControllerBuilder()
        {
            return new ProductsControllerBuilder();
        }
    }
}
