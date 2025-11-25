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

        // GET: Admin/CreateUser
        public IActionResult CreateUser()
        {
            ViewData["Title"] = "Tạo User Mới";
            return View();
        }

        // POST: Admin/CreateUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(string email, string password, string role)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                TempData["Error"] = "Email và mật khẩu là bắt buộc";
                return RedirectToAction(nameof(CreateUser));
            }

            var user = new IdentityUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Assign role
                if (!string.IsNullOrEmpty(role) && (role == "Admin" || role == "User"))
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, "User"); // Default role
                }

                TempData["Success"] = $"Đã tạo user {email} thành công";
                return RedirectToAction(nameof(Users));
            }

            TempData["Error"] = string.Join(", ", result.Errors.Select(e => e.Description));
            return RedirectToAction(nameof(CreateUser));
        }

        // GET: Admin/EditUserRoles/id
        public async Task<IActionResult> EditUserRoles(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            ViewData["Title"] = "Chỉnh Sửa Roles";
            ViewBag.UserId = user.Id;
            ViewBag.UserEmail = user.Email;
            ViewBag.CurrentRoles = userRoles;

            return View();
        }

        // POST: Admin/EditUserRoles
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserRoles(string userId, List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Remove all current roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // Add new roles
            if (roles != null && roles.Any())
            {
                await _userManager.AddToRolesAsync(user, roles);
            }

            TempData["Success"] = $"Đã cập nhật roles cho {user.Email}";
            return RedirectToAction(nameof(Users));
        }

        // POST: Admin/DeleteUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Error"] = "Không tìm thấy user";
                return RedirectToAction(nameof(Users));
            }

            // Prevent deleting admin users
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Admin"))
            {
                TempData["Error"] = "Không thể xóa tài khoản Admin";
                return RedirectToAction(nameof(Users));
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = $"Đã xóa user {user.Email}";
            }
            else
            {
                TempData["Error"] = "Không thể xóa user";
            }

            return RedirectToAction(nameof(Users));
        }
    }
}
