using System;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using ProductInventory.Tests.Common.Builders.Domain;
using ProductInventory.Tests.Common.Helpers;
using ProductsInventory.DB;
using ProductsInventory.DB.Domain;
using ProductsInventory.Persistence.Services;

namespace ProductInventory.Tests.Persistence.Services
{
    [TestFixture]
    public class TestCategoriesService
    {
        private FakeDbContext _db;
        [SetUp]
        public void SetUp()
        {
            _db = new FakeDbContext(Guid.NewGuid().ToString());
            _db.DbContext.Database.EnsureCreated();
        }

        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() =>
                new CategoryService(Substitute.For<IApplicationDbContext>()));
            //---------------Test Result -----------------------
        }

        [Test]
        public async Task GetCategoriesAsync_GivenNoCategoryExist_ShouldReturnEmptyList()
        {
            //---------------Set up test pack-------------------            
            var categoryService = CreateCategoryService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await categoryService.GetCategoriesAsync();
            //---------------Test Result -----------------------
            Assert.IsEmpty(results);
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public async Task GetCategoriesAsync_GivenOneCategoryExist_ShouldReturnListWithThatCategory()
        {
            //---------------Set up test pack-------------------
            var category = CreateRandomCategory(Guid.NewGuid());
            await _db.Add(category);

            var categoryService = CreateCategoryService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await categoryService.GetCategoriesAsync();
            //---------------Test Result -----------------------
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(category.Name, results[0].Name);
            Assert.AreEqual(category.CategoryCode, results[0].CategoryCode);
        }

        [Test]
        public async Task GetCategoriesAsync_GivenTwoCategoriesExist_ShouldReturnAListWithTwoCategories()
        {
            //---------------Set up test pack-------------------
            var firstCategory = CreateRandomCategory(Guid.NewGuid());
            var secondCategory = CreateRandomCategory(Guid.NewGuid());

            await _db.Add(firstCategory, secondCategory);

            var categoryService = CreateCategoryService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await categoryService.GetCategoriesAsync();
            //---------------Test Result -----------------------
            Assert.IsNotNull(results);
            Assert.AreEqual(2, results.Count);
        }

        [Test]
        public async Task GetCategoriesAsync_GivenManyCategoriesExist_ShouldReturnAListWithAllCategories()
        {
            //---------------Set up test pack-------------------
            var firstCategory = CreateRandomCategory(Guid.NewGuid());
            var secondCategory = CreateRandomCategory(Guid.NewGuid());
            var thirdCategory = CreateRandomCategory(Guid.NewGuid());
            var forthCategory = CreateRandomCategory(Guid.NewGuid());

            await _db.Add(firstCategory, secondCategory, thirdCategory, forthCategory);

            var categoryService = CreateCategoryService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await categoryService.GetCategoriesAsync();
            //---------------Test Result -----------------------
            Assert.IsNotNull(results);
            Assert.AreEqual(4, results.Count);
        }

        [Test]
        public async Task CreateCategoryAsync_GivenACategoryExistOnDb_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------            
            var category = CreateRandomCategory(Guid.NewGuid());
            await _db.Add(category);

            var categoryService = CreateCategoryService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await categoryService.CreateCategoryAsync(category);
            //---------------Test Result -----------------------
            Assert.IsFalse(results);
        }

        [Test]
        public async Task CreateCategoryAsync_GivenACategory_ShouldAddCategoryToRepo()
        {
            //---------------Set up test pack-------------------
            var category = CreateRandomCategory(Guid.NewGuid());
            var categoryService = CreateCategoryService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await categoryService.CreateCategoryAsync(category);
            //---------------Test Result -----------------------
            var categoryFromRepo = await categoryService.GetCategoryByIdAsync(category.CategoryId);
            Assert.IsTrue(results);
            Assert.AreEqual(categoryFromRepo.CategoryId, category.CategoryId);
            Assert.AreEqual(categoryFromRepo.Name, category.Name);
            Assert.AreEqual(categoryFromRepo.CategoryCode, category.CategoryCode);
        }

        [Test]
        public async Task CreateCategoryAsync_GivenCategoryHasBeenSavedSuccessfully_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var category = CreateRandomCategory(Guid.NewGuid());
            var categoryService = CreateCategoryService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await categoryService.CreateCategoryAsync(category);
            //---------------Test Result -----------------------            
            Assert.IsTrue(results);
        }

        [Test]
        public async Task GetCategoryByIdAsync_GivenNoCategoryExist_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------  
            var categoryService = CreateCategoryService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await categoryService.GetCategoryByIdAsync(Guid.NewGuid());
            //---------------Test Result -----------------------
            Assert.IsNull(results);
        }

        [Test]
        public async Task GetCategoryByIdAsync_GivenCategoryExistInRepo_ShouldReturnThatCategory()
        {
            //---------------Set up test pack-------------------
            var category = CreateRandomCategory(Guid.NewGuid());
            var categoryService = CreateCategoryService();
            await _db.Add(category);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await categoryService.GetCategoryByIdAsync(category.CategoryId);
            //---------------Test Result -----------------------         
            Assert.AreEqual(results.Name, category.Name);
            Assert.AreEqual(results.CategoryId, category.CategoryId);
            Assert.AreEqual(results.CategoryCode, category.CategoryCode);
        }

        [Test]
        public async Task DeleteCategoryAsync_GivenNoCategoryExist_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------            
            var categoryService = CreateCategoryService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await categoryService.DeleteCategoryAsync(Guid.Empty);
            //---------------Test Result -----------------------            
            Assert.IsFalse(results);
        }

        [Test]
        public async Task UpdateCategoryAsync_GivenCategoryExistInRepo_ShouldUpdateThatCategory()
        {
            //---------------Set up test pack-------------------
            var category = CreateRandomCategory(Guid.NewGuid());
            await _db.Add(category);
            var categoryService = CreateCategoryService();
            //---------------Assert Precondition----------------
            category.Name = "This has been updated";
            //---------------Execute Test ----------------------
            var results = await categoryService.UpdateCategoryAsync(category);
            //---------------Test Result -----------------------         
            var categoryFromRepo = await categoryService.GetCategoryByIdAsync(category.CategoryId);
            Assert.AreEqual(categoryFromRepo.Name, "This has been updated");
        }

        [Test]
        public async Task UpdateCategoryAsync_GivenCategoryHasBeenUpdatedSuccessfully_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var category = CreateRandomCategory(Guid.NewGuid());
            await _db.Add(category);
            var categoryService = CreateCategoryService();
            //---------------Assert Precondition----------------
            category.Name = "This has been updated";
            //---------------Execute Test ----------------------
            var results = await categoryService.UpdateCategoryAsync(category);
            //---------------Test Result -----------------------                     
            Assert.IsTrue(results);
        }

        private CategoryService CreateCategoryService()
        {                      
            return new CategoryService(_db.DbContext);
        }

        private static Category CreateRandomCategory(Guid id)
        {
            var category = new CategoryBuilder().WithId(id).WithRandomProps().Build();
            return category;
        }

        public void Dispose()
        {
            _db.DbContext.Database.EnsureDeleted();
            _db.DbContext.Dispose();
        }
    }
}
