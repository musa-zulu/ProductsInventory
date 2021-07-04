using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductsInventory.DB.Domain;
using ProductsInventory.Persistence.Interfaces.Services;
using ProductsInventory.Persistence.V1;
using ProductsInventory.Persistence.V1.Requests;
using ProductsInventory.Persistence.V1.Requests.Queries;
using ProductsInventory.Persistence.V1.Responses;
using ProductsInventory.Server.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsInventory.Server.Controllers.V1
{
    public class ProductsController : ControllerBase
    {

        private readonly IProductService _productService;

        public ProductsController(IProductService productService, IMapper mapper, IUriService uriService) : base(mapper, uriService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        [HttpGet(ApiRoutes.Products.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery paginationQuery)
        {
            Guid userId = GetLoggedInUserId();

            if (userId == Guid.Empty)
            {
                return InvalidRequest();
            }
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);

            var products = await _productService.GetProductsAsync(pagination);

            var userProducts = products?.Where(x => x.UserId == userId);
            var productResponse = _mapper.Map<List<ProductResponse>>(userProducts);


            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<ProductResponse>(productResponse));
            }

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, productResponse);
            return Ok(paginationResponse);
        }

        [HttpGet(ApiRoutes.Products.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid productId)
        {
            Guid userId = GetLoggedInUserId();
            if (userId == Guid.Empty)
            {
                return InvalidRequest();
            }
            var product = await _productService.GetProductByIdAsync(productId);

            if (product == null || product.UserId != userId)
                return NotFound();

            var productResponse = _mapper.Map<ProductResponse>(product);
            return Ok(new Response<ProductResponse>(productResponse));
        }

        [HttpPost(ApiRoutes.Products.Create)]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest productRequest)
        {
            SetDefaultFieldsFor(productRequest);
            if (productRequest.UserId == Guid.Empty)
            {
                return InvalidRequest();
            }
            var product = _mapper.Map<CreateProductRequest, Product>(productRequest);

            await _productService.CreateProductAsync(product);

            var locationUri = _uriService.GetProductUri(product.ProductId.ToString());
            return Created(locationUri, new Response<ProductResponse>(_mapper.Map<ProductResponse>(product)));
        }

        [HttpPut(ApiRoutes.Products.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateProductRequest request)
        {
            if (request.ProductId == Guid.Empty)
            {
                return InvalidRequest("The product does not exist, or the id is empty.");
            }

            UpdateBaseFieldsOn(request);
            if (request.UserId == Guid.Empty)
            {
                return InvalidRequest();
            }
            var product = _mapper.Map<UpdateProductRequest, Product>(request);
            product.ProductId = request.ProductId;

            var isUpdated = await _productService.UpdateProductAsync(product);

            if (isUpdated)
                return Ok(new Response<ProductResponse>(_mapper.Map<ProductResponse>(product)));

            return NotFound();
        }
        
        [HttpDelete(ApiRoutes.Products.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid productId)
        {
            if (productId == Guid.Empty)
                return NoContent();

            var deleted = await _productService.DeleteProductAsync(productId);

            if (deleted)
                return NoContent();

            return NotFound();
        }
    }
}
