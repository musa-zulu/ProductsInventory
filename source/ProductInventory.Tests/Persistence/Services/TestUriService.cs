using NUnit.Framework;
using ProductsInventory.Persistence.Services;
using ProductsInventory.Persistence.V1.Requests.Queries;
using System;

namespace ProductInventory.Tests.Persistence.Services
{
    [TestFixture]
    public class TestUriService
    {
        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new UriService(Guid.NewGuid().ToString()));
            //---------------Test Result -----------------------
        }

        [Test]
        public void GetAllUri_GivenPaginationQueryIsNull_ShouldReturnBaseUri()
        {
            //---------------Set up test pack-------------------
            var baseUri = "localhost:4000";
            var uriService = new UriService(baseUri);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = uriService.GetAllUri();
            //---------------Test Result -----------------------
            Assert.AreEqual(baseUri, results.AbsoluteUri);
        }

        [Test]
        public void GetAllUri_GivenPaginationQueryIsNotNull_ShouldReturnModifiedUri()
        {
            //---------------Set up test pack-------------------
            var baseUri = "localhost:4000";
            var paginationQuery = new PaginationQuery
            {
                PageNumber = 1,
                PageSize = 10
            };
            var uriService = new UriService(baseUri);
            var modifiedUrl = "localhost:4000?pageNumber=1&pageSize=10";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = uriService.GetAllUri(paginationQuery);
            //---------------Test Result -----------------------            
            Assert.AreEqual(modifiedUrl, results.AbsoluteUri);
        }

        [Test]
        public void GetCategoryUri_GivenCategoryId_ShouldReturnModifiedUri()
        {
            //---------------Set up test pack-------------------
            var baseUri = "localhost:4000/";
            var categoryId = Guid.NewGuid().ToString();
            var uriService = new UriService(baseUri);
            var modifiedUrl = "localhost:4000/api/v1/categories/"+ categoryId;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = uriService.GetCategoryUri(categoryId);
            //---------------Test Result -----------------------            
            Assert.AreEqual(modifiedUrl, results.AbsoluteUri);
        }

        [Test]
        public void GetProductUri_GivenProductId_ShouldReturnModifiedUri()
        {
            //---------------Set up test pack-------------------
            var baseUri = "localhost:4000/";
            var productId = Guid.NewGuid().ToString();
            var uriService = new UriService(baseUri);
            var modifiedUrl = "localhost:4000/api/v1/products/" + productId;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = uriService.GetProductUri(productId);
            //---------------Test Result -----------------------            
            Assert.AreEqual(modifiedUrl, results.AbsoluteUri);
        }
    }
}
