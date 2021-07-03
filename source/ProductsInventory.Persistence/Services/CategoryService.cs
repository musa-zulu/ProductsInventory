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
    public class CategoryService : ICategoryService
    {
        private readonly IApplicationDbContext _dataContext;

        public CategoryService(IApplicationDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Category>> GetCategoriesAsync(PaginationFilter paginationFilter = null)
        {
            var queryable = _dataContext.Categories.AsQueryable();

            if (paginationFilter == null)
            {
                return await queryable.ToListAsync();
            }

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await queryable.Skip(skip).Take(paginationFilter.PageSize).ToListAsync();
        }       
    }
}
