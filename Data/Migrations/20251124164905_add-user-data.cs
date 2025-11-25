using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLySuKien.Data.Migrations
{
    /// <inheritdoc />
    public partial class adduserdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiaDiem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDiaDiem = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    SucChua = table.Column<int>(type: "int", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DiaDiem__3214EC071A2B6D73", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NgheSi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNgheSi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TheLoai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TieuSu = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AnhDaiDien = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NgheSi__3214EC07CD2682D5", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NgheSiSuKien",
                columns: table => new
                {
                    NgheSiId = table.Column<int>(type: "int", nullable: false),
                    SuKienId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NgheSiSuKien", x => new { x.NgheSiId, x.SuKienId });
                });

            migrationBuilder.CreateTable(
                name: "SuKien",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenSuKien = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LoaiSuKien = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DiaDiemId = table.Column<int>(type: "int", nullable: false),
                    NgayToChuc = table.Column<DateOnly>(type: "date", nullable: false),
                    GioToChuc = table.Column<TimeOnly>(type: "time", nullable: false),
                    AnhBia = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "SapDienRa")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SuKien__3214EC07FB8C7128", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuKien_DiaDiem",
                        column: x => x.DiaDiemId,
                        principalTable: "DiaDiem",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LoaiVe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SuKienId = table.Column<int>(type: "int", nullable: false),
                    TenLoai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GiaVe = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TongSoLuong = table.Column<int>(type: "int", nullable: false),
                    SoLuongConLai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LoaiVe__3214EC07EAA01E1C", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoaiVe_SuKien",
                        column: x => x.SuKienId,
                        principalTable: "SuKien",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuKien_NgheSi",
                columns: table => new
                {
                    SuKienId = table.Column<int>(type: "int", nullable: false),
                    NgheSiId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SuKien_N__976F770CBED139A5", x => new { x.SuKienId, x.NgheSiId });
                    table.ForeignKey(
                        name: "FK_SuKienNgheSi_NgheSi",
                        column: x => x.NgheSiId,
                        principalTable: "NgheSi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SuKienNgheSi_SuKien",
                        column: x => x.SuKienId,
                        principalTable: "SuKien",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonHang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SuKienId = table.Column<int>(type: "int", nullable: false),
                    LoaiVeId = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TenKhachHang = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    NgayDat = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    TrangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "ChoDuyet")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DonHang__3214EC07ADB7744C", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DonHang_LoaiVe",
                        column: x => x.LoaiVeId,
                        principalTable: "LoaiVe",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DonHang_SuKien",
                        column: x => x.SuKienId,
                        principalTable: "SuKien",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_LoaiVeId",
                table: "DonHang",
                column: "LoaiVeId");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_NgayDat",
                table: "DonHang",
                column: "NgayDat");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_SuKienId",
                table: "DonHang",
                column: "SuKienId");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_UserId",
                table: "DonHang",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LoaiVe_SuKienId",
                table: "LoaiVe",
                column: "SuKienId");

            migrationBuilder.CreateIndex(
                name: "IX_SuKien_DiaDiemId",
                table: "SuKien",
                column: "DiaDiemId");

            migrationBuilder.CreateIndex(
                name: "IX_SuKien_NgayToChuc",
                table: "SuKien",
                column: "NgayToChuc");

            migrationBuilder.CreateIndex(
                name: "IX_SuKien_TrangThai",
                table: "SuKien",
                column: "TrangThai");

            migrationBuilder.CreateIndex(
                name: "IX_SuKien_NgheSi_NgheSiId",
                table: "SuKien_NgheSi",
                column: "NgheSiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DonHang");

            migrationBuilder.DropTable(
                name: "NgheSiSuKien");

            migrationBuilder.DropTable(
                name: "SuKien_NgheSi");

            migrationBuilder.DropTable(
                name: "LoaiVe");

            migrationBuilder.DropTable(
                name: "NgheSi");

            migrationBuilder.DropTable(
                name: "SuKien");

            migrationBuilder.DropTable(
                name: "DiaDiem");
        }
    }
}
