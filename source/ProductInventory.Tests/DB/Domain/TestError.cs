using NUnit.Framework;
using PeanutButter.TestUtils.Generic;
using ProductsInventory.DB.Domain;
using System;

namespace ProductInventory.Tests.DB.Domain
{
    [TestFixture]
    public class TestError
    {
        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new Error());
            //---------------Test Result -----------------------
        }

        [TestCase("ErrorId", typeof(Guid))]
        [TestCase("Message", typeof(string))]
        [TestCase("StackTrace", typeof(string))]
        [TestCase("LastUpdatedBy", typeof(string))]
        [TestCase("DateCreated", typeof(DateTime?))]
        [TestCase("DateLastModified", typeof(DateTime?))]
        public void Error_ShouldHaveProperty(string propertyName, Type propertyType)
        {
            //---------------Set up test pack-------------------
            var sut = typeof(Error);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            sut.ShouldHaveProperty(propertyName, propertyType);
            //---------------Test Result -----------------------
        }
    }
}
