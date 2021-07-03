using AutoMapper;
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
using System.Threading.Tasks;

namespace ProductsInventory.Server.Controllers.V1
{
    public class UsersController : Controller
    {
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IEncryptionService _encryptionService;
        private IDateTimeProvider _dateTimeProvider;

        public UsersController(IUserService userService, IMapper mapper, IUriService uriService, IEncryptionService encryptionService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _uriService = uriService ?? throw new ArgumentNullException(nameof(uriService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
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

        [HttpGet(ApiRoutes.Users.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);

            var users = await _userService.GetUsersAsync(pagination);
            var userResponse = _mapper.Map<List<UserResponse>>(users);

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<UserResponse>(userResponse));
            }

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, userResponse);
            return Ok(paginationResponse);
        }

        [HttpGet(ApiRoutes.Users.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid userId)
        {
            var user = await _userService.GetUserByUserIdAsync(userId);

            if (user == null)
                return NotFound();

            var userResponse = _mapper.Map<UserResponse>(user);
            return Ok(new Response<UserResponse>(userResponse));
        }

        [HttpGet(ApiRoutes.Users.GetByUsername)]
        public async Task<IActionResult> GetByUserName([FromRoute] string userName)
        {
            var user = await _userService.GetUserByUserNameAsync(userName);

            if (user == null)
                return NotFound();

            var userResponse = _mapper.Map<UserResponse>(user);
            return Ok(new Response<UserResponse>(userResponse));
        }

        [HttpPost(ApiRoutes.Users.Create)]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest userRequest)
        {
            SetDefaultFieldsFor(userRequest);
            var user = _mapper.Map<CreateUserRequest, User>(userRequest);

            await _userService.CreateUserAsync(user);

            var locationUri = _uriService.GetUserUri(user.UserId.ToString());
            return Created(locationUri, new Response<UserResponse>(_mapper.Map<UserResponse>(user)));
        }

        private void SetDefaultFieldsFor(CreateUserRequest userRequest)
        {
            var saltPassword = _encryptionService.CreateSalt();
            var hashedPassword = _encryptionService.EncryptPassword(userRequest.Password, saltPassword);

            userRequest.UserId = Guid.NewGuid();
            userRequest.IsLocked = false;
            userRequest.HashedPassword = hashedPassword;
            userRequest.Salt = saltPassword;
            userRequest.DateCreated = DateTimeProvider.Now;
            userRequest.DateLastModified = DateTimeProvider.Now;
        }
    }
}
