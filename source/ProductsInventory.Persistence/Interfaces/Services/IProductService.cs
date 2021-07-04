using ProductsInventory.DB.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductsInventory.Persistence.Interfaces.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsync(PaginationFilter paginationFilter = null);
        Task<bool> CreateProductAsync(Product product);
        Task<Product> GetProductByIdAsync(Guid productId);
        Task<bool> UpdateProductAsync(Product productToUpdate);
        Task<bool> DeleteProductAsync(Guid productId);
    }
}
