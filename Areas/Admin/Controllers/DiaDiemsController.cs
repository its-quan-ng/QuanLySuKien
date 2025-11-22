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
    public class DiaDiemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DiaDiemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/DiaDiems
        public async Task<IActionResult> Index()
        {
            return View(await _context.DiaDiems.ToListAsync());
        }

        // GET: Admin/DiaDiems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diaDiem = await _context.DiaDiems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (diaDiem == null)
            {
                return NotFound();
            }

            return View(diaDiem);
        }

        // GET: Admin/DiaDiems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/DiaDiems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenDiaDiem,DiaChi,SucChua,MoTa")] DiaDiem diaDiem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(diaDiem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(diaDiem);
        }

        // GET: Admin/DiaDiems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diaDiem = await _context.DiaDiems.FindAsync(id);
            if (diaDiem == null)
            {
                return NotFound();
            }
            return View(diaDiem);
        }

        // POST: Admin/DiaDiems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenDiaDiem,DiaChi,SucChua,MoTa")] DiaDiem diaDiem)
        {
            if (id != diaDiem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(diaDiem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiaDiemExists(diaDiem.Id))
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
            return View(diaDiem);
        }

        // GET: Admin/DiaDiems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diaDiem = await _context.DiaDiems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (diaDiem == null)
            {
                return NotFound();
            }

            return View(diaDiem);
        }

        // POST: Admin/DiaDiems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var diaDiem = await _context.DiaDiems.FindAsync(id);
            if (diaDiem != null)
            {
                _context.DiaDiems.Remove(diaDiem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiaDiemExists(int id)
        {
            return _context.DiaDiems.Any(e => e.Id == id);
        }
    }
}
