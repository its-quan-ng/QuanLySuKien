using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySuKien.Models;

[Table("DonHang")]
[Index("NgayDat", Name = "IX_DonHang_NgayDat")]
[Index("SuKienId", Name = "IX_DonHang_SuKienId")]
[Index("UserId", Name = "IX_DonHang_UserId")]
public partial class DonHang
{
    [Key]
    public int Id { get; set; }

    public string? UserId { get; set; }

    public int SuKienId { get; set; }

    public int LoaiVeId { get; set; }

    public int SoLuong { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TongTien { get; set; }

    [StringLength(100)]
    public string TenKhachHang { get; set; } = null!;

    [StringLength(100)]
    public string Email { get; set; } = null!;

    [StringLength(15)]
    public string SoDienThoai { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime NgayDat { get; set; }

    [StringLength(20)]
    public string TrangThai { get; set; } = null!;

    [ForeignKey("LoaiVeId")]
    [InverseProperty("DonHangs")]
    public virtual LoaiVe LoaiVe { get; set; } = null!;

    [ForeignKey("SuKienId")]
    [InverseProperty("DonHangs")]
    public virtual SuKien SuKien { get; set; } = null!;
}
