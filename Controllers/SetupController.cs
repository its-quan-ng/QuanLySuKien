using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace QuanLySuKien.Controllers
{
    public class SetupController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SetupController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdmin()
        {
            try
            {
                // Tạo Roles
                string[] roleNames = { "Admin", "User" };
                foreach (var roleName in roleNames)
                {
                    var roleExist = await _roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                // Tạo Admin User
                var adminEmail = "admin@eventhub.com";
                var adminPassword = "Admin@123";

                var adminUser = await _userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    var newAdmin = new IdentityUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true
                    };

                    var createAdmin = await _userManager.CreateAsync(newAdmin, adminPassword);
                    if (createAdmin.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(newAdmin, "Admin");
                        ViewBag.Message = "Admin account created successfully!";
                        ViewBag.Success = true;
                    }
                    else
                    {
                        ViewBag.Message = "Error: " + string.Join(", ", createAdmin.Errors.Select(e => e.Description));
                        ViewBag.Success = false;
                    }
                }
                else
                {
                    // Đảm bảo admin user có role Admin
                    if (!await _userManager.IsInRoleAsync(adminUser, "Admin"))
                    {
                        await _userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                    ViewBag.Message = "Admin account already exists!";
                    ViewBag.Success = true;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error: " + ex.Message;
                ViewBag.Success = false;
            }

            return View("Index");
        }
    }
}
