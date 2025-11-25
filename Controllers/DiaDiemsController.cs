using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        // GET: DiaDiems/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Tạo Địa Điểm Mới";
            return View();
        }

        // POST: DiaDiems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("TenDiaDiem,DiaChi,SucChua,MoTa")] DiaDiem diaDiem)
        {
            ModelState.Remove("SuKiens");

            if (ModelState.IsValid)
            {
                _context.Add(diaDiem);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Đã tạo địa điểm '{diaDiem.TenDiaDiem}' thành công";
                return RedirectToAction(nameof(Index));
            }
            return View(diaDiem);
        }

        // GET: DiaDiems/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var diaDiem = await _context.DiaDiems.FindAsync(id);
            if (diaDiem == null) return NotFound();
            ViewData["Title"] = "Chỉnh Sửa Địa Điểm";
            return View(diaDiem);
        }

        // POST: DiaDiems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenDiaDiem,DiaChi,SucChua,MoTa")] DiaDiem diaDiem)
        {
            if (id != diaDiem.Id) return NotFound();
            ModelState.Remove("SuKiens");

            if (ModelState.IsValid)
            {
                _context.Update(diaDiem);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Đã cập nhật địa điểm '{diaDiem.TenDiaDiem}'";
                return RedirectToAction(nameof(Index));
            }
            return View(diaDiem);
        }

        // GET: DiaDiems/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var diaDiem = await _context.DiaDiems
                .Include(d => d.SuKiens)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (diaDiem == null) return NotFound();
            ViewData["Title"] = "Xóa Địa Điểm";
            return View(diaDiem);
        }

        // POST: DiaDiems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var diaDiem = await _context.DiaDiems
                .Include(d => d.SuKiens)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (diaDiem != null)
            {
                if (diaDiem.SuKiens != null && diaDiem.SuKiens.Any())
                {
                    TempData["Error"] = $"Không thể xóa '{diaDiem.TenDiaDiem}' vì có {diaDiem.SuKiens.Count} sự kiện đang sử dụng địa điểm này";
                    return RedirectToAction(nameof(Index));
                }

                _context.DiaDiems.Remove(diaDiem);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Đã xóa địa điểm '{diaDiem.TenDiaDiem}'";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

