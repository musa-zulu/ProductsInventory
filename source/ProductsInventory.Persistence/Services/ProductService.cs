using Microsoft.EntityFrameworkCore;
using ProductsInventory.DB;
using ProductsInventory.DB.Domain;
using ProductsInventory.Persistence.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsInventory.Persistence.Services
{
    public class ProductService : IProductService
    {
        private readonly IApplicationDbContext _dataContext;

        public ProductService(IApplicationDbContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<bool> CreateProductAsync(Product product)
        {
            var isSaved = false;
            try
            {
                _dataContext.Products.Add(product);
                isSaved = await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return isSaved;
            }
            return isSaved;
        }

        public async Task<bool> DeleteProductAsync(Guid productId)
        {
            var product = await GetProductByIdAsync(productId);

            if (product == null)
                return false;

            _dataContext.Products.Remove(product);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<List<Product>> GetProductsAsync(PaginationFilter paginationFilter = null)
        {
            var queryable = _dataContext.Products.AsQueryable();

            if (paginationFilter == null)
            {
                return await queryable.ToListAsync();
            }

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await queryable.Skip(skip).Take(paginationFilter.PageSize).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(Guid productId)
        {
            return await _dataContext.Products
                .Include(c => c.Category)
                .SingleOrDefaultAsync(x => x.ProductId == productId);
        }

        public async Task<bool> UpdateProductAsync(Product productToUpdate)
        {
            _dataContext.Products.Update(productToUpdate);
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}
