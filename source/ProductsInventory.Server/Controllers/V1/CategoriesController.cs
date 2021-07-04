using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsInventory.DB.Domain;
using ProductsInventory.Persistence.Helpers;
using ProductsInventory.Persistence.Interfaces.Services;
using ProductsInventory.Persistence.V1;
using ProductsInventory.Persistence.V1.Requests;
using ProductsInventory.Persistence.V1.Requests.Queries;
using ProductsInventory.Persistence.V1.Responses;
using ProductsInventory.Server.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProductsInventory.Server.Controllers.V1
{
    public class CategoriesController : Controller
    {
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        private IDateTimeProvider _dateTimeProvider;
        private HttpContext _httpContext;

        public CategoriesController(ICategoryService categoryService, IMapper mapper, IUriService uriService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _uriService = uriService ?? throw new ArgumentNullException(nameof(uriService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }

        public IDateTimeProvider DateTimeProvider
        {
            get { return _dateTimeProvider ??= new DefaultDateTimeProvider(); }
            set
            {
                if (_dateTimeProvider != null) throw new InvalidOperationException("DateTimeProvider is already set");
                _dateTimeProvider = value;
            }
        }

        public HttpContext Context
        {
            get { return _httpContext ??= HttpContext; }
            set
            {
                if (_httpContext != null) throw new InvalidOperationException("HttpContext is already set");
                _httpContext = value;
            }
        }

        [HttpGet(ApiRoutes.Categories.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery paginationQuery)
        {
            Guid userId = GetLoggedInUserId();
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

        private void SetDefaultFieldsFor(CreateCategoryRequest categoryRequest)
        {
            Guid userId = GetLoggedInUserId();
            categoryRequest.CategoryId = Guid.NewGuid();
            categoryRequest.DateCreated = DateTimeProvider.Now;
            categoryRequest.DateLastModified = DateTimeProvider.Now;
            categoryRequest.UserId = userId;
        }

        private void UpdateBaseFieldsOn(UpdateCategoryRequest request)
        {
            Guid userId = GetLoggedInUserId();
            request.DateLastModified = DateTimeProvider.Now;
            request.LastUpdatedBy = Context?.User.Identity.Name ?? "";
            request.UserId = userId;
        }
        private Guid GetLoggedInUserId()
        {
            return Guid.Parse(Context?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}
