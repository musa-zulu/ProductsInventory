using NUnit.Framework;
using PeanutButter.TestUtils.Generic;
using ProductsInventory.DB.Domain;
using System;

namespace ProductInventory.Tests.DB.Domain
{
    [TestFixture]
    public class TestEntityBase
    {
        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new EntityBase());
            //---------------Test Result -----------------------
        }

        [TestCase("CreatedBy", typeof(string))]
        [TestCase("LastUpdatedBy", typeof(string))]
        [TestCase("DateCreated", typeof(DateTime?))]
        [TestCase("DateLastModified", typeof(DateTime?))]
        public void EntityBase_ShouldHaveProperty(string propertyName, Type propertyType)
        {
            //---------------Set up test pack-------------------
            var sut = typeof(EntityBase);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            sut.ShouldHaveProperty(propertyName, propertyType);
            //---------------Test Result -----------------------
        }
    }
}
