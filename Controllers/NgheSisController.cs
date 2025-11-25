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

        // GET: NgheSis/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Tạo Nghệ Sĩ Mới";
            return View();
        }

        // POST: NgheSis/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("TenNgheSi,TheLoai,TieuSu")] NgheSi ngheSi, IFormFile? anhDaiDienFile)
        {
            ModelState.Remove("AnhDaiDien");
            ModelState.Remove("SuKiens");

            if (ModelState.IsValid)
            {
                if (anhDaiDienFile != null && anhDaiDienFile.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(anhDaiDienFile.FileName)}";
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "artists");
                    Directory.CreateDirectory(uploadPath);
                    var filePath = Path.Combine(uploadPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await anhDaiDienFile.CopyToAsync(stream);
                    }
                    ngheSi.AnhDaiDien = $"/images/artists/{fileName}";
                }

                _context.Add(ngheSi);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Đã tạo nghệ sĩ '{ngheSi.TenNgheSi}' thành công";
                return RedirectToAction(nameof(Index));
            }

            return View(ngheSi);
        }

        // GET: NgheSis/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var ngheSi = await _context.NgheSis.FindAsync(id);
            if (ngheSi == null) return NotFound();
            ViewData["Title"] = "Chỉnh Sửa Nghệ Sĩ";
            return View(ngheSi);
        }

        // POST: NgheSis/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenNgheSi,TheLoai,TieuSu")] NgheSi ngheSi, IFormFile? anhDaiDienFile)
        {
            if (id != ngheSi.Id) return NotFound();

            var existing = await _context.NgheSis.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id);
            if (existing == null) return NotFound();

            ngheSi.AnhDaiDien = existing.AnhDaiDien;
            ModelState.Remove("AnhDaiDien");
            ModelState.Remove("SuKiens");

            if (ModelState.IsValid)
            {
                if (anhDaiDienFile != null && anhDaiDienFile.Length > 0)
                {
                    if (!string.IsNullOrEmpty(ngheSi.AnhDaiDien))
                    {
                        var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", ngheSi.AnhDaiDien.TrimStart('/'));
                        if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                    }

                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(anhDaiDienFile.FileName)}";
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "artists");
                    Directory.CreateDirectory(uploadPath);
                    var filePath = Path.Combine(uploadPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await anhDaiDienFile.CopyToAsync(stream);
                    }
                    ngheSi.AnhDaiDien = $"/images/artists/{fileName}";
                }

                _context.Update(ngheSi);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Đã cập nhật nghệ sĩ '{ngheSi.TenNgheSi}'";
                return RedirectToAction(nameof(Index));
            }
            return View(ngheSi);
        }

        // GET: NgheSis/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var ngheSi = await _context.NgheSis.FirstOrDefaultAsync(m => m.Id == id);
            if (ngheSi == null) return NotFound();
            ViewData["Title"] = "Xóa Nghệ Sĩ";
            return View(ngheSi);
        }

        // POST: NgheSis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ngheSi = await _context.NgheSis.FindAsync(id);
            if (ngheSi != null)
            {
                if (!string.IsNullOrEmpty(ngheSi.AnhDaiDien))
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", ngheSi.AnhDaiDien.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath)) System.IO.File.Delete(imagePath);
                }
                _context.NgheSis.Remove(ngheSi);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Đã xóa nghệ sĩ '{ngheSi.TenNgheSi}'";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

