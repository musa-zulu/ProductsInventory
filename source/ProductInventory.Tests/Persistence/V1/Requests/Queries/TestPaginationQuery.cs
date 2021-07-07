using NUnit.Framework;
using PeanutButter.TestUtils.Generic;
using ProductsInventory.Persistence.V1.Requests.Queries;
using System;

namespace ProductInventory.Tests.Persistence.V1.Requests.Queries
{
    [TestFixture]
    public class TestPaginationQuery
    {
        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new PaginationQuery());
            //---------------Test Result -----------------------
        }

        [Test]
        public void Constract_ShouldReturnDefaultValues()
        {
            //---------------Set up test pack-------------------
            int pageNumber = 1;
            int pageSize = 1000;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------            
            var paginationQuery = new PaginationQuery();
            //---------------Test Result -----------------------
            Assert.AreEqual(pageNumber, paginationQuery.PageNumber);
            Assert.AreEqual(pageSize, paginationQuery.PageSize);
        }

        [TestCase(1, 10)]
        [TestCase(2, 11)]
        [TestCase(3, 12)]
        [TestCase(4, 13)]
        public void Constract_ShouldReturnValuesSuplied(int pageNumber, int pageSize)
        {
            //---------------Set up test pack-------------------            
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------            
            var paginationQuery = new PaginationQuery(pageNumber, pageSize);
            //---------------Test Result -----------------------
            Assert.AreEqual(pageNumber, paginationQuery.PageNumber);
            Assert.AreEqual(pageSize, paginationQuery.PageSize);
        }

        [TestCase("PageNumber", typeof(int))]
        [TestCase("PageSize", typeof(int))]
        public void PaginationQuery_ShouldHaveProperty(string propertyName, Type propertyType)
        {
            //---------------Set up test pack-------------------
            var sut = typeof(PaginationQuery);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            sut.ShouldHaveProperty(propertyName, propertyType);
            //---------------Test Result -----------------------
        }
    }
}
