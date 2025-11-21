using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using QuanLySuKien.Models;

namespace QuanLySuKien.Data;

public partial class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DiaDiem> DiaDiems { get; set; }

    public virtual DbSet<DonHang> DonHangs { get; set; }

    public virtual DbSet<LoaiVe> LoaiVes { get; set; }

    public virtual DbSet<NgheSi> NgheSis { get; set; }

    public virtual DbSet<SuKien> SuKiens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Định nghĩa khóa chính cho IdentityUserLogin<string>
        modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.HasKey(l => new { l.LoginProvider, l.ProviderKey });
        });

        // Configure business entities
        modelBuilder.Entity<DiaDiem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DiaDiem__3214EC071A2B6D73");
        });

        modelBuilder.Entity<DonHang>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DonHang__3214EC07ADB7744C");

            entity.Property(e => e.NgayDat).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TrangThai).HasDefaultValue("ChoDuyet");

            entity.HasOne(d => d.LoaiVe).WithMany(p => p.DonHangs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DonHang_LoaiVe");

            entity.HasOne(d => d.SuKien).WithMany(p => p.DonHangs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DonHang_SuKien");
        });

        modelBuilder.Entity<LoaiVe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LoaiVe__3214EC07EAA01E1C");

            entity.HasOne(d => d.SuKien).WithMany(p => p.LoaiVes).HasConstraintName("FK_LoaiVe_SuKien");
        });

        modelBuilder.Entity<NgheSi>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NgheSi__3214EC07CD2682D5");
        });

        modelBuilder.Entity<SuKien>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SuKien__3214EC07FB8C7128");

            entity.Property(e => e.TrangThai).HasDefaultValue("SapDienRa");

            entity.HasOne(d => d.DiaDiem).WithMany(p => p.SuKiens)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SuKien_DiaDiem");

            entity.HasMany(d => d.NgheSis).WithMany(p => p.SuKiens)
                .UsingEntity<Dictionary<string, object>>(
                    "SuKienNgheSi",
                    r => r.HasOne<NgheSi>().WithMany()
                        .HasForeignKey("NgheSiId")
                        .HasConstraintName("FK_SuKienNgheSi_NgheSi"),
                    l => l.HasOne<SuKien>().WithMany()
                        .HasForeignKey("SuKienId")
                        .HasConstraintName("FK_SuKienNgheSi_SuKien"),
                    j =>
                    {
                        j.HasKey("SuKienId", "NgheSiId").HasName("PK__SuKien_N__976F770CBED139A5");
                        j.ToTable("SuKien_NgheSi");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
