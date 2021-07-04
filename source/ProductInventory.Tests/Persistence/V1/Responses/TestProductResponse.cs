using NUnit.Framework;
using PeanutButter.TestUtils.Generic;
using ProductsInventory.Persistence.V1.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductInventory.Tests.Persistence.V1.Responses
{
    [TestFixture]
    public class TestProductResponse
    {
        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new ProductResponse());
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
        public void ProductResponse_ShouldHaveProperty(string propertyName, Type propertyType)
        {
            //---------------Set up test pack-------------------
            var sut = typeof(ProductResponse);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            sut.ShouldHaveProperty(propertyName, propertyType);
            //---------------Test Result -----------------------
        }
    }
}
