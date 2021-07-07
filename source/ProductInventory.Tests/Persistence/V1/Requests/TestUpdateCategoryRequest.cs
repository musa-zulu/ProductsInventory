using NUnit.Framework;
using PeanutButter.TestUtils.Generic;
using ProductsInventory.Persistence.V1.Requests;
using System;

namespace ProductInventory.Tests.Persistence.V1.Requests
{
    [TestFixture]
    public class TestUpdateCategoryRequest
    {
        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new UpdateCategoryRequest());
            //---------------Test Result -----------------------
        }


        [TestCase("CategoryId", typeof(Guid))]
        [TestCase("Name", typeof(string))]
        [TestCase("Name", typeof(string))]
        [TestCase("CategoryCode", typeof(string))]
        [TestCase("IsActive", typeof(bool))]        
        [TestCase("UserName", typeof(string))]        
        [TestCase("UserId", typeof(Guid))]
        [TestCase("LastUpdatedBy", typeof(string))]
        [TestCase("DateCreated", typeof(DateTime?))]
        [TestCase("DateLastModified", typeof(DateTime?))]
        public void UpdateCategoryRequest_ShouldHaveProperty(string propertyName, Type propertyType)
        {
            //---------------Set up test pack-------------------
            var sut = typeof(UpdateCategoryRequest);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            sut.ShouldHaveProperty(propertyName, propertyType);
            //---------------Test Result -----------------------
        }
    }
}
