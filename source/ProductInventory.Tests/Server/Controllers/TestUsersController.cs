using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using ProductInventory.Tests.Common.Builders.Controllers;
using ProductInventory.Tests.Common.Builders.Domain;
using ProductsInventory.DB.Domain;
using ProductsInventory.Persistence.Helpers;
using ProductsInventory.Persistence.Interfaces.Services;
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
    public class TestUsersController
    {
        private static IMapper _mapper;

        public TestUsersController()
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
                new UsersController(Substitute.For<IUserService>(), Substitute.For<IMapper>(), Substitute.For<IUriService>()));
            //---------------Test Result -----------------------
        }

        [Test]
        public void Construct_GivenIUserServiceIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() =>
                    new UsersController(null, Substitute.For<IMapper>(), Substitute.For<IUriService>()));
            //---------------Test Result -----------------------
            Assert.AreEqual("userService", ex.ParamName);
        }

        [Test]
        public void Construct_GivenIMapperIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() =>
                    new UsersController(Substitute.For<IUserService>(), null, Substitute.For<IUriService>()));
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
                    new UsersController(Substitute.For<IUserService>(), Substitute.For<IMapper>(), null));
            //---------------Test Result -----------------------
            Assert.AreEqual("uriService", ex.ParamName);
        }

        [Test]
        public void DateTimeProvider_GivenSetDateTimeProvider_ShouldSetDateTimeProviderOnFirstCall()
        {
            //---------------Set up test pack-------------------
            var controller = CreateUsersControllerBuilder().Build();
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
            var controller = CreateUsersControllerBuilder().Build();
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
            var methodInfo = typeof(UsersController)
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
            var controller = CreateUsersControllerBuilder()
                .WithMapper(mappingEngine)
                .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = await controller.GetAll(paginationQuery);
            //---------------Test Result -----------------------
            mappingEngine.Received(1).Map<PaginationFilter>(paginationQuery);
        }

        [Test]
        public async Task GetAll_ShouldReturnOkResultObject_WhenUserExist()
        {
            //---------------Set up test pack-------------------
            var user = UserBuilder.BuildRandom();
            List<User> users = CreateUsers(user);
            var uriService = Substitute.For<IUriService>();
            var userService = Substitute.For<IUserService>();
            var paginationQuery = CreatePaginationQuery();
            Uri uri = CreateUri();

            uriService.GetAllUri(Arg.Any<PaginationQuery>()).Returns(uri);
            userService.GetUsersAsync(Arg.Any<PaginationFilter>()).Returns(users);
            var controller = CreateUsersControllerBuilder()
                                   .WithUserService(userService)
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
        public async Task GetAll_ShouldReturnCountOfOne_WhenUserExist()
        {
            //---------------Set up test pack-------------------
            var user = UserBuilder.BuildRandom();
            List<User> users = CreateUsers(user);
            var userService = Substitute.For<IUserService>();
            var paginationQuery = CreatePaginationQuery();
            var uriService = Substitute.For<IUriService>();
            Uri uri = CreateUri();

            uriService.GetAllUri(Arg.Any<PaginationQuery>()).Returns(uri);
            userService.GetUsersAsync(Arg.Any<PaginationFilter>()).Returns(users);

            var controller = CreateUsersControllerBuilder()
                                   .WithUserService(userService)
                                   .WithMapper(_mapper)
                                   .WithUriService(uriService)
                                   .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = await controller.GetAll(paginationQuery) as OkObjectResult;
            //---------------Test Result -----------------------
            var pagedResponse = result.Value as PagedResponse<UserResponse>;
            Assert.IsNotNull(pagedResponse);
            Assert.AreEqual(1, pagedResponse.Data.Count());
        }

        [Test]
        public async Task GetAll_ShouldReturnUser_WhenUserExist()
        {
            //---------------Set up test pack-------------------
            var user = UserBuilder.BuildRandom();
            List<User> users = CreateUsers(user);
            var userService = Substitute.For<IUserService>();
            var paginationQuery = CreatePaginationQuery();
            var uriService = Substitute.For<IUriService>();
            Uri uri = CreateUri();

            uriService.GetAllUri(Arg.Any<PaginationQuery>()).Returns(uri);
            userService.GetUsersAsync(Arg.Any<PaginationFilter>()).Returns(users);

            var controller = CreateUsersControllerBuilder()
                                   .WithUserService(userService)
                                   .WithMapper(_mapper)
                                   .WithUriService(uriService)
                                   .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = await controller.GetAll(paginationQuery) as OkObjectResult;
            //---------------Test Result -----------------------
            var pagedResponse = result.Value as PagedResponse<UserResponse>;
            Assert.IsNotNull(pagedResponse);
            Assert.AreEqual(user.Username, pagedResponse.Data.FirstOrDefault().UserName);
        }

        [Test]
        public void Get_ShouldHaveHttpGetAttribute()
        {
            //---------------Set up test pack-------------------
            var methodInfo = typeof(UsersController)
                .GetMethod("Get");
            //---------------Assert Precondition----------------
            Assert.IsNotNull(methodInfo);
            //---------------Execute Test ----------------------
            var httpPostAttribute = methodInfo.GetCustomAttribute<HttpGetAttribute>();
            //---------------Test Result -----------------------
            Assert.NotNull(httpPostAttribute);
        }

        [Test]
        public async Task Get_ShouldReturnOkResultObject_WhenUserExist()
        {
            //---------------Set up test pack-------------------
            var user = UserBuilder.BuildRandom();
            var userId = user.UserId;
            var userService = Substitute.For<IUserService>();

            userService.GetUserByUserIdAsync(userId).Returns(user);

            var controller = CreateUsersControllerBuilder()
                                   .WithUserService(userService)
                                   .WithMapper(_mapper)
                                   .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = await controller.Get(userId) as OkObjectResult;
            //---------------Test Result -----------------------            
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Get_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            //---------------Set up test pack-------------------
            var controller = CreateUsersControllerBuilder().Build();
            //---------------Assert Precondition----------------
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
            var user = UserBuilder.BuildRandom();
            var userId = user.UserId;
            var userService = Substitute.For<IUserService>();
            var mappingEngine = Substitute.For<IMapper>();

            userService.GetUserByUserIdAsync(userId).Returns(user);

            var controller = CreateUsersControllerBuilder()
                .WithMapper(mappingEngine)
                .WithUserService(userService)
                .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = await controller.Get(userId);
            //---------------Test Result -----------------------
            mappingEngine.Received(1).Map<UserResponse>(user);
        }

        [Test]
        public async Task Get_ShouldReturnUser_WhenUserExist()
        {
            //---------------Set up test pack-------------------
            var user = UserBuilder.BuildRandom();
            var userId = user.UserId;
            var userService = Substitute.For<IUserService>();

            userService.GetUserByUserIdAsync(userId).Returns(user);
            
            var controller = CreateUsersControllerBuilder()
                .WithMapper(_mapper)
                .WithUserService(userService)
                .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = await controller.Get(userId) as OkObjectResult;
            //---------------Test Result -----------------------
            var pagedResponse = result.Value as Response<UserResponse>;
            Assert.IsNotNull(pagedResponse);
            Assert.AreEqual(user.UserId.ToString(), pagedResponse.Data.UserId);
            Assert.AreEqual(user.Username, pagedResponse.Data.UserName);
        }

        private static PaginationQuery CreatePaginationQuery()
        {
            return new PaginationQuery();
        }
        private static Uri CreateUri()
        {
            return new Uri("localhost:4000?pageNumber=1&pageSize=10");
        }
        private static List<User> CreateUsers(User user)
        {
            return new List<User>
            {
                user
            };
        }
        private static UsersControllerBuilder CreateUsersControllerBuilder()
        {
            return new UsersControllerBuilder();
        }
    }
}
