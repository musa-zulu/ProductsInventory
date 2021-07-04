using Microsoft.AspNetCore.WebUtilities;
using ProductsInventory.Persistence.Interfaces.Services;
using ProductsInventory.Persistence.V1;
using ProductsInventory.Persistence.V1.Requests.Queries;
using System;

namespace ProductsInventory.Persistence.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;

        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetAllUri(PaginationQuery pagination = null)
        {
            var uri = new Uri(_baseUri);

            if (pagination == null)
            {
                return uri;
            }

            var modifiedUri = QueryHelpers.AddQueryString(_baseUri, "pageNumber", pagination.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", pagination.PageSize.ToString());

            return new Uri(modifiedUri);

        }

        public Uri GetCategoryUri(string categoryId)
        {
            return new Uri(_baseUri + ApiRoutes.Categories.Get.Replace("{categoryId}", categoryId));
        }

        public Uri GetProductUri(string productId)
        {
            return new Uri(_baseUri + ApiRoutes.Products.Get.Replace("{productId}", productId));
        }
    }
}
