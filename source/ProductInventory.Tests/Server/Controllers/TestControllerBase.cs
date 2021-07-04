using AutoMapper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NUnit.Framework;
using ProductsInventory.Persistence.Helpers;
using ProductsInventory.Persistence.Interfaces.Services;
using ProductsInventory.Persistence.V1.Requests;
using ProductsInventory.Persistence.V1.Responses;
using ProductsInventory.Server.Controllers.V1;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;

namespace ProductInventory.Tests.Server.Controllers
{
    [TestFixture]
    public class TestControllerBase
    {
        [Test]
        public void SetDefaultFieldsFor_ShouldSetDefaultFields()
        {
            //---------------Set up test pack-------------------
            ControllerBase controller = CreateBaseController();
            var categoryRequest = new CreateCategoryRequest();
            HttpContextSetup(out HttpContext httpContext, out ClaimsPrincipal claimsPrincipal);
            var userId = Guid.Parse(claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            categoryRequest.UserId = userId;
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            controller.DateTimeProvider = dateTimeProvider;
            controller.Context = httpContext;

            //---------------Assert Precondition----------------            
            //---------------Execute Test ----------------------
            controller.SetDefaultFieldsFor(categoryRequest);
            //---------------Test Result -----------------------
            Assert.AreEqual(userId, categoryRequest.UserId);
            Assert.AreEqual(dateTimeProvider.Now, categoryRequest.DateCreated);
            Assert.AreEqual(dateTimeProvider.Now, categoryRequest.DateLastModified);
        }

        [Test]
        public void UpdateBaseFieldsOn_ShouldSetDefaultFields()
        {
            //---------------Set up test pack-------------------
            ControllerBase controller = CreateBaseController();
            var categoryRequest = new UpdateCategoryRequest();
            HttpContextSetup(out HttpContext httpContext, out ClaimsPrincipal claimsPrincipal);
            var userId = Guid.Parse(claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            categoryRequest.UserId = userId;
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            controller.DateTimeProvider = dateTimeProvider;
            controller.Context = httpContext;

            //---------------Assert Precondition----------------            
            //---------------Execute Test ----------------------
            controller.UpdateBaseFieldsOn(categoryRequest);
            //---------------Test Result -----------------------
            Assert.AreEqual(userId, categoryRequest.UserId);
            Assert.AreEqual(httpContext.User.Identity.Name, categoryRequest.LastUpdatedBy);
            Assert.AreEqual(dateTimeProvider.Now, categoryRequest.DateLastModified);
        }

        [Test]
        public void GetLoggedInUserId_ShouldReturnUserId()
        {
            //---------------Set up test pack-------------------
            ControllerBase controller = CreateBaseController();
            HttpContextSetup(out HttpContext httpContext, out ClaimsPrincipal claimsPrincipal);
            var userId = Guid.Parse(claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value);            
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            controller.DateTimeProvider = dateTimeProvider;
            controller.Context = httpContext;

            //---------------Assert Precondition----------------            
            //---------------Execute Test ----------------------
            var results = controller.GetLoggedInUserId();
            //---------------Test Result -----------------------
            Assert.AreEqual(userId, results);
        }

        [Test]
        public void SetDefaultFieldsFor_CreateProductRequest_ShouldSetDefaultFields()
        {
            //---------------Set up test pack-------------------
            ControllerBase controller = CreateBaseController();
            var productRequest = new CreateProductRequest();
            HttpContextSetup(out HttpContext httpContext, out ClaimsPrincipal claimsPrincipal);
            var userId = Guid.Parse(claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            productRequest.UserId = userId;
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            controller.DateTimeProvider = dateTimeProvider;
            controller.Context = httpContext;

            //---------------Assert Precondition----------------            
            //---------------Execute Test ----------------------
            controller.SetDefaultFieldsFor(productRequest);
            //---------------Test Result -----------------------
            Assert.AreEqual(userId, productRequest.UserId);
            Assert.AreEqual(dateTimeProvider.Now, productRequest.DateCreated);
            Assert.AreEqual(dateTimeProvider.Now, productRequest.DateLastModified);
        }

        [Test]
        public void UpdateBaseFieldsOn_CreateProductRequest_ShouldSetDefaultFields()
        {
            //---------------Set up test pack-------------------
            ControllerBase controller = CreateBaseController();
            var productRequest = new UpdateProductRequest();
            HttpContextSetup(out HttpContext httpContext, out ClaimsPrincipal claimsPrincipal);
            var userId = Guid.Parse(claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            productRequest.UserId = userId;
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            controller.DateTimeProvider = dateTimeProvider;
            controller.Context = httpContext;

            //---------------Assert Precondition----------------            
            //---------------Execute Test ----------------------
            controller.UpdateBaseFieldsOn(productRequest);
            //---------------Test Result -----------------------
            Assert.AreEqual(userId, productRequest.UserId);
            Assert.AreEqual(httpContext.User.Identity.Name, productRequest.LastUpdatedBy);
            Assert.AreEqual(dateTimeProvider.Now, productRequest.DateLastModified);
        }

        [Test]
        public void InvalidRequest_ShouldReturnBadRequest()
        {
            //---------------Set up test pack-------------------
            ControllerBase controller = CreateBaseController();                                   
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.InvalidRequest() as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public void InvalidRequest_ShouldReturnBadRequestWithMessage()
        {
            //---------------Set up test pack-------------------
            ControllerBase controller = CreateBaseController();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.InvalidRequest() as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
            //---------------Test Result -----------------------
            var response = result.Value as ErrorResponse;
            Assert.AreEqual(1, response.Errors.Count);
            Assert.AreEqual("Please login...", response.Errors[0].Message);
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
        private static ControllerBase CreateBaseController()
        {
            return new ControllerBase(Substitute.For<IMapper>(), Substitute.For<IUriService>());
        }
    }
}
