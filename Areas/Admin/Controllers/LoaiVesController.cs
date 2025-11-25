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
    public class LoaiVesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoaiVesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/LoaiVes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.LoaiVes.Include(l => l.SuKien);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/LoaiVes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiVe = await _context.LoaiVes
                .Include(l => l.SuKien)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loaiVe == null)
            {
                return NotFound();
            }

            return View(loaiVe);
        }

        // GET: Admin/LoaiVes/Create
        public IActionResult Create()
        {
            ViewData["SuKienId"] = new SelectList(_context.SuKiens, "Id", "TenSuKien");
            return View();
        }

        // POST: Admin/LoaiVes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SuKienId,TenLoai,GiaVe,TongSoLuong,SoLuongConLai")] LoaiVe loaiVe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loaiVe);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Đã thêm loại vé '{loaiVe.TenLoai}' thành công!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["SuKienId"] = new SelectList(_context.SuKiens, "Id", "TenSuKien", loaiVe.SuKienId);
            return View(loaiVe);
        }

        // GET: Admin/LoaiVes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiVe = await _context.LoaiVes.FindAsync(id);
            if (loaiVe == null)
            {
                return NotFound();
            }
            ViewData["SuKienId"] = new SelectList(_context.SuKiens, "Id", "TenSuKien", loaiVe.SuKienId);
            return View(loaiVe);
        }

        // POST: Admin/LoaiVes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SuKienId,TenLoai,GiaVe,TongSoLuong,SoLuongConLai")] LoaiVe loaiVe)
        {
            if (id != loaiVe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loaiVe);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = $"Đã cập nhật loại vé '{loaiVe.TenLoai}' thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoaiVeExists(loaiVe.Id))
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
            ViewData["SuKienId"] = new SelectList(_context.SuKiens, "Id", "TenSuKien", loaiVe.SuKienId);
            return View(loaiVe);
        }

        // GET: Admin/LoaiVes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiVe = await _context.LoaiVes
                .Include(l => l.SuKien)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loaiVe == null)
            {
                return NotFound();
            }

            return View(loaiVe);
        }

        // POST: Admin/LoaiVes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loaiVe = await _context.LoaiVes.FindAsync(id);
            if (loaiVe != null)
            {
                _context.LoaiVes.Remove(loaiVe);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Đã xóa loại vé thành công!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool LoaiVeExists(int id)
        {
            return _context.LoaiVes.Any(e => e.Id == id);
        }
    }
}
