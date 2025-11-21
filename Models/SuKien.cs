using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySuKien.Models;

[Table("SuKien")]
[Index("DiaDiemId", Name = "IX_SuKien_DiaDiemId")]
[Index("NgayToChuc", Name = "IX_SuKien_NgayToChuc")]
[Index("TrangThai", Name = "IX_SuKien_TrangThai")]
public partial class SuKien
{
    [Key]
    public int Id { get; set; }

    [StringLength(200)]
    public string TenSuKien { get; set; } = null!;

    [StringLength(50)]
    public string? LoaiSuKien { get; set; }

    public int DiaDiemId { get; set; }

    public DateOnly NgayToChuc { get; set; }

    public TimeOnly GioToChuc { get; set; }

    [StringLength(300)]
    public string? AnhBia { get; set; }

    [StringLength(1000)]
    public string? MoTa { get; set; }

    [StringLength(20)]
    public string TrangThai { get; set; } = null!;

    [ForeignKey("DiaDiemId")]
    [InverseProperty("SuKiens")]
    public virtual DiaDiem DiaDiem { get; set; } = null!;

    [InverseProperty("SuKien")]
    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    [InverseProperty("SuKien")]
    public virtual ICollection<LoaiVe> LoaiVes { get; set; } = new List<LoaiVe>();

    [ForeignKey("SuKienId")]
    [InverseProperty("SuKiens")]
    public virtual ICollection<NgheSi> NgheSis { get; set; } = new List<NgheSi>();
}
