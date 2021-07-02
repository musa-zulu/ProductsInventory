using NUnit.Framework;
using PeanutButter.TestUtils.Generic;
using ProductsInventory.DB.Domain;
using System;

namespace ProductInventory.Tests.DB.Domain
{
    [TestFixture]
    public class TestUser
    {
        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new User());
            //---------------Test Result -----------------------
        }

        [TestCase("UserId", typeof(Guid))]
        [TestCase("Username", typeof(string))]
        [TestCase("Email", typeof(string))]
        [TestCase("HashedPassword", typeof(string))]
        [TestCase("Salt", typeof(string))]
        [TestCase("IsLocked", typeof(bool))]
        [TestCase("CreatedBy", typeof(string))]
        [TestCase("LastUpdatedBy", typeof(string))]
        [TestCase("DateCreated", typeof(DateTime?))]
        [TestCase("DateLastModified", typeof(DateTime?))]
        public void User_ShouldHaveProperty(string propertyName, Type propertyType)
        {
            //---------------Set up test pack-------------------
            var sut = typeof(User);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            sut.ShouldHaveProperty(propertyName, propertyType);
            //---------------Test Result -----------------------
        }
    }
}
