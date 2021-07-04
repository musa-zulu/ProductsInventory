using NSubstitute;
using NUnit.Framework;
using ProductInventory.Tests.Common.Builders.Domain;
using ProductInventory.Tests.Common.Helpers;
using ProductsInventory.DB;
using ProductsInventory.DB.Domain;
using ProductsInventory.Persistence.Services;
using System;
using System.Threading.Tasks;

namespace ProductInventory.Tests.Persistence.Services
{
    [TestFixture]
    public class TestProductService
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
                new ProductService(Substitute.For<IApplicationDbContext>()));
            //---------------Test Result -----------------------
        }

        [Test]
        public async Task GetProductsAsync_GivenNoProductExist_ShouldReturnEmptyList()
        {
            //---------------Set up test pack-------------------            
            var productService = CreateProductService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await productService.GetProductsAsync();
            //---------------Test Result -----------------------
            Assert.IsEmpty(results);
            Assert.AreEqual(0, results.Count);
        }
        
        [Test]
        public async Task GetProductsAsync_GivenOneProductExist_ShouldReturnListWithThatProduct()
        {
            //---------------Set up test pack-------------------
            var product = CreateRandomProduct(Guid.NewGuid());
            await _db.Add(product);

            var productService = CreateProductService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await productService.GetProductsAsync();
            //---------------Test Result -----------------------
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(product.Name, results[0].Name);
            Assert.AreEqual(product.ProductCode, results[0].ProductCode);
        }

        [Test]
        public async Task GetProductsAsync_GivenTwoProductsExist_ShouldReturnAListWithTwoProducts()
        {
            //---------------Set up test pack-------------------
            var firstProduct = CreateRandomProduct(Guid.NewGuid());
            var secondProduct = CreateRandomProduct(Guid.NewGuid());

            await _db.Add(firstProduct, secondProduct);

            var productService = CreateProductService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await productService.GetProductsAsync();
            //---------------Test Result -----------------------
            Assert.IsNotNull(results);
            Assert.AreEqual(2, results.Count);
        }

        [Test]
        public async Task GetProductsAsync_GivenManyProductsExist_ShouldReturnAListWithAllProducts()
        {
            //---------------Set up test pack-------------------
            var firstProduct = CreateRandomProduct(Guid.NewGuid());
            var secondProduct = CreateRandomProduct(Guid.NewGuid());
            var thirdProduct = CreateRandomProduct(Guid.NewGuid());
            var forthProduct = CreateRandomProduct(Guid.NewGuid());

            await _db.Add(firstProduct, secondProduct, thirdProduct, forthProduct);

            var productService = CreateProductService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await productService.GetProductsAsync();
            //---------------Test Result -----------------------
            Assert.IsNotNull(results);
            Assert.AreEqual(4, results.Count);
        }

        [Test]
        public async Task CreateProductAsync_GivenAProductExistOnDb_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------            
            var product = CreateRandomProduct(Guid.NewGuid());
            await _db.Add(product);

            var productService = CreateProductService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await productService.CreateProductAsync(product);
            //---------------Test Result -----------------------
            Assert.IsFalse(results);
        }

        [Test]
        public async Task CreateProductAsync_GivenAProduct_ShouldAddProductToRepo()
        {
            //---------------Set up test pack-------------------
            var product = CreateRandomProduct(Guid.NewGuid());      ;

            var productService = CreateProductService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await productService.CreateProductAsync(product);
            //---------------Test Result -----------------------
            var productFromRepo = await productService.GetProductByIdAsync(product.ProductId);
            Assert.IsTrue(results);
            Assert.AreEqual(productFromRepo.ProductId, product.ProductId);
            Assert.AreEqual(productFromRepo.Name, product.Name);
            Assert.AreEqual(productFromRepo.ProductCode, product.ProductCode);
        }

        [Test]
        public async Task CreateProductAsync_GivenProductHasBeenSavedSuccessfully_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var product = CreateRandomProduct(Guid.NewGuid());
            var productService = CreateProductService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await productService.CreateProductAsync(product);
            //---------------Test Result -----------------------            
            Assert.IsTrue(results);
        }

        [Test]
        public async Task GetProductByIdAsync_GivenNoProductExist_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------  
            var productService = CreateProductService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await productService.GetProductByIdAsync(Guid.NewGuid());
            //---------------Test Result -----------------------
            Assert.IsNull(results);
        }

        [Test]
        public async Task GetProductByIdAsync_GivenProductExistInRepo_ShouldReturnThatProduct()
        {
            //---------------Set up test pack-------------------
            var product = CreateRandomProduct(Guid.NewGuid());
            var productService = CreateProductService();
            await _db.Add(product);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await productService.GetProductByIdAsync(product.ProductId);
            //---------------Test Result -----------------------         
            Assert.AreEqual(results.Name, product.Name);
            Assert.AreEqual(results.ProductId, product.ProductId);
            Assert.AreEqual(results.ProductCode, product.ProductCode);
        }

        [Test]
        public async Task DeleteProductAsync_GivenNoProductExist_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------            
            var productService = CreateProductService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await productService.DeleteProductAsync(Guid.Empty);
            //---------------Test Result -----------------------            
            Assert.IsFalse(results);
        }

        [Test]
        public async Task UpdateProductAsync_GivenProductExistInRepo_ShouldUpdateThatProduct()
        {
            //---------------Set up test pack-------------------
            var product = CreateRandomProduct(Guid.NewGuid());
            await _db.Add(product);
            var productService = CreateProductService();
            //---------------Assert Precondition----------------
            product.Name = "This has been updated";
            //---------------Execute Test ----------------------
            var results = await productService.UpdateProductAsync(product);
            //---------------Test Result -----------------------         
            var productFromRepo = await productService.GetProductByIdAsync(product.ProductId);
            Assert.AreEqual(productFromRepo.Name, "This has been updated");
        }

        [Test]
        public async Task UpdateProductAsync_GivenProductHasBeenUpdatedSuccessfully_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var product = CreateRandomProduct(Guid.NewGuid());
            await _db.Add(product);
            var productService = CreateProductService();
            //---------------Assert Precondition----------------
            product.Name = "This has been updated";
            //---------------Execute Test ----------------------
            var results = await productService.UpdateProductAsync(product);
            //---------------Test Result -----------------------                     
            Assert.IsTrue(results);
        }
        private ProductService CreateProductService()
        {
            return new ProductService(_db.DbContext);
        }

        private static Product CreateRandomProduct(Guid id)
        {
            var product = new ProductBuilder().WithId(id).WithRandomProps().Build();
            return product;
        }

        public void Dispose()
        {
            _db.DbContext.Database.EnsureDeleted();
            _db.DbContext.Dispose();
        }
    }
}
