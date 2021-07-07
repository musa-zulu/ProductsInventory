using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using OfficeOpenXml;
using ProductsInventory.DB.Domain;
using ProductsInventory.Persistence.Interfaces.Services;
using ProductsInventory.Persistence.V1;
using ProductsInventory.Persistence.V1.Requests;
using ProductsInventory.Persistence.V1.Requests.Queries;
using ProductsInventory.Persistence.V1.Responses;
using ProductsInventory.Server.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
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
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);

            var products = await _productService.GetProductsAsync(pagination);

            var productResponse = _mapper.Map<List<ProductResponse>>(products);

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
            var product = await _productService.GetProductByIdAsync(productId);

            if (product == null)
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

            if(product.CategoryId == Guid.Empty)
            {
                return InvalidRequest("Please select category");
            }

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

        [HttpPost(ApiRoutes.Products.UploadImage), DisableRequestSizeLimit]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length <= 0) return BadRequest();
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.TrimEnd();
                var fullPath = Path.Combine(pathToSave, fileName.ToString());
                var dbPath = Path.Combine(folderName, fileName.ToString());
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                //SetDefaultFieldsFor(productRequest);
                return Ok(new { dbPath });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost(ApiRoutes.Products.DownloadExcel)]
        public IActionResult DownloadExcel()
        {
            try
            {
                var products = _productService.GetProductsAsync().Result;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var stream = new MemoryStream();
                using var package = new ExcelPackage(stream);
                var worksheet = package.Workbook.Worksheets.Add("Products");
                worksheet.Cells.LoadFromCollection(products, true);
                package.Save();
                stream.Position = 0;
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var fileName = "ProductsReport.xlsx";
                return File(stream, contentType, fileName);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
