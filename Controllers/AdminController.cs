using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuanLySuKien.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Admin Dashboard";
            return View();
        }

        public IActionResult Users()
        {
            ViewData["Title"] = "Quản Lý Users";
            return View();
        }

        public IActionResult Events()
        {
            ViewData["Title"] = "Quản Lý Sự Kiện";
            return View();
        }
    }
}
