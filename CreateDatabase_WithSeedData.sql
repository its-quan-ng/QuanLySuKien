
USE master;
GO

IF EXISTS (SELECT * FROM sys.databases WHERE name = 'QuanLySuKien')
BEGIN
    ALTER DATABASE QuanLySuKien SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QuanLySuKien;
END
GO

CREATE DATABASE QuanLySuKien;
GO

USE QuanLySuKien;
GO

-- =============================================
-- PHẦN 1: TẠO TABLES
-- =============================================

-- LƯU Ý: Bảng NguoiDung (AspNetUsers) sẽ được tạo tự động bởi ASP.NET Core Identity
-- khi chọn Individual Accounts authentication

-- 1. Bảng DiaDiem
CREATE TABLE DiaDiem (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TenDiaDiem NVARCHAR(200) NOT NULL,
    DiaChi NVARCHAR(300) NOT NULL,
    SucChua INT NOT NULL,
    MoTa NVARCHAR(500)
);
GO

-- 3. Bảng NgheSi
CREATE TABLE NgheSi (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TenNgheSi NVARCHAR(100) NOT NULL,
    TheLoai NVARCHAR(50),
    TieuSu NVARCHAR(1000),
    AnhDaiDien NVARCHAR(300)
);
GO

-- 4. Bảng SuKien
CREATE TABLE SuKien (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TenSuKien NVARCHAR(200) NOT NULL,
    LoaiSuKien NVARCHAR(50),
    DiaDiemId INT NOT NULL,
    NgayToChuc DATE NOT NULL,
    GioToChuc TIME NOT NULL,
    AnhBia NVARCHAR(300),
    MoTa NVARCHAR(1000),
    TrangThai NVARCHAR(20) NOT NULL DEFAULT 'SapDienRa' CHECK (TrangThai IN ('SapDienRa', 'DangDienRa', 'KetThuc', 'HuyBo')),
    CONSTRAINT FK_SuKien_DiaDiem FOREIGN KEY (DiaDiemId) REFERENCES DiaDiem(Id)
);
GO

-- 5. Bảng SuKien_NgheSi (Many-to-Many)
CREATE TABLE SuKien_NgheSi (
    SuKienId INT NOT NULL,
    NgheSiId INT NOT NULL,
    PRIMARY KEY (SuKienId, NgheSiId),
    CONSTRAINT FK_SuKienNgheSi_SuKien FOREIGN KEY (SuKienId) REFERENCES SuKien(Id) ON DELETE CASCADE,
    CONSTRAINT FK_SuKienNgheSi_NgheSi FOREIGN KEY (NgheSiId) REFERENCES NgheSi(Id) ON DELETE CASCADE
);
GO

-- 6. Bảng LoaiVe
CREATE TABLE LoaiVe (
    Id INT PRIMARY KEY IDENTITY(1,1),
    SuKienId INT NOT NULL,
    TenLoai NVARCHAR(50) NOT NULL,
    GiaVe DECIMAL(18,2) NOT NULL,
    TongSoLuong INT NOT NULL,
    SoLuongConLai INT NOT NULL,
    CONSTRAINT FK_LoaiVe_SuKien FOREIGN KEY (SuKienId) REFERENCES SuKien(Id) ON DELETE CASCADE,
    CONSTRAINT CHK_SoLuongConLai CHECK (SoLuongConLai >= 0 AND SoLuongConLai <= TongSoLuong)
);
GO

-- 6. Bảng DonHang
CREATE TABLE DonHang (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId NVARCHAR(450) NULL, -- FK to AspNetUsers.Id (Identity), NULL = khách vãng lai
    SuKienId INT NOT NULL,
    LoaiVeId INT NOT NULL,
    SoLuong INT NOT NULL,
    TongTien DECIMAL(18,2) NOT NULL,
    TenKhachHang NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    SoDienThoai NVARCHAR(15) NOT NULL,
    NgayDat DATETIME NOT NULL DEFAULT GETDATE(),
    TrangThai NVARCHAR(20) NOT NULL DEFAULT 'ChoDuyet' CHECK (TrangThai IN ('ChoDuyet', 'DaXacNhan', 'DaHuy')),
    -- FK to AspNetUsers sẽ được thêm sau khi Identity tạo tables
    -- CONSTRAINT FK_DonHang_AspNetUsers FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id),
    CONSTRAINT FK_DonHang_SuKien FOREIGN KEY (SuKienId) REFERENCES SuKien(Id),
    CONSTRAINT FK_DonHang_LoaiVe FOREIGN KEY (LoaiVeId) REFERENCES LoaiVe(Id),
    CONSTRAINT CHK_SoLuong CHECK (SoLuong > 0)
);
GO

-- Indexes
CREATE INDEX IX_SuKien_DiaDiemId ON SuKien(DiaDiemId);
CREATE INDEX IX_SuKien_NgayToChuc ON SuKien(NgayToChuc);
CREATE INDEX IX_SuKien_TrangThai ON SuKien(TrangThai);
CREATE INDEX IX_LoaiVe_SuKienId ON LoaiVe(SuKienId);
CREATE INDEX IX_DonHang_UserId ON DonHang(UserId);
CREATE INDEX IX_DonHang_SuKienId ON DonHang(SuKienId);
CREATE INDEX IX_DonHang_NgayDat ON DonHang(NgayDat);
GO

PRINT 'Tables đã được tạo thành công!';
GO

-- =============================================
-- PHẦN 2: SEED DATA
-- =============================================

-- LƯU Ý: Người dùng sẽ được tạo thông qua ASP.NET Core Identity
-- Có thể tạo users trong code (Seed data Identity) hoặc thông qua trang Register

-- 1. INSERT DiaDiem
INSERT INTO DiaDiem (TenDiaDiem, DiaChi, SucChua, MoTa) VALUES
(N'Nhà hát Lớn Hà Nội', N'01 Tràng Tiền, Hoàn Kiếm, Hà Nội', 1000, N'Nhà hát lịch sử với kiến trúc Pháp cổ điển'),
(N'Cung Văn hóa Hữu nghị', N'91 Trần Hưng Đạo, Hoàn Kiếm, Hà Nội', 800, N'Địa điểm tổ chức sự kiện văn hóa lớn'),
(N'Sân vận động Mỹ Đình', N'Từ Liêm, Hà Nội', 40000, N'Sân vận động quốc gia, phù hợp concert lớn'),
(N'The Opera House', N'Số 10, Đồng Khởi, Quận 1, TP.HCM', 500, N'Nhà hát Opera sang trọng tại Sài Gòn'),
(N'SECC Hall', N'799 Nguyễn Văn Linh, Quận 7, TP.HCM', 5000, N'Trung tâm hội chợ triển lãm lớn'),
(N'Nhà hát Hòa Bình', N'240 Điện Biên Phủ, Bình Thạnh, TP.HCM', 1200, N'Nhà hát đa năng hiện đại');
GO

-- 3. INSERT NgheSi
INSERT INTO NgheSi (TenNgheSi, TheLoai, TieuSu, AnhDaiDien) VALUES
-- Kpop Artists
(N'Red Velvet', N'Kpop/Pop/R&B', N'Nhóm nhạc nữ Hàn Quốc thuộc SM Entertainment. Nổi tiếng với concept "dual" kết hợp giữa Red (sôi động) và Velvet (mềm mại). Thành viên: Irene, Seulgi, Wendy, Joy, Yeri.', '/images/artists/red-velvet.jpg'),
(N'Bùi Lan Hương', N'VPop/Jazz/dream pop', N'Ca sĩ, diễn viên Việt Nam tham gia Đạp Gió 2024. Nhiều ca khúc hit như Ngày chưa giông bão, Mê muội, Bùa mê.', '/images/artists/chi-pu.jpg'),
(N'Taeyeon (SNSD) ', N'Kpop/R&B', N'Là một nữ ca sĩ người Hàn Quốc. Cô nổi tiếng với tư cách là thành viên hát chính và đội trưởng của nhóm nhạc nữ Hàn Quốc Girls' Generation với ca khúc I và các album nổi bật: My Voice, Purpose, INVU.', '/images/artists/taeyeon.jpg'),
(N'Ái Phương', N'Vpop', N'Là một nghệ sĩ đa năng của làng giải trí Việt Nam, hoạt động trong các lĩnh vực ca hát, sáng tác nhạc, đóng phim và làm MC', '/images/artists/ai-phuong.jpg'),
(N'Mỹ Tâm', N'Ballad/Pop', N'Nữ ca sĩ, nhạc sĩ được mệnh danh là "Họa mi tóc nâu". Có nhiều album thành công và giải thưởng danh giá.', '/images/artists/my-tam.jpg'),
(N'Phác Thụ (Pu Shu)', N'Pop-rock, Folk rock Trung Quốc', N'Ca sĩ kiêm nhạc sĩ người Trung Quốc nổi tiếng với phong cách âm nhạc sâu sắc, giàu chất thơ và kín tiếng trong đời tư, có các tác phẩm nổi bật như "Những bông hoa đó" và "Rừng bạch dương trắng".', N'/images/artists/pu-shu.jpg),
('Phan Mạnh Quỳnh', N'VPop/Pop/Ballad', 'Nhạc sĩ kiêm ca sĩ với sở trường sáng tác ca từ tự sự, sâu sắc, giai điệu nhẹ nhàng chạm đến cảm xúc người nghe, nổi tiếng với các hit "Vợ người ta", "Từ đó".', '/images/artists/phan-manh-quynh.jpg'),
(N'Hoàng Thùy Linh', N'Pop/EDM', N'Ca sĩ nữ với phong cách âm nhạc đương đại, kết hợp yếu tố dân gian hiện đại.', '/images/artists/hoang-thuy-linh.jpg'),
('Đông Nhi', N'Vpop/Pop/Dance', 'Ca sĩ nữ hàng đầu Việt Nam, nổi tiếng với nhiều ca khúc hit và phong cách trình diễn sôi động, từng giành giải MTV EMA 2016, có lượng người hâm mộ đông đảo.', '/images/artists/dong-nhi.jpg'),
('Bùi Công Nam', 'Pop/Ballad/Indie', 'Ca sĩ, nhạc sĩ tài năng được mệnh danh "Ông hoàng nhạc Tết", nổi tiếng với ca từ mộc mạc, gần gũi, truyền tải năng lượng tích cực qua các ca khúc hit như "Có không giữ mất đừng tìm", "Tết "phiền" vẫn iu".', '/images/artists/bui-cong-nam.jpg'
(N'Kim Taeri', N'Actress', N'Nữ diễn viên Hàn Quốc đình đám với các phim Twenty Five Twenty One, The Handmaiden, Little Forest. Được yêu mến bởi diễn xuất tự nhiên và nụ cười tỏa nắng.', '/images/artists/kim-taeri.jpg'),
(N'T-ARA', N'Kpop/Dance', N'Nhóm nhạc nữ huyền thoại Kpop thế hệ 2. Queen of retro concept với loạt hit đình đám: Roly Poly, Lovey Dovey, Bo Peep Bo Peep, Number 9. Fanbase mạnh tại Việt Nam.', '/images/artists/t-ara.jpg');
GO

-- 4. INSERT SuKien
INSERT INTO SuKien (TenSuKien, LoaiSuKien, DiaDiemId, NgayToChuc, GioToChuc, AnhBia, MoTa, TrangThai) VALUES
(N'Red Velvet "RED MARE" in Vietnam', N'Concert', 3, '2024-12-15', '19:00:00', '/images/events/red-mare.jpg',
 N'Tour concert đình đám RED MARE của Red Velvet lần đầu đến Việt Nam! Sân khấu hoành tráng với các hit như Queendom, Feel My Rhythm, Russian Roulette.', 'SapDienRa'),

(N'Red Velvet "R TO V" Fancon Vietnam', N'Fan Meeting', 1, '2024-12-20', '18:30:00', '/images/events/r-to-v.jpg',
 N'Fan meeting thân mật R TO V của Red Velvet. Cơ hội giao lưu, chơi game và xem các thành viên biểu diễn những ca khúc yêu thích của ReVeluv!', 'SapDienRa'),

(N'Tri Âm - Mỹ Tâm', N'Concert', 2, '2024-12-25', '19:00:00', '/images/events/tri-am-my-tam.jpg',
 N'Live concert Tri Âm của Mỹ Tâm với những ca khúc ballad hay nhất trong sự nghiệp.', 'SapDienRa'),

(N'Đạp Gió 2024 - Chị Đẹp Đạp Gió Rẽ Sóng', N'Concert', 4, '2024-12-10', '19:30:00', '/images/events/dap-gio-2024.jpg',
 N'Concert của các "Chị Đẹp" từ show Đạp Gió Rẽ Sóng! Sự góp mặt của Chi Pu, Ella (S.H.E), Twins, Joey Yung và nhiều nghệ sĩ đình đám châu Á.', 'SapDienRa'),

(N'Red Velvet "THE RED ROOM" Asia Tour', N'Concert', 5, '2025-01-05', '19:00:00', '/images/events/red-room.jpg',
 N'Concert THE RED ROOM - không gian âm nhạc đỏ rực với những bản hit đỉnh cao: Red Flavor, Psycho, Bad Boy, Dumb Dumb. Chuẩn bị cháy hết mình!', 'SapDienRa'),

(N'Seventeen "FOLLOW" World Tour in Vietnam', N'Concert', 6, '2024-12-30', '18:00:00', '/images/events/seventeen-follow.jpg',
 N'Tour thế giới FOLLOW của Seventeen ghé Việt Nam! 13 thành viên tài năng với vũ đạo đỉnh cao, live cực đỉnh. Carat Việt chuẩn bị lightstick nhé!', 'SapDienRa'),

(N'KPOP Music Festival Vietnam 2025', N'Festival', 5, '2025-01-15', '17:00:00', '/images/events/kpop-fest.jpg',
 N'Lễ hội Kpop lớn nhất Việt Nam với lineup khủng! Red Velvet, Seventeen và nhiều nghệ sĩ Kpop đình đám. 2 ngày bùng nổ với âm nhạc Hàn Quốc.', 'SapDienRa'),

(N'Red Velvet "LA ROUGE" Special Stage', N'Fan Meeting', 4, '2024-12-18', '19:30:00', '/images/events/la-rouge.jpg',
 N'Sân khấu đặc biệt LA ROUGE - concept sang chảnh với dress code đỏ. Red Velvet sẽ trình diễn các bản ballad và R&B đầy cảm xúc như Kingdom Come, Automatic.', 'SapDienRa'),

(N'Kim Tae Ri "2521 Days" Fan Meeting Vietnam', N'Fan Meeting', 2, '2025-01-20', '14:00:00', '/images/events/kim-tae-ri-fanmeeting.jpg',
 N'Lần đầu tiên Kim Tae Ri đến Việt Nam! Cơ hội gặp gỡ nữ diễn viên Twenty Five Twenty One, xem talk show chia sẻ về cuộc sống và sự nghiệp, chơi game cùng idol. Fan meeting trong mơ của mọi fan Kdrama!', 'SapDienRa'),

(N'T-ara "Queens Are Back" Vietnam Fan Meeting', N'Fan Meeting', 6, '2025-02-01', '18:00:00', '/images/events/t-ara-queens.jpg',
 N'T-ara comeback Việt Nam! "Queens Are Back" với mini concert đặc biệt - biểu diễn loạt hit huyền thoại Roly Poly, Lovey Dovey, Day By Day. Tương tác, chụp hình và nhận quà từ 6 thành viên. T-ara Việt Nam đã chờ đợi quá lâu rồi!', 'SapDienRa');
GO

-- 5. INSERT SuKien_NgheSi (Gắn nghệ sĩ vào sự kiện)
INSERT INTO SuKien_NgheSi (SuKienId, NgheSiId) VALUES
-- Red Velvet "RED MARE": Red Velvet
(1, 1),

-- Red Velvet "R TO V" Fancon: Red Velvet
(2, 1),

-- Tri Âm: Mỹ Tâm
(3, 5),

-- Đạp Gió 2024: Chi Pu, Ella, Twins, Joey Yung
(4, 2),
(4, 4),
(4, 6),
(4, 10),

-- Red Velvet "THE RED ROOM": Red Velvet
(5, 1),

-- Seventeen "FOLLOW": Seventeen
(6, 9),

-- KPOP Music Festival: Red Velvet, Seventeen, Noo Phước Thịnh, Hoàng Thùy Linh
(7, 1),
(7, 9),
(7, 7),
(7, 8),

-- Red Velvet "LA ROUGE": Red Velvet
(8, 1),

-- Kim Tae Ri "2521 Days" Fan Meeting: Kim Tae Ri
(9, 11),

-- T-ara "Queens Are Back": T-ara
(10, 12);
GO

-- 6. INSERT LoaiVe
-- Red Velvet "RED MARE"
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(1, N'VVIP Hi-Touch', 3500000, 50, 50),
(1, N'VIP', 2500000, 200, 200),
(1, N'Standing', 1200000, 800, 800),
(1, N'Seated', 900000, 500, 500);

-- Red Velvet "R TO V" Fancon
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(2, N'VVIP + Photo', 2800000, 60, 60),
(2, N'VIP', 1800000, 150, 150),
(2, N'General', 1200000, 300, 300);

-- Tri Âm - Mỹ Tâm
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(3, N'VIP', 1500000, 100, 100),
(3, N'Thường', 600000, 400, 400);

-- Đạp Gió 2024
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(4, N'VVIP Meet & Greet', 4000000, 30, 30),
(4, N'VIP', 2200000, 80, 80),
(4, N'ReVeLuv Zone', 1500000, 150, 150),
(4, N'Thường', 800000, 200, 200);

-- Red Velvet "THE RED ROOM"
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(5, N'VVIP Soundcheck', 4500000, 80, 80),
(5, N'VIP', 3000000, 300, 300),
(5, N'Standing A', 1500000, 1000, 1000),
(5, N'Seated', 1000000, 800, 800);

-- Seventeen "FOLLOW"
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(6, N'VVIP Carat Zone', 4000000, 100, 100),
(6, N'VIP', 2800000, 250, 250),
(6, N'Standing', 1500000, 600, 600);

-- KPOP Music Festival
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(7, N'VIP 2 Ngày', 5000000, 200, 200),
(7, N'Standing 2 Ngày', 2000000, 1500, 1500),
(7, N'Seated 1 Ngày', 1200000, 800, 800);

-- Red Velvet "LA ROUGE"
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(8, N'VVIP Red Carpet', 3200000, 40, 40),
(8, N'VIP', 2000000, 120, 120),
(8, N'General', 1200000, 300, 300);

-- Kim Tae Ri "2521 Days" Fan Meeting
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(9, N'VVIP Polaroid + Sign', 2500000, 50, 50),
(9, N'VIP Hi-Touch', 1800000, 100, 100),
(9, N'Premium Seat', 1200000, 200, 200),
(9, N'Standard', 800000, 400, 400);

-- T-ara "Queens Are Back"
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(10, N'VVIP Group Photo', 3500000, 40, 40),
(10, N'VIP Hi-Touch', 2200000, 120, 120),
(10, N'Fan Zone', 1500000, 300, 300),
(10, N'General', 900000, 500, 500);
GO

-- 7. INSERT DonHang (Một vài đơn hàng mẫu - tất cả là khách vãng lai)
-- Sau khi tạo users qua Identity, có thể cập nhật UserId cho các đơn hàng này
INSERT INTO DonHang (UserId, SuKienId, LoaiVeId, SoLuong, TongTien, TenKhachHang, Email, SoDienThoai, TrangThai) VALUES
-- Khách vãng lai đặt vé Red Velvet RED MARE Standing (LoaiVeId = 3)
(NULL, 1, 3, 2, 2400000, N'Nguyễn Thùy Linh', 'thuylinh.reveluv@gmail.com', '0912345678', 'DaXacNhan'),

-- Khách vãng lai đặt vé R TO V Fancon VIP (LoaiVeId = 6)
(NULL, 2, 6, 2, 3600000, N'Trần Minh Anh', 'minhanh.rv@gmail.com', '0923456789', 'DaXacNhan'),

-- Khách vãng lai đặt vé Đạp Gió VIP (LoaiVeId = 11)
(NULL, 4, 11, 2, 4400000, N'Phạm Thảo Nguyên', 'thaonguyen@gmail.com', '0945678901', 'ChoDuyet'),

-- Khách vãng lai đặt vé KPOP Fest Standing 2 Ngày (LoaiVeId = 22)
(NULL, 7, 22, 3, 6000000, N'Lê Khánh Linh', 'khanhlinh.kpop@gmail.com', '0934567890', 'DaXacNhan'),

-- Khách vãng lai đặt vé LA ROUGE General (LoaiVeId = 26)
(NULL, 8, 26, 2, 2400000, N'Võ Hương Giang', 'huonggiang.rv@gmail.com', '0956789012', 'DaXacNhan'),

-- Khách vãng lai đặt vé RED ROOM VIP (LoaiVeId = 15)
(NULL, 5, 15, 1, 3000000, N'Đỗ Thanh Tâm', 'thanhtam.reveluv@gmail.com', '0967890123', 'DaXacNhan'),

-- Khách vãng lai đặt vé Seventeen VVIP Carat Zone (LoaiVeId = 18)
(NULL, 6, 18, 1, 4000000, N'Bùi Ngọc Ánh', 'ngocanh.carat@gmail.com', '0978901234', 'ChoDuyet'),

-- Khách vãng lai đặt vé Kim Tae Ri VVIP Polaroid (LoaiVeId = 27)
(NULL, 9, 27, 2, 5000000, N'Hoàng Thu Hà', 'thuha.2521@gmail.com', '0989012345', 'DaXacNhan'),

-- Khách vãng lai đặt vé T-ara VIP Hi-Touch (LoaiVeId = 32)
(NULL, 10, 32, 3, 6600000, N'Phan Quỳnh Anh', 'quynhanh.tara@gmail.com', '0990123456', 'DaXacNhan');
GO


 ALTER TABLE DonHang
  ADD CONSTRAINT FK_DonHang_AspNetUsers
  FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)
  ON DELETE SET NULL;  -- Khi xóa user, UserId trong DonHang = NULL (khách vãng lai)
  GO

