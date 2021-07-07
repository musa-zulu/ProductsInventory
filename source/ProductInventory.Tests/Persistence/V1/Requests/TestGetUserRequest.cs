using NUnit.Framework;
using PeanutButter.TestUtils.Generic;
using ProductsInventory.Persistence.V1.Requests;
using System;

namespace ProductInventory.Tests.Persistence.V1.Requests
{
    [TestFixture]
    public class TestGetUserRequest
    {
        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new GetUserRequest());
            //---------------Test Result -----------------------
        }
                
        [TestCase("Email", typeof(string))]        
        public void GetUserRequest_ShouldHaveProperty(string propertyName, Type propertyType)
        {
            //---------------Set up test pack-------------------
            var sut = typeof(GetUserRequest);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            sut.ShouldHaveProperty(propertyName, propertyType);
            //---------------Test Result -----------------------
        }
    }
}
