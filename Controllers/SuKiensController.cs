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
    public class SuKiensController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuKiensController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SuKiens
        public async Task<IActionResult> Index(string searchKeyword)
        {
            var query = _context.SuKiens
                .Include(s => s.DiaDiem)
                .Include(s => s.LoaiVes)
                .Where(s => s.TrangThai == "SapDienRa" || s.TrangThai == "DangDienRa");

            // Search filter
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                query = query.Where(s =>
                    EF.Functions.Like(s.TenSuKien, $"%{searchKeyword}%") ||
                    EF.Functions.Like(s.DiaDiem.TenDiaDiem, $"%{searchKeyword}%") ||
                    (s.LoaiSuKien != null && EF.Functions.Like(s.LoaiSuKien, $"%{searchKeyword}%"))
                );
                ViewBag.SearchKeyword = searchKeyword;
            }

            var events = await query
                .OrderBy(s => s.NgayToChuc)
                .ThenBy(s => s.GioToChuc)
                .ToListAsync();

            return View(events);
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
                .Include(s => s.LoaiVes)
                .Include(s => s.NgheSis)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (suKien == null)
            {
                return NotFound();
            }

            // Get related events (same venue or same category, excluding current event)
            var relatedEvents = await _context.SuKiens
                .Include(s => s.DiaDiem)
                .Include(s => s.LoaiVes)
                .Where(s => s.Id != id &&
                       s.TrangThai == "SapDienRa" &&
                       (s.DiaDiemId == suKien.DiaDiemId || s.LoaiSuKien == suKien.LoaiSuKien))
                .OrderBy(s => s.NgayToChuc)
                .Take(6)
                .ToListAsync();

            ViewBag.RelatedEvents = relatedEvents;

            return View(suKien);
        }

        // GET: SuKiens/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Tạo Sự Kiện Mới";
            ViewData["DiaDiemId"] = new SelectList(_context.DiaDiems, "Id", "TenDiaDiem");
            return View();
        }

        // POST: SuKiens/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("TenSuKien,LoaiSuKien,DiaDiemId,NgayToChuc,GioToChuc,MoTa,TrangThai")] SuKien suKien, IFormFile? anhBiaFile)
        {
            // Remove navigation property validations
            ModelState.Remove("AnhBia");
            ModelState.Remove("DiaDiem");
            ModelState.Remove("LoaiVes");
            ModelState.Remove("DonHangs");
            ModelState.Remove("NgheSis");

            if (ModelState.IsValid)
            {
                // Handle image upload
                if (anhBiaFile != null && anhBiaFile.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(anhBiaFile.FileName)}";
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "events");

                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    var filePath = Path.Combine(uploadPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await anhBiaFile.CopyToAsync(stream);
                    }

                    suKien.AnhBia = $"/images/events/{fileName}";
                }

                _context.Add(suKien);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Đã tạo sự kiện '{suKien.TenSuKien}' thành công";
                return RedirectToAction(nameof(Index));
            }

            // Debug: Show validation errors
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            TempData["Error"] = "Lỗi validation: " + string.Join(", ", errors);

            ViewData["DiaDiemId"] = new SelectList(_context.DiaDiems, "Id", "TenDiaDiem", suKien.DiaDiemId);
            return View(suKien);
        }

        // GET: SuKiens/Edit/5
        [Authorize(Roles = "Admin")]
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

            ViewData["Title"] = "Chỉnh Sửa Sự Kiện";
            ViewData["DiaDiemId"] = new SelectList(_context.DiaDiems, "Id", "TenDiaDiem", suKien.DiaDiemId);
            return View(suKien);
        }

        // POST: SuKiens/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenSuKien,LoaiSuKien,DiaDiemId,NgayToChuc,GioToChuc,MoTa,TrangThai")] SuKien suKien, IFormFile? anhBiaFile)
        {
            if (id != suKien.Id)
            {
                return NotFound();
            }

            // Get current event to preserve AnhBia
            var existingEvent = await _context.SuKiens.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
            if (existingEvent == null)
            {
                return NotFound();
            }

            // Preserve old image path
            suKien.AnhBia = existingEvent.AnhBia;

            // Remove navigation property validations
            ModelState.Remove("AnhBia");
            ModelState.Remove("DiaDiem");
            ModelState.Remove("LoaiVes");
            ModelState.Remove("DonHangs");
            ModelState.Remove("NgheSis");

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle image upload
                    if (anhBiaFile != null && anhBiaFile.Length > 0)
                    {
                        // Delete old image if exists
                        if (!string.IsNullOrEmpty(suKien.AnhBia))
                        {
                            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", suKien.AnhBia.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Upload new image
                        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(anhBiaFile.FileName)}";
                        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "events");

                        if (!Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(uploadPath);
                        }

                        var filePath = Path.Combine(uploadPath, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await anhBiaFile.CopyToAsync(stream);
                        }

                        suKien.AnhBia = $"/images/events/{fileName}";
                    }

                    _context.Update(suKien);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = $"Đã cập nhật sự kiện '{suKien.TenSuKien}' thành công";
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

            ViewData["DiaDiemId"] = new SelectList(_context.DiaDiems, "Id", "TenDiaDiem", suKien.DiaDiemId);
            return View(suKien);
        }

        // GET: SuKiens/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suKien = await _context.SuKiens
                .Include(s => s.DiaDiem)
                .Include(s => s.LoaiVes)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (suKien == null)
            {
                return NotFound();
            }

            ViewData["Title"] = "Xóa Sự Kiện";
            return View(suKien);
        }

        // POST: SuKiens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suKien = await _context.SuKiens.FindAsync(id);

            if (suKien != null)
            {
                // Step 1: Get all LoaiVe IDs for this event
                var loaiVeIds = await _context.LoaiVes
                    .Where(lv => lv.SuKienId == id)
                    .Select(lv => lv.Id)
                    .ToListAsync();

                // Step 2: Delete ALL orders related to this event
                // (both via SuKienId and via LoaiVeId)
                var ordersToDelete = await _context.DonHangs
                    .Where(d => d.SuKienId == id || loaiVeIds.Contains(d.LoaiVeId))
                    .ToListAsync();

                if (ordersToDelete.Any())
                {
                    _context.DonHangs.RemoveRange(ordersToDelete);
                }

                // Step 3: Delete all ticket types
                var loaiVesToDelete = await _context.LoaiVes
                    .Where(lv => lv.SuKienId == id)
                    .ToListAsync();

                if (loaiVesToDelete.Any())
                {
                    _context.LoaiVes.RemoveRange(loaiVesToDelete);
                }

                // Step 4: Delete image if exists
                if (!string.IsNullOrEmpty(suKien.AnhBia))
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", suKien.AnhBia.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                // Step 5: Delete the event
                _context.SuKiens.Remove(suKien);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"Đã xóa sự kiện '{suKien.TenSuKien}' và {ordersToDelete.Count} đơn hàng liên quan";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SuKienExists(int id)
        {
            return _context.SuKiens.Any(e => e.Id == id);
        }
    }
}
