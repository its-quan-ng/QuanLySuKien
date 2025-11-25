using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLySuKien.Data;
using QuanLySuKien.Models;
using QuanLySuKien.Models.ViewModels;
using Microsoft.AspNetCore.Antiforgery;
using QRCoder;
using OfficeOpenXml;

namespace QuanLySuKien.Controllers
{
    [Authorize] // All order operations require login
    public class DonHangsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DonHangsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: DonHangs - Only Admin can view all orders
        [Authorize(Roles = "Admin")]
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
                        UserId = User.Identity.IsAuthenticated ? _userManager.GetUserId(User) : null
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

        // GET: DonHangs/MyOrders - Show current user's orders
        public async Task<IActionResult> MyOrders()
        {
            var userId = _userManager.GetUserId(User);
            var userEmail = User.Identity?.Name;

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            // Get all orders for this user (match by UserId OR Email)
            var orders = await _context.DonHangs
                .Include(d => d.SuKien)
                    .ThenInclude(s => s.DiaDiem)
                .Include(d => d.LoaiVe)
                .Where(d => d.UserId == userId ||
                           (d.Email != null && userEmail != null && d.Email.ToLower() == userEmail.ToLower()))
                .OrderByDescending(d => d.NgayDat)
                .ToListAsync();

            ViewData["Title"] = "Vé Của Tôi";
            return View(orders);
        }

        // GET: DonHangs/GetQRCode/id
        public IActionResult GetQRCode(int id)
        {
            // Generate QR code data: Order ID + Event info
            var qrData = $"ORDER-{id}-EVENTHUB";

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q);
                using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
                {
                    byte[] qrCodeBytes = qrCode.GetGraphic(10);
                    return File(qrCodeBytes, "image/png");
                }
            }
        }

        // GET: DonHangs/ExportExcel
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExportExcel()
        {
            var orders = await _context.DonHangs
                .Include(d => d.SuKien)
                .Include(d => d.LoaiVe)
                .OrderByDescending(d => d.NgayDat)
                .ToListAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Orders");

                // Headers
                worksheet.Cells[1, 1].Value = "Order ID";
                worksheet.Cells[1, 2].Value = "Khách Hàng";
                worksheet.Cells[1, 3].Value = "Email";
                worksheet.Cells[1, 4].Value = "Số ĐT";
                worksheet.Cells[1, 5].Value = "Sự Kiện";
                worksheet.Cells[1, 6].Value = "Loại Vé";
                worksheet.Cells[1, 7].Value = "Số Lượng";
                worksheet.Cells[1, 8].Value = "Tổng Tiền";
                worksheet.Cells[1, 9].Value = "Trạng Thái";
                worksheet.Cells[1, 10].Value = "Ngày Đặt";

                // Style header
                using (var range = worksheet.Cells[1, 1, 1, 10])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(59, 130, 246));
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                }

                // Data
                int row = 2;
                foreach (var order in orders)
                {
                    worksheet.Cells[row, 1].Value = order.Id;
                    worksheet.Cells[row, 2].Value = order.TenKhachHang;
                    worksheet.Cells[row, 3].Value = order.Email;
                    worksheet.Cells[row, 4].Value = order.SoDienThoai;
                    worksheet.Cells[row, 5].Value = order.SuKien.TenSuKien;
                    worksheet.Cells[row, 6].Value = order.LoaiVe.TenLoai;
                    worksheet.Cells[row, 7].Value = order.SoLuong;
                    worksheet.Cells[row, 8].Value = order.TongTien;
                    worksheet.Cells[row, 9].Value = order.TrangThai;
                    worksheet.Cells[row, 10].Value = order.NgayDat.ToString("dd/MM/yyyy HH:mm");
                    row++;
                }

                worksheet.Cells.AutoFitColumns();

                var stream = new MemoryStream(package.GetAsByteArray());
                var fileName = $"Orders_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
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
