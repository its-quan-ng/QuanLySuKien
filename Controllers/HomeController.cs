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
            // Lấy events cho carousel (ưu tiên có ảnh, chỉ sắp diễn ra hoặc đang diễn ra)
            var carouselEvents = await _context.SuKiens
                .Include(s => s.DiaDiem)
                .Include(s => s.LoaiVes)
                .Where(s => !string.IsNullOrEmpty(s.AnhBia) &&
                           (s.TrangThai == "SapDienRa" || s.TrangThai == "DangDienRa"))
                .OrderBy(s => Guid.NewGuid()) // Random
                .Take(5)
                .ToListAsync();

            ViewBag.CarouselEvents = carouselEvents;

            // Lấy categories từ ALL events với event count
            var categoryCounts = await _context.SuKiens
                .Where(s => !string.IsNullOrEmpty(s.LoaiSuKien))
                .GroupBy(s => s.LoaiSuKien)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToListAsync();

            ViewBag.CategoryCounts = categoryCounts;

            // Lấy 6 sự kiện nổi bật (sắp diễn ra HOẶC đang diễn ra)
            var featuredEvents = await _context.SuKiens
                .Include(s => s.DiaDiem)
                .Include(s => s.LoaiVes)
                .Where(s => s.TrangThai == "SapDienRa" || s.TrangThai == "DangDienRa")
                .OrderBy(s => s.NgayToChuc)
                .Take(6)
                .ToListAsync();

            return View(featuredEvents);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            ViewData["Title"] = "Truy Cập Bị Từ Chối";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
