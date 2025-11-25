using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySuKien.Data;
using QuanLySuKien.Models.ViewModels;

namespace QuanLySuKien.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Admin Dashboard";

            // Statistics
            var totalUsers = await _userManager.Users.CountAsync();
            var totalEvents = await _context.SuKiens.CountAsync();
            var totalOrders = await _context.DonHangs.CountAsync();
            var totalRevenue = await _context.DonHangs
                .Where(d => d.TrangThai == "DaXacNhan" || d.TrangThai == "DaThanhToan")
                .SumAsync(d => (decimal?)d.TongTien) ?? 0;

            // Recent orders
            var recentOrders = await _context.DonHangs
                .Include(d => d.SuKien)
                .Include(d => d.LoaiVe)
                .OrderByDescending(d => d.NgayDat)
                .Take(10)
                .ToListAsync();

            // Upcoming events
            var upcomingEvents = await _context.SuKiens
                .Include(s => s.DiaDiem)
                .Where(s => s.NgayToChuc >= DateOnly.FromDateTime(DateTime.Now))
                .OrderBy(s => s.NgayToChuc)
                .Take(5)
                .ToListAsync();

            // Orders by status
            var ordersByStatus = await _context.DonHangs
                .GroupBy(d => d.TrangThai)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalEvents = totalEvents;
            ViewBag.TotalOrders = totalOrders;
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.RecentOrders = recentOrders;
            ViewBag.UpcomingEvents = upcomingEvents;
            ViewBag.OrdersByStatus = ordersByStatus;

            return View();
        }

        public async Task<IActionResult> Users()
        {
            ViewData["Title"] = "Quản Lý Users";

            var users = await _userManager.Users.ToListAsync();
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userViewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    EmailConfirmed = user.EmailConfirmed,
                    Roles = roles.ToList()
                });
            }

            return View(userViewModels);
        }

        public IActionResult Events()
        {
            ViewData["Title"] = "Quản Lý Sự Kiện";
            return View();
        }
    }
}
