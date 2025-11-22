using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySuKien.Data;
using QuanLySuKien.Models;

namespace QuanLySuKien.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Lấy 6 sự kiện sắp diễn ra
            var upcomingSuKiens = await _context.SuKiens
                .Include(s => s.DiaDiem)
                .Where(s => s.NgayToChuc >= DateOnly.FromDateTime(DateTime.Now))
                .OrderBy(s => s.NgayToChuc)
                .Take(6)
                .ToListAsync();

            return View(upcomingSuKiens);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
