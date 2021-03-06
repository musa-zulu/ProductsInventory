using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsInventory.Persistence.Helpers;
using ProductsInventory.Persistence.Interfaces.Services;
using ProductsInventory.Persistence.V1.Requests;
using ProductsInventory.Persistence.V1.Responses;
using System;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace ProductsInventory.Server.Controllers.V1
{
    public class ControllerBase : Controller
    {
        public readonly IUriService _uriService;
        public readonly IMapper _mapper;
        private IDateTimeProvider _dateTimeProvider;
        private HttpContext _httpContext;

        public ControllerBase(IMapper mapper, IUriService uriService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _uriService = uriService ?? throw new ArgumentNullException(nameof(uriService));
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

        public void SetDefaultFieldsFor(CreateCategoryRequest categoryRequest)
        {            
            if (categoryRequest != null)
            {
                categoryRequest.CategoryId = Guid.NewGuid();
                categoryRequest.DateCreated = DateTimeProvider.Now;
                categoryRequest.DateLastModified = DateTimeProvider.Now;
                categoryRequest.UserId = categoryRequest.UserId;
                categoryRequest.CreatedBy = categoryRequest.UserName;
                categoryRequest.LastUpdatedBy = categoryRequest.UserName;
            }
        }

        public void UpdateBaseFieldsOn(UpdateCategoryRequest request)
        {            
            request.DateLastModified = DateTimeProvider.Now;
            request.LastUpdatedBy = request.UserName;       
        }
        
        public void SetDefaultFieldsFor(CreateProductRequest productRequest)
        {          
            productRequest.ProductId = Guid.NewGuid();
            productRequest.DateCreated = DateTimeProvider.Now;
            productRequest.DateLastModified = DateTimeProvider.Now;
            productRequest.CreatedBy = productRequest.UserName;
            productRequest.LastUpdatedBy = productRequest.UserName;
            productRequest.UserId = productRequest.UserId;
        }

        public void UpdateBaseFieldsOn(UpdateProductRequest request)
        {
            request.DateLastModified = DateTimeProvider.Now;            
            request.LastUpdatedBy = request.UserName;            
        }

        public IActionResult InvalidRequest(string message = null)
        {
            var messageToDisplay = message ?? "Please login...";
            return BadRequest(new ErrorResponse(new ErrorModel { Message = messageToDisplay }));
        }
        public string ValidateCategoryCode(string categoryCode)
        {
            var invalidCodeMessage = "Invalid code format, code must be 3 alphabet letters and three numeric characters e.g., ABC123.";
            var length = categoryCode.Length;

            if (length != 6)
                return invalidCodeMessage;

            var firstLetters = categoryCode.Substring(0, 3);
            if (!IsAlpha(firstLetters))
                return invalidCodeMessage;

            var lastLetters = categoryCode.Substring(length - 3);
            string validResposnse;
            try
            {
                int validNumber = int.Parse(lastLetters);
                validResposnse = "";
            }
            catch (Exception)
            {
                validResposnse = invalidCodeMessage;
            }

            return validResposnse;
        }

        public bool IsAlpha(string name)
        {
            return Regex.IsMatch(name, @"^[a-zA-Z]+$");
        }
    }
}
