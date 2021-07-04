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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService, IMapper mapper, IUriService uriService)
            :base(mapper, uriService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }

        [HttpGet(ApiRoutes.Categories.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery paginationQuery)
        {
            Guid userId = GetLoggedInUserId();
            if (userId == Guid.Empty)
            {
                return InvalidRequest();
            }
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);

            var categories = await _categoryService.GetCategoriesAsync(pagination);

            var userCategories = categories?.Where(x => x.UserId == userId);
            var categoryResponse = _mapper.Map<List<CategoryResponse>>(userCategories);


            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<CategoryResponse>(categoryResponse));
            }

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, categoryResponse);
            return Ok(paginationResponse);
        }

        [HttpGet(ApiRoutes.Categories.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid categoryId)
        {
            Guid userId = GetLoggedInUserId();
            if (userId == Guid.Empty)
            {
                return InvalidRequest();
            }
            var category = await _categoryService.GetCategoryByIdAsync(categoryId);
            
            if (category == null || category.UserId != userId)
                return NotFound();

            var categoryResponse = _mapper.Map<CategoryResponse>(category);
            return Ok(new Response<CategoryResponse>(categoryResponse));
        }

        [HttpPost(ApiRoutes.Categories.Create)]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest categoryRequest)
        {
            SetDefaultFieldsFor(categoryRequest);
            if (categoryRequest.UserId == Guid.Empty)
            {
                return InvalidRequest();
            }

            var validCategoryCode = ValidateCategoryCode(categoryRequest.CategoryCode);
            if (!string.IsNullOrWhiteSpace(validCategoryCode))
            {
                return InvalidRequest(validCategoryCode);
            }
            var categories = await _categoryService.GetCategoriesAsync();
            var categoryWihCodeExist = categories?.FirstOrDefault(x => x.CategoryCode == categoryRequest.CategoryCode);
            if (categoryWihCodeExist != null)
            {
                string message = "Category Code already exist for " + categoryWihCodeExist.Name + " category";
                return InvalidRequest(message);
            }

            categoryRequest.CategoryCode = categoryRequest.CategoryCode.ToUpperInvariant();
            var category = _mapper.Map<CreateCategoryRequest, Category>(categoryRequest);            

            await _categoryService.CreateCategoryAsync(category);

            var locationUri = _uriService.GetCategoryUri(category.CategoryId.ToString());
            return Created(locationUri, new Response<CategoryResponse>(_mapper.Map<CategoryResponse>(category)));
        }
      

        [HttpPut(ApiRoutes.Categories.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateCategoryRequest request)
        {
            if (request.CategoryId == Guid.Empty)
            {
                return BadRequest(new ErrorResponse(new ErrorModel { Message = "The category does not exist, or the id is empty." }));
            }

            UpdateBaseFieldsOn(request);
            if (request.UserId == Guid.Empty)
            {
                return InvalidRequest();
            }

            var category = _mapper.Map<UpdateCategoryRequest, Category>(request);
            category.CategoryId = request.CategoryId;

            var isUpdated = await _categoryService.UpdateCategoryAsync(category);

            if (isUpdated)
                return Ok(new Response<CategoryResponse>(_mapper.Map<CategoryResponse>(category)));

            return NotFound();
        }

        [HttpDelete(ApiRoutes.Categories.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid categoryId)
        {
            if (categoryId == Guid.Empty)
                return NoContent();

            var deleted = await _categoryService.DeleteCategoryAsync(categoryId);

            if (deleted)
                return NoContent();

            return NotFound();
        }       
    }
}
