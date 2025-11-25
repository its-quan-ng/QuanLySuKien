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
    public class NgheSisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NgheSisController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/NgheSis
        public async Task<IActionResult> Index()
        {
            return View(await _context.NgheSis.ToListAsync());
        }

        // GET: Admin/NgheSis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ngheSi = await _context.NgheSis
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ngheSi == null)
            {
                return NotFound();
            }

            return View(ngheSi);
        }

        // GET: Admin/NgheSis/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/NgheSis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenNgheSi,TheLoai,TieuSu,AnhDaiDien")] NgheSi ngheSi)
        {
            // Check for duplicate name
            if (await _context.NgheSis.AnyAsync(n => n.TenNgheSi == ngheSi.TenNgheSi))
            {
                ModelState.AddModelError("TenNgheSi", "Tên nghệ sĩ này đã tồn tại. Vui lòng chọn tên khác.");
                return View(ngheSi);
            }

            if (ModelState.IsValid)
            {
                _context.Add(ngheSi);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Đã thêm nghệ sĩ '{ngheSi.TenNgheSi}' thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(ngheSi);
        }

        // GET: Admin/NgheSis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ngheSi = await _context.NgheSis.FindAsync(id);
            if (ngheSi == null)
            {
                return NotFound();
            }
            return View(ngheSi);
        }

        // POST: Admin/NgheSis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenNgheSi,TheLoai,TieuSu,AnhDaiDien")] NgheSi ngheSi)
        {
            if (id != ngheSi.Id)
            {
                return NotFound();
            }

            // Check for duplicate name (excluding current record)
            if (await _context.NgheSis.AnyAsync(n => n.TenNgheSi == ngheSi.TenNgheSi && n.Id != id))
            {
                ModelState.AddModelError("TenNgheSi", "Tên nghệ sĩ này đã tồn tại. Vui lòng chọn tên khác.");
                return View(ngheSi);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ngheSi);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = $"Đã cập nhật nghệ sĩ '{ngheSi.TenNgheSi}' thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NgheSiExists(ngheSi.Id))
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
            return View(ngheSi);
        }

        // GET: Admin/NgheSis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ngheSi = await _context.NgheSis
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ngheSi == null)
            {
                return NotFound();
            }

            return View(ngheSi);
        }

        // POST: Admin/NgheSis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ngheSi = await _context.NgheSis.FindAsync(id);
            if (ngheSi != null)
            {
                _context.NgheSis.Remove(ngheSi);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NgheSiExists(int id)
        {
            return _context.NgheSis.Any(e => e.Id == id);
        }
    }
}
