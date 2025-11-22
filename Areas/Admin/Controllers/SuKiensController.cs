using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLySuKien.Data;
using QuanLySuKien.Models;

namespace QuanLySuKien.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SuKiensController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuKiensController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/SuKiens
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SuKiens.Include(s => s.DiaDiem);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/SuKiens/Details/5
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

        // GET: Admin/SuKiens/Create
        public IActionResult Create()
        {
            ViewData["DiaDiemId"] = new SelectList(_context.DiaDiems, "Id", "Id");
            return View();
        }

        // POST: Admin/SuKiens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenSuKien,LoaiSuKien,DiaDiemId,NgayToChuc,GioToChuc,AnhBia,MoTa,TrangThai")] SuKien suKien)
        {
            if (ModelState.IsValid)
            {
                _context.Add(suKien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DiaDiemId"] = new SelectList(_context.DiaDiems, "Id", "Id", suKien.DiaDiemId);
            return View(suKien);
        }

        // GET: Admin/SuKiens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suKien = await _context.SuKiens.FindAsync(id);
            if (suKien == null)
            {
                return NotFound();
            }
            ViewData["DiaDiemId"] = new SelectList(_context.DiaDiems, "Id", "Id", suKien.DiaDiemId);
            return View(suKien);
        }

        // POST: Admin/SuKiens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenSuKien,LoaiSuKien,DiaDiemId,NgayToChuc,GioToChuc,AnhBia,MoTa,TrangThai")] SuKien suKien)
        {
            if (id != suKien.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suKien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuKienExists(suKien.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DiaDiemId"] = new SelectList(_context.DiaDiems, "Id", "Id", suKien.DiaDiemId);
            return View(suKien);
        }

        // GET: Admin/SuKiens/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Admin/SuKiens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suKien = await _context.SuKiens.FindAsync(id);
            if (suKien != null)
            {
                _context.SuKiens.Remove(suKien);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuKienExists(int id)
        {
            return _context.SuKiens.Any(e => e.Id == id);
        }
    }
}
