using NUnit.Framework;
using PeanutButter.TestUtils.Generic;
using ProductsInventory.Persistence.V1.Requests;
using System;

namespace ProductInventory.Tests.Persistence.V1.Requests
{
    [TestFixture]
    public class TestCreateProductRequest
    {
        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new CreateProductRequest());
            //---------------Test Result -----------------------
        }


        [TestCase("ProductId", typeof(Guid))]
        [TestCase("ProductCode", typeof(string))]
        [TestCase("Name", typeof(string))]
        [TestCase("Description", typeof(string))]
        [TestCase("Price", typeof(decimal))]
        [TestCase("ImagePath", typeof(string))]
        [TestCase("CategoryId", typeof(Guid))]
        [TestCase("UserId", typeof(Guid))]        
        [TestCase("LastUpdatedBy", typeof(string))]
        [TestCase("DateCreated", typeof(DateTime?))]
        [TestCase("DateLastModified", typeof(DateTime?))]
        [TestCase("UserName", typeof(string))]
        public void CreateProductRequest_ShouldHaveProperty(string propertyName, Type propertyType)
        {
            //---------------Set up test pack-------------------
            var sut = typeof(CreateProductRequest);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            sut.ShouldHaveProperty(propertyName, propertyType);
            //---------------Test Result -----------------------
        }
    }
}
