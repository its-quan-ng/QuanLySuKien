using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySuKien.Models;

[Table("NgheSi")]
public partial class NgheSi
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string TenNgheSi { get; set; } = null!;

    [StringLength(50)]
    public string? TheLoai { get; set; }

    [StringLength(1000)]
    public string? TieuSu { get; set; }

    [StringLength(300)]
    public string? AnhDaiDien { get; set; }

    [ForeignKey("NgheSiId")]
    [InverseProperty("NgheSis")]
    public virtual ICollection<SuKien> SuKiens { get; set; } = new List<SuKien>();
}
