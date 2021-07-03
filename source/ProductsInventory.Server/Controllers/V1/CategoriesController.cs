using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductsInventory.DB.Domain;
using ProductsInventory.Persistence.Helpers;
using ProductsInventory.Persistence.Interfaces.Services;
using ProductsInventory.Persistence.V1;
using ProductsInventory.Persistence.V1.Requests.Queries;
using ProductsInventory.Persistence.V1.Responses;
using ProductsInventory.Server.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductsInventory.Server.Controllers.V1
{
    public class CategoriesController : Controller
    {
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        private IDateTimeProvider _dateTimeProvider;

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

        [HttpGet(ApiRoutes.Categories.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);

            var categories = await _categoryService.GetCategoriesAsync(pagination);
            var categoryResponse = _mapper.Map<List<CategoryResponse>>(categories);

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<CategoryResponse>(categoryResponse));
            }

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, categoryResponse);
            return Ok(paginationResponse);
        }
    }
}
