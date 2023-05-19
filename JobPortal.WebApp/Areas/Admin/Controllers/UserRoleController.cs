using JobPortal.Data.DataContext;
using JobPortal.Data.Entities;
using JobPortal.Data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.WebApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("admin/users")]
    public class UserRoleController : Controller
    {
        private readonly RoleManager<AppRole> roleManager;

        private readonly UserManager<AppUser> userManager;
        private readonly DataDbContext dataDbContext;
        public UserRoleController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager, DataDbContext dataDbContext)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dataDbContext = dataDbContext;
        }

        [Route("")]
        public IActionResult Index(Guid Id)
        {


            //ViewData["RoleID"] = new SelectList(dataDbContext.AppRoles, "Id", "Name");
            //var users = dataDbContext.Users.Where(u => u.Id == Id).First();
            var users = userManager.Users;
            return View(users);
        }

        // Phân quyền Users
        [Route("manage-user-roles/{userId}")]
        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(Guid userId)
        {
            ViewBag.userId = userId;

            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRolesViewModel>();

            foreach (var role in roleManager.Roles)
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }

                model.Add(userRolesViewModel);
            }

            return View(model);
        }

        [Route("manage-user-roles/{userId}")]
        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }

            result = await userManager.AddToRolesAsync(user,
                model.Where(x => x.IsSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }

            return RedirectToAction("Index");
        }

    }
}
