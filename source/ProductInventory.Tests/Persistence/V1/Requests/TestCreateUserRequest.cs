using System;
using NUnit.Framework;
using PeanutButter.TestUtils.Generic;
using ProductsInventory.Persistence.V1.Requests;

namespace ProductInventory.Tests.Persistence.V1.Requests
{
    [TestFixture]
    public class TestCreateUserRequest
    {
        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new CreateUserRequest());
            //---------------Test Result -----------------------
        }

        [TestCase("UserId", typeof(Guid))]
        [TestCase("Username", typeof(string))]        
        [TestCase("Email", typeof(string))]
        [TestCase("HashedPassword", typeof(string))]
        [TestCase("Password", typeof(string))]
        [TestCase("Salt", typeof(string))]
        [TestCase("IsLocked", typeof(bool))]
        [TestCase("CreatedBy", typeof(string))]
        [TestCase("LastUpdatedBy", typeof(string))]
        [TestCase("DateCreated", typeof(DateTime?))]
        [TestCase("DateLastModified", typeof(DateTime?))]
        public void CreateUserRequest_ShouldHaveProperty(string propertyName, Type propertyType)
        {
            //---------------Set up test pack-------------------
            var sut = typeof(CreateUserRequest);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            sut.ShouldHaveProperty(propertyName, propertyType);
            //---------------Test Result -----------------------
        }
    }
}
