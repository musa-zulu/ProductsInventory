using AutoMapper;
using NSubstitute;
using ProductsInventory.Persistence.Interfaces.Services;
using ProductsInventory.Server.Controllers.V1;

namespace ProductInventory.Tests.Common.Builders.Controllers
{
    public class UsersControllerBuilder
    {
        public UsersControllerBuilder()
        {
            Mapper = Substitute.For<IMapper>();
            UriService = Substitute.For<IUriService>();
            UserService = Substitute.For<IUserService>();
        }

        public IMapper Mapper { get; private set; }
        public IUriService UriService { get; private set; }
        public IUserService UserService { get; private set; }

        public UsersControllerBuilder WithMapper(IMapper mapper)
        {
            Mapper = mapper;
            return this;
        }

        public UsersControllerBuilder WithUriService(IUriService uriService)
        {
            UriService = uriService;
            return this;
        }

        public UsersControllerBuilder WithUserService(IUserService userService)
        {
            UserService = userService;
            return this;
        }

        public UsersController Build()
        {
            return new UsersController(UserService, Mapper, UriService);
        }
    }
}
