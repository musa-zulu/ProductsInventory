using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductsInventory.Persistence.V1;
using ProductsInventory.Persistence.V1.Requests;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProductsInventory.Server.Controllers.V1
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AccountController(UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
                
        [HttpPost(ApiRoutes.Account.Register)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok(result.Succeeded);
                }
                return BadRequest(result.Errors);
            }
            return BadRequest();
        }

        [HttpPost(ApiRoutes.Account.Login)]
        public async Task<IActionResult> Login([FromBody] LoginRequest user)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);
                if (result.Succeeded)
                {
                    var currentUser = _userManager.Users.FirstOrDefault(u => u.UserName == user.Email);
                    return Ok(currentUser);
                }
                return BadRequest(result.IsNotAllowed);
            }
            return BadRequest();
        }

        [HttpPost(ApiRoutes.Account.Logout)]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok();
        }

        [HttpPost(ApiRoutes.Account.GetUser)]
        public IActionResult SignedInUser([FromBody] GetUserRequest request)
        {
            var currentUser = _userManager.Users.FirstOrDefault(u => u.UserName == request.Email);            
            
            if (currentUser == null)
            {
                return Ok("user doesn't exist!");
            }
            var user = JsonConvert.SerializeObject(currentUser);
            return Ok(user);
        }
    }
}