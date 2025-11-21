using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySuKien.Models;

[Table("LoaiVe")]
[Index("SuKienId", Name = "IX_LoaiVe_SuKienId")]
public partial class LoaiVe
{
    [Key]
    public int Id { get; set; }

    public int SuKienId { get; set; }

    [StringLength(50)]
    public string TenLoai { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal GiaVe { get; set; }

    public int TongSoLuong { get; set; }

    public int SoLuongConLai { get; set; }

    [InverseProperty("LoaiVe")]
    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    [ForeignKey("SuKienId")]
    [InverseProperty("LoaiVes")]
    public virtual SuKien SuKien { get; set; } = null!;
}
