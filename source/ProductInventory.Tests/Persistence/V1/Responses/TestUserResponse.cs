using NUnit.Framework;
using PeanutButter.TestUtils.Generic;
using ProductsInventory.Persistence.V1.Responses;
using System;

namespace ProductInventory.Tests.Persistence.V1.Responses
{
    [TestFixture]
    public class TestUserResponse
    {
        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new UserResponse());
            //---------------Test Result -----------------------
        }

        [TestCase("UserId", typeof(Guid))]
        [TestCase("UserName", typeof(string))]
        [TestCase("Email", typeof(string))]        
        [TestCase("Password", typeof(string))]
        public void UserResponse_ShouldHaveProperty(string propertyName, Type propertyType)
        {
            //---------------Set up test pack-------------------
            var sut = typeof(UserResponse);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            sut.ShouldHaveProperty(propertyName, propertyType);
            //---------------Test Result -----------------------
        }
    }
}
