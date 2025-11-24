using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLySuKien.Data;
using QuanLySuKien.Models;

namespace QuanLySuKien.Controllers
{
    public class DiaDiemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DiaDiemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DiaDiems
        public async Task<IActionResult> Index()
        {
            var diaDiems = await _context.DiaDiems
                .Include(d => d.SuKiens)
                .ToListAsync();
            return View(diaDiems);
        }

        // GET: DiaDiems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diaDiem = await _context.DiaDiems
                .Include(d => d.SuKiens.Where(s => s.TrangThai == "SapDienRa"))
                    .ThenInclude(s => s.LoaiVes)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (diaDiem == null)
            {
                return NotFound();
            }

            return View(diaDiem);
        }
    }
}
