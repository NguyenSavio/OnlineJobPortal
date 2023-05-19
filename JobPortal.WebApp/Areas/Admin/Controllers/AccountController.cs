using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JobPortal.Data.Entities;
using JobPortal.Data.ViewModel;

namespace JobPortal.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin")]
    [Route("admin/auth")]
    //[Route("Account")]
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> signInManager;
        private readonly RoleManager<AppRole> roleManager;


        public AccountController(SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager)
        {
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        //public AccountController(SignInManager<AppUser> signInManager)
        //{
        //    this.signInManager = signInManager;
        //}

        [HttpGet]
        //[HttpGet("Admin/Login")]
        [Route("login")]
        
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        //[HttpPost("Admin/Login")]
        [Route("login")]
        
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(
                    model.Email,
                    model.Password,
                    model.RememberMe,
                    false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    //else if (!model.Email.Equals("admin@gmail.com"))
                    //{
                    //    await signInManager.SignOutAsync();
                    //    return RedirectToAction("accessdenied", "account");
                    //}
                    //else if (!roleManager.Roles.Equals("Admin"))
                    //{
                    //    await signInManager.SignOutAsync();
                    //    return RedirectToAction("accessdenied", "account");
                    //}
                    else
                    {
                        
                        //return RedirectToAction("AccessDenied", "Account");
                        return RedirectToAction("Index", "Home");

                    }
                }
                else if (!result.Succeeded)
                    return RedirectToAction("AccessDenied", "Account");

            }
            
            return View(model);
        }

        [Route("logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            //return RedirectToRoute("admin/auth/login");
            return RedirectToAction("Index", "Home");
        }

        //[HttpGet("admin/auth/access-denined")]
        [HttpGet]
        [Route("access-denined")]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {

            //return RedirectToRoute("admin/auth/access-denined");
             return View();
        }
    }
}