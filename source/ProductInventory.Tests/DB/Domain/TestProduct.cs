using NUnit.Framework;
using PeanutButter.TestUtils.Generic;
using ProductsInventory.DB.Domain;
using System;

namespace ProductInventory.Tests.DB.Domain
{
    [TestFixture]
    public class TestProduct
    {
        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new Product());
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
        [TestCase("Category", typeof(Category))]
        [TestCase("LastUpdatedBy", typeof(string))]
        [TestCase("DateCreated", typeof(DateTime?))]
        [TestCase("DateLastModified", typeof(DateTime?))]
        public void Product_ShouldHaveProperty(string propertyName, Type propertyType)
        {
            //---------------Set up test pack-------------------
            var sut = typeof(Product);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            sut.ShouldHaveProperty(propertyName, propertyType);
            //---------------Test Result -----------------------
        }
    }
}
