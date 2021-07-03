using ProductsInventory.Persistence.V1.Requests.Queries;
using System;

namespace ProductsInventory.Persistence.Interfaces.Services
{
    public interface IUriService
    {
        Uri GetAllUri(PaginationQuery pagination = null);
        Uri GetCategoryUri(string categoryId);
    }
}
