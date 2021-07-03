using AutoMapper;
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
using System.Threading.Tasks;

namespace ProductInventory.Tests.Server.Controllers
{
    [TestFixture]
    public class TestCategoriesController
    {
        private static IMapper _mapper;

        public TestCategoriesController()
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
                new CategoriesController(Substitute.For<ICategoryService>(), Substitute.For<IMapper>(), Substitute.For<IUriService>()));
            //---------------Test Result -----------------------
        }

        [Test]
        public void Construct_GivenICategoryServiceIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() =>
                    new CategoriesController(null, Substitute.For<IMapper>(), Substitute.For<IUriService>()));
            //---------------Test Result -----------------------
            Assert.AreEqual("categoryService", ex.ParamName);
        }

        [Test]
        public void Construct_GivenIMapperIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() =>
                    new CategoriesController(Substitute.For<ICategoryService>(), null, Substitute.For<IUriService>()));
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
                    new CategoriesController(Substitute.For<ICategoryService>(), Substitute.For<IMapper>(), null));
            //---------------Test Result -----------------------
            Assert.AreEqual("uriService", ex.ParamName);
        }

        [Test]
        public void DateTimeProvider_GivenSetDateTimeProvider_ShouldSetDateTimeProviderOnFirstCall()
        {
            //---------------Set up test pack-------------------
            var controller = CreateCategoriesControllerBuilder().Build();
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
            var controller = CreateCategoriesControllerBuilder().Build();
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
            var methodInfo = typeof(CategoriesController)
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
            var controller = CreateCategoriesControllerBuilder()
                .WithMapper(mappingEngine)
                .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = await controller.GetAll(paginationQuery);
            //---------------Test Result -----------------------
            mappingEngine.Received(1).Map<PaginationFilter>(paginationQuery);
        }

        [Test]
        public async Task GetAll_ShouldReturnOkResultObject_WhenCategoryExist()
        {
            //---------------Set up test pack-------------------
            var category = CategoryBuilder.BuildRandom();
            List<Category> categories = CreateCategories(category);
            var uriService = Substitute.For<IUriService>();
            var categoryService = Substitute.For<ICategoryService>();
            var paginationQuery = CreatePaginationQuery();
            Uri uri = CreateUri();

            uriService.GetAllUri(Arg.Any<PaginationQuery>()).Returns(uri);
            categoryService.GetCategoriesAsync(Arg.Any<PaginationFilter>()).Returns(categories);
            var controller = CreateCategoriesControllerBuilder()
                                   .WithCategoryService(categoryService)
                                   .WithMapper(_mapper)
                                   .WithUriService(uriService)
                                   .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = await controller.GetAll(paginationQuery) as OkObjectResult;
            //---------------Test Result -----------------------            
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetAll_ShouldReturnCountOfOne_WhenCategoryExist()
        {
            //---------------Set up test pack-------------------
            var category = CategoryBuilder.BuildRandom();
            List<Category> categories = CreateCategories(category);
            var categoryService = Substitute.For<ICategoryService>();
            var paginationQuery = CreatePaginationQuery();
            var uriService = Substitute.For<IUriService>();
            Uri uri = CreateUri();

            uriService.GetAllUri(Arg.Any<PaginationQuery>()).Returns(uri);
            categoryService.GetCategoriesAsync(Arg.Any<PaginationFilter>()).Returns(categories);

            var controller = CreateCategoriesControllerBuilder()
                                   .WithCategoryService(categoryService)
                                   .WithMapper(_mapper)
                                   .WithUriService(uriService)
                                   .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = await controller.GetAll(paginationQuery) as OkObjectResult;
            //---------------Test Result -----------------------
            var pagedResponse = result.Value as PagedResponse<CategoryResponse>;
            Assert.IsNotNull(pagedResponse);
            Assert.AreEqual(1, pagedResponse.Data.Count());
        }

        [Test]
        public async Task GetAll_ShouldReturnCategory_WhenCategoryExist()
        {
            //---------------Set up test pack-------------------
            var category = CategoryBuilder.BuildRandom();
            List<Category> categories = CreateCategories(category);
            var categoryService = Substitute.For<ICategoryService>();
            var paginationQuery = CreatePaginationQuery();
            var uriService = Substitute.For<IUriService>();
            Uri uri = CreateUri();

            uriService.GetAllUri(Arg.Any<PaginationQuery>()).Returns(uri);
            categoryService.GetCategoriesAsync(Arg.Any<PaginationFilter>()).Returns(categories);

            var controller = CreateCategoriesControllerBuilder()
                                   .WithCategoryService(categoryService)
                                   .WithMapper(_mapper)
                                   .WithUriService(uriService)
                                   .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = await controller.GetAll(paginationQuery) as OkObjectResult;
            //---------------Test Result -----------------------
            var pagedResponse = result.Value as PagedResponse<CategoryResponse>;
            Assert.IsNotNull(pagedResponse);
            Assert.AreEqual(category.Name, pagedResponse.Data.FirstOrDefault().Name);
        }    

        private static PaginationQuery CreatePaginationQuery()
        {
            return new PaginationQuery();
        }
        private static Uri CreateUri()
        {
            return new Uri("localhost:4000?pageNumber=1&pageSize=10");
        }
        private static List<Category> CreateCategories(Category category)
        {
            return new List<Category>
            {
                category
            };
        }
        private static CategoriesControllerBuilder CreateCategoriesControllerBuilder()
        {
            return new CategoriesControllerBuilder();
        }
    }
}
