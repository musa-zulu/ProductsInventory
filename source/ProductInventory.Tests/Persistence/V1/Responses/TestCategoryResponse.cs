using NUnit.Framework;
using PeanutButter.TestUtils.Generic;
using ProductsInventory.Persistence.V1.Responses;
using System;

namespace ProductInventory.Tests.Persistence.V1.Responses
{
    [TestFixture]
    public class TestCategoryResponse
    {
        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new CategoryResponse());
            //---------------Test Result -----------------------
        }

        [TestCase("CategoryId", typeof(Guid))]
        [TestCase("Name", typeof(string))]
        [TestCase("CategoryCode", typeof(string))]        
        [TestCase("IsActive", typeof(bool))]
        public void CategoryResponse_ShouldHaveProperty(string propertyName, Type propertyType)
        {
            //---------------Set up test pack-------------------
            var sut = typeof(CategoryResponse);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            sut.ShouldHaveProperty(propertyName, propertyType);
            //---------------Test Result -----------------------
        }
    }
}
