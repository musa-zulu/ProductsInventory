using NUnit.Framework;
using PeanutButter.TestUtils.Generic;
using ProductsInventory.Persistence.V1.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductInventory.Tests.Persistence.V1.Requests
{
    [TestFixture]
    public class TestRequestBase
    {
        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new RequestBase());
            //---------------Test Result -----------------------
        }

        [TestCase("CreatedBy", typeof(string))]
        [TestCase("LastUpdatedBy", typeof(string))]
        [TestCase("DateCreated", typeof(DateTime?))]
        [TestCase("DateLastModified", typeof(DateTime?))]
        public void RequestBase_ShouldHaveProperty(string propertyName, Type propertyType)
        {
            //---------------Set up test pack-------------------
            var sut = typeof(RequestBase);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            sut.ShouldHaveProperty(propertyName, propertyType);
            //---------------Test Result -----------------------
        }
    }
}
