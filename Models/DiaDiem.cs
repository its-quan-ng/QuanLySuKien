using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLySuKien.Models;

[Table("DiaDiem")]
public partial class DiaDiem
{
    [Key]
    public int Id { get; set; }

    [StringLength(200)]
    public string TenDiaDiem { get; set; } = null!;

    [StringLength(300)]
    public string DiaChi { get; set; } = null!;

    public int SucChua { get; set; }

    [StringLength(500)]
    public string? MoTa { get; set; }

    [InverseProperty("DiaDiem")]
    public virtual ICollection<SuKien> SuKiens { get; set; } = new List<SuKien>();
}
