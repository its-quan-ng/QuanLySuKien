using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLySuKien.Data;
using QuanLySuKien.Models;
using QuanLySuKien.Models.ViewModels;
using Microsoft.AspNetCore.Antiforgery;

namespace QuanLySuKien.Controllers
{
    public class DonHangsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DonHangsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DonHangs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DonHangs.Include(d => d.LoaiVe).Include(d => d.SuKien);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DonHangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHangs
                .Include(d => d.LoaiVe)
                .Include(d => d.SuKien)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }

        // GET: DonHangs/Create
        public IActionResult Create()
        {
            ViewData["LoaiVeId"] = new SelectList(_context.LoaiVes, "Id", "Id");
            ViewData["SuKienId"] = new SelectList(_context.SuKiens, "Id", "Id");
            return View();
        }

        // POST: DonHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,SuKienId,LoaiVeId,SoLuong,TongTien,TenKhachHang,Email,SoDienThoai,NgayDat,TrangThai")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(donHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LoaiVeId"] = new SelectList(_context.LoaiVes, "Id", "Id", donHang.LoaiVeId);
            ViewData["SuKienId"] = new SelectList(_context.SuKiens, "Id", "Id", donHang.SuKienId);
            return View(donHang);
        }

        // POST: DonHangs/Purchase
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Purchase([FromBody] TicketPurchaseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderIds = new List<int>();

            try
            {
                // Create DonHang for each ticket type
                foreach (var ticket in model.Tickets)
                {
                    var loaiVe = await _context.LoaiVes.FindAsync(ticket.LoaiVeId);

                    if (loaiVe == null || loaiVe.SoLuongConLai < ticket.SoLuong)
                    {
                        return BadRequest(new { message = "Không đủ vé hoặc loại vé không tồn tại!" });
                    }

                    // Create order
                    var donHang = new DonHang
                    {
                        SuKienId = model.SuKienId,
                        LoaiVeId = ticket.LoaiVeId,
                        SoLuong = ticket.SoLuong,
                        TongTien = ticket.GiaVe * ticket.SoLuong,
                        TenKhachHang = model.TenKhachHang,
                        Email = model.Email,
                        SoDienThoai = model.SoDienThoai,
                        NgayDat = DateTime.Now,
                        TrangThai = "ChoDuyet",
                        UserId = null // Guest checkout
                    };

                    _context.DonHangs.Add(donHang);

                    // Update ticket availability
                    loaiVe.SoLuongConLai -= ticket.SoLuong;
                    _context.LoaiVes.Update(loaiVe);

                    await _context.SaveChangesAsync();

                    orderIds.Add(donHang.Id);
                }

                return Ok(new { success = true, orderIds = orderIds });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // GET: DonHangs/Confirmation
        public async Task<IActionResult> Confirmation(string orderIds)
        {
            if (string.IsNullOrEmpty(orderIds))
            {
                return RedirectToAction("Index", "Home");
            }

            var ids = orderIds.Split(',').Select(int.Parse).ToList();

            var donHangs = await _context.DonHangs
                .Include(d => d.SuKien)
                    .ThenInclude(s => s.DiaDiem)
                .Include(d => d.LoaiVe)
                .Where(d => ids.Contains(d.Id))
                .ToListAsync();

            if (!donHangs.Any())
            {
                return NotFound();
            }

            var viewModel = new OrderConfirmationViewModel
            {
                OrderIds = ids,
                TenKhachHang = donHangs.First().TenKhachHang,
                Email = donHangs.First().Email,
                TongTien = donHangs.Sum(d => d.TongTien),
                SuKien = donHangs.First().SuKien,
                DonHangs = donHangs
            };

            return View(viewModel);
        }

        private bool DonHangExists(int id)
        {
            return _context.DonHangs.Any(e => e.Id == id);
        }
    }
}
