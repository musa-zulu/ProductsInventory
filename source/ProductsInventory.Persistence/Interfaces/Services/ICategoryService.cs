using ProductsInventory.DB.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductsInventory.Persistence.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategoriesAsync(PaginationFilter paginationFilter = null);
        Task<bool> CreateCategoryAsync(Category category);
        Task<Category> GetCategoryByIdAsync(Guid categoryId);
        Task<bool> UpdateCategoryAsync(Category categoryToUpdate);
        Task<bool> DeleteCategoryAsync(Guid categoryId);
    }
}
