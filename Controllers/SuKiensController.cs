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
    public class SuKiensController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuKiensController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SuKiens
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SuKiens.Include(s => s.DiaDiem);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SuKiens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suKien = await _context.SuKiens
                .Include(s => s.DiaDiem)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (suKien == null)
            {
                return NotFound();
            }

            return View(suKien);
        }
    }
}
