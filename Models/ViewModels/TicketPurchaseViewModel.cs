using System.ComponentModel.DataAnnotations;

namespace QuanLySuKien.Models.ViewModels
{
    public class TicketPurchaseViewModel
    {
        public int SuKienId { get; set; }
        public List<TicketItemViewModel> Tickets { get; set; } = new();

        [Required]
        [StringLength(100)]
        public string TenKhachHang { get; set; } = null!;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(15)]
        public string SoDienThoai { get; set; } = null!;
    }

    public class TicketItemViewModel
    {
        public int LoaiVeId { get; set; }
        public int SoLuong { get; set; }
        public decimal GiaVe { get; set; }
    }

    public class OrderConfirmationViewModel
    {
        public List<int> OrderIds { get; set; } = new();
        public string TenKhachHang { get; set; } = null!;
        public string Email { get; set; } = null!;
        public decimal TongTien { get; set; }
        public SuKien? SuKien { get; set; }
        public List<DonHang> DonHangs { get; set; } = new();
    }
}
