using NUnit.Framework;
using ProductsInventory.Persistence.Services;
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

    }
}
