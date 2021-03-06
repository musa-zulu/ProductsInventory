using System;
using NUnit.Framework;
using PeanutButter.TestUtils.Generic;
using ProductsInventory.Persistence.V1.Requests;

namespace ProductInventory.Tests.Persistence.V1.Requests
{
    [TestFixture]
    public class TestCreateCategoryRequest
    {
        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new CreateCategoryRequest());
            //---------------Test Result -----------------------
        }

        [TestCase("CategoryId", typeof(Guid))]
        [TestCase("Name", typeof(string))]        
        [TestCase("CategoryCode", typeof(string))]
        [TestCase("IsActive", typeof(bool))]
        [TestCase("UserId", typeof(Guid))]
        public void CreateCategoryRequest_ShouldHaveProperty(string propertyName, Type propertyType)
        {
            //---------------Set up test pack-------------------
            var sut = typeof(CreateCategoryRequest);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            sut.ShouldHaveProperty(propertyName, propertyType);
            //---------------Test Result -----------------------
        }
    }
}
