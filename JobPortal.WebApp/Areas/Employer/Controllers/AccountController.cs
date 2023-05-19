using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JobPortal.Data.Entities;
using JobPortal.Data.ViewModel;

namespace JobPortal.WebApp.Areas.Employer.Controllers
{
    [Area("Employer")]
    [Route("employer")]
    [Route("employer/auth")]
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> signInManager;
        private readonly RoleManager<AppRole> roleManager;

        public AccountController(SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager)
        {
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        [Route("login")]
        //[HttpGet("Employer/Login")]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = "/")
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [Route("login")]
        //[HttpPost("Employer/Login")]
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
                    //else if (!roleManager.Roles.Equals("Employer"))
                    //{
                    //    await signInManager.SignOutAsync();
                    //    return RedirectToAction("accessdenied", "account");
                    //}
                    //else if (!model.Email.Equals("employer@gmail.com"))
                    //{
                    //    await signInManager.SignOutAsync();
                    //    return RedirectToAction("accessdenied", "account");
                    //}
                    else
                    {
                        return RedirectToAction("index", "home");
                    }
                }
            }
            return View(model);
        }

        [Route("logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        [Route("access-denied")]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}