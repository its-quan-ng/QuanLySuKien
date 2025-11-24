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
    public class NgheSisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NgheSisController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: NgheSis
        public async Task<IActionResult> Index()
        {
            return View(await _context.NgheSis.ToListAsync());
        }

        // GET: NgheSis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ngheSi = await _context.NgheSis
                .Include(n => n.SuKiens)
                    .ThenInclude(s => s.DiaDiem)
                .Include(n => n.SuKiens)
                    .ThenInclude(s => s.LoaiVes)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ngheSi == null)
            {
                return NotFound();
            }

            return View(ngheSi);
        }
    }
}
