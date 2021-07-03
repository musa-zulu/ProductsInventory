using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductsInventory.Persistence.V1;
using ProductsInventory.Persistence.V1.Requests;
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
        public async Task<IActionResult> Register(RegisterRequest model)
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
        public async Task<IActionResult> Login(LoginRequest user)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);
                if (result.Succeeded)
                {
                    return Ok(result.Succeeded);
                }
                return BadRequest();
            }
            return BadRequest();
        }

        [HttpPost(ApiRoutes.Account.Logout)]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok();
        }
    }
}