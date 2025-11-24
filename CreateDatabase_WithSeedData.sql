
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
(N'Nhà hát Hòa Bình', N'240 Điện Biên Phủ, Bình Thạnh, TP.HCM', 1200, N'Nhà hát đa năng hiện đại'),
(N'Đồng Dao Tea House', N'38 Nguyễn Huệ, Quận 1, TP.HCM', 200, N'Không gian live music ấm cúng, sang trọng'),
(N'Youth Cultural House', N'04 Phạm Ngọc Thạch, Quận 3, TP.HCM', 600, N'Trung tâm văn hóa thanh niên với sân khấu hiện đại'),
(N'Vinhomes Ocean Park 3', N'Vinhomes Ocean Park 3, Hưng Yên', 15000, N'Khu đô thị hiện đại với không gian tổ chức sự kiện lớn, phù hợp với concert quy mô khủng');
GO

-- 2. INSERT NgheSi (ĐÃ SỬA LỖI SYNTAX)
INSERT INTO NgheSi (TenNgheSi, TheLoai, TieuSu, AnhDaiDien) VALUES
-- Kpop Artists
(N'Red Velvet', N'Kpop/Pop/R&B', N'Nhóm nhạc nữ Hàn Quốc thuộc SM Entertainment. Nổi tiếng với concept "dual" kết hợp giữa Red (sôi động) và Velvet (mềm mại). Thành viên: Irene, Seulgi, Wendy, Joy, Yeri. Các hit: Psycho, Red Flavor, Feel My Rhythm, Queendom.', N'/images/artists/red-velvet.jpg'),

(N'Taeyeon', N'Kpop/R&B/Ballad', N'Kim Tae-yeon - Nữ ca sĩ solo hàng đầu Kpop, leader và main vocal của Girls'' Generation (SNSD). Giọng ca quyền lực với loạt album đỉnh cao: My Voice, Purpose, INVU. Tour concert The Tense 2025 thành công rực rỡ khắp châu Á. Các hit: I, Weekend, INVU, Fine.', N'/images/artists/taeyeon.jpg'),

(N'T-ARA', N'Kpop/Dance/Retro', N'Nhóm nhạc nữ huyền thoại Kpop thế hệ 2. Queen of retro concept với loạt hit đình đám: Roly Poly, Lovey Dovey, Bo Peep Bo Peep, Number 9, Day By Day. Fanbase cực mạnh tại Việt Nam. Comeback 2024 với sự chào đón nồng nhiệt.', N'/images/artists/t-ara.jpg'),

-- Vpop Artists
(N'Bùi Lan Hương', N'Dream Pop/Indie/Jazz', N'Ca sĩ, nhạc sĩ Việt Nam - "Nữ hoàng Dream Pop". Ra mắt từ Sing My Song 2016, album đầu tay "Thiên Thần Sa Ngã" được đề cử Album của năm tại Cống Hiến 2019. Các hit: Bùa Mê, Ngày Chưa Giông Bão, Mặt Trăng, Kiều Mệnh Khúc, Đóa Bạch Trà. Tham gia Đạp Gió 2024.', N'/images/artists/bui-lan-huong.jpg'),

(N'Mỹ Tâm', N'Pop/Ballad/R&B', N'Họa mi tóc nâu - Diva hàng đầu Vbiz. Sự nghiệp 20+ năm với vô số giải thưởng, album bạch kim. Các hit bất hủ: Ước Gì, Như Một Giấc Mơ, Hẹn Em Ở Lần Yêu Thứ 2. Live concert Tri Âm - Trở Về là chuỗi concert ấn tượng nhất sự nghiệp.', N'/images/artists/my-tam.jpg'),

(N'Phan Mạnh Quỳnh', N'Pop/Ballad/Acoustic', N'Nhạc sĩ, ca sĩ tài năng với khả năng sáng tác ca từ tự sự chạm đến trái tim người nghe. Hit triệu view: Vợ Người Ta, Có Chàng Trai Viết Lên Cây, Ngày Chưa Giông Bão (collab Bùi Lan Hương). Được mệnh danh "Ông hoàng nhạc phim" với loạt OST đình đám.', N'/images/artists/phan-manh-quynh.jpg'),

(N'Ái Phương', N'Pop/R&B/Soul', N'Ca sĩ, nhạc sĩ, diễn viên đa tài. Á quân Vietnam Supermodel 2011. Sở hữu giọng hát nội lực với các ca khúc tự sáng tác như Trót Yêu (Trung Quân), Lỗi Ở Yêu Thương (Thanh Duy), Ích Kỷ. Live concert "Ký Ức Tình Yêu" thành công tại TP.HCM.', N'/images/artists/ai-phuong.jpg'),

(N'Đông Nhi', N'Pop/Dance/EDM', N'Ca sĩ hàng đầu Vpop với sự nghiệp 15+ năm. 6 năm liên tiếp (2011-2016) giành giải Ca sĩ nữ được yêu thích nhất Zing Music Awards. Giải thưởng MTV EMA 2016 "Best Southeast Asian Act". Các hit: Taxi, Bad Boy, 1234, We Belong Together. Phong cách trình diễn sôi động, bùng nổ.', N'/images/artists/dong-nhi.jpg'),

(N'Hoàng Thùy Linh', N'Pop/EDM/World Music', N'Ca sĩ, diễn viên với phong cách âm nhạc độc đáo kết hợp văn hóa truyền thống và hiện đại. Đình đám với See Tình, Để Mị Nói Cho Mà Nghe, Kẻ Cắp Gặp Bà Già. Album LINK thu hút hàng triệu lượt xem. Là gương mặt dẫn đầu xu hướng V-pop đương đại.', N'/images/artists/hoang-thuy-linh.jpg'),

(N'Bùi Công Nam', N'Pop/Indie/Tết Music', N'Ca sĩ, nhạc sĩ, producer - "Ông hoàng nhạc Tết" của Vpop. Nổi lên từ Sing My Song 2016. Thành viên nhóm B.O.F (Anh Trai Vượt Ngàn Chông Gai 2024). Các hit: Có Không Giữ Mất Đừng Tìm, Tết Này Con Sẽ Về, Tết Phiền Vẫn Iu, Tết Nhà Là Vô Giá. Sáng tác hơn 7 ca khúc Tết hit trong mùa Tết 2025.', N'/images/artists/bui-cong-nam.jpg'),

-- International Artists
(N'Phác Thụ (Pu Shu)', N'Folk Rock/Alternative', N'朴树 - Ca sĩ, nhạc sĩ Trung Quốc huyền thoại với phong cách âm nhạc sâu sắc, giàu chất thơ. Nổi tiếng kín tiếng trong đời tư, chỉ xuất hiện vì đam mê âm nhạc thuần túy. Các tác phẩm kinh điển: Những Bông Hoa Đó (那些花儿), Rừng Bạch Dương Trắng (白桦林), Con Đường Bình Thường (平凡之路). Tour "好好地II" đình đám 2017-2019.', N'/images/artists/pu-shu.jpg'),

-- Actresses/Celebrities
(N'Kim Tae-ri', N'Actress/Celebrity', N'Nữ diễn viên hàng đầu Hàn Quốc sinh năm 1990. Nổi tiếng toàn cầu với Twenty Five Twenty One (vai Na Hee-do), The Handmaiden, Little Forest, Mr. Sunshine. Được yêu mến bởi diễn xuất tự nhiên, chân thật và nụ cười tỏa nắng. Nhiều giải thưởng danh giá: Blue Dragon, Baeksang, Grand Bell.', N'/images/artists/kim-taeri.jpg');
GO

-- 3. INSERT SuKien (CẬP NHẬT MỚI - PHÙ HỢP VỚI NGHỆ SĨ)
INSERT INTO SuKien (TenSuKien, LoaiSuKien, DiaDiemId, NgayToChuc, GioToChuc, AnhBia, MoTa, TrangThai) VALUES
-- Red Velvet Events
(N'Red Velvet "REDMARE" in Vietnam', N'Concert', 3, '2025-03-15', '19:00:00', N'/images/events/red-mare.jpg',
 N'Lần đầu tiên Red Velvet mang tour concert đình đám REDMARE đến Việt Nam! Sân khấu hoành tráng với đầy đủ các hit: Psycho, Red Flavor, Feel My Rhythm, Queendom, Russian Roulette, Bad Boy. Dress code: RED! Chuẩn bị lightstick và hát hết mình cùng 5 cô gái tài năng nhé ReVeluv!', N'SapDienRa'),

(N'Red Velvet "R TO V" Fancon Vietnam', N'Fan Meeting', 6, '2025-03-18', '18:30:00', N'/images/events/r-to-v.jpg',
 N'Fan meeting thân mật "R TO V" của Red Velvet tại Việt Nam! Giao lưu trực tiếp, chơi game vui nhộn, xem các thành viên biểu diễn cover những ca khúc yêu thích. Cơ hội vàng để gần gũi với Irene, Seulgi, Wendy, Joy, Yeri!', N'SapDienRa'),

-- Taeyeon Events
(N'Taeyeon "The Tense" Concert in Vietnam', N'Concert', 5, '2025-04-10', '19:00:00', N'/images/events/taeyeon-tense.jpg',
 N'Tour concert "The Tense" của Taeyeon chính thức đổ bộ Việt Nam! Main vocal huyền thoại của SNSD sẽ mang đến những bản ballad đầy cảm xúc: I, INVU, Fine, Weekend, cùng loạt hit từ album My Voice, Purpose. Một đêm nhạc đỉnh cao với giọng ca quyền lực nhất Kpop!', N'SapDienRa'),

(N'Taeyeon "Weekend" Special Fan Meeting', N'Fan Meeting', 2, '2025-04-12', '15:00:00', N'/images/events/taeyeon-weekend.jpg',
 N'Fan meeting đặc biệt "Weekend" của Taeyeon! Không khí thân mật với các hoạt động: Talk show chia sẻ về sự nghiệp, hát live acoustic, hi-touch, ký tặng album. Limited seats - cơ hội duy nhất gặp "Tae Tae" tại Việt Nam!', N'SapDienRa'),

-- T-ARA Events
(N'T-ARA "Queens Are Back" Vietnam Concert', N'Concert', 5, '2025-05-01', '18:30:00', N'/images/events/t-ara-queens.jpg',
 N'T-ARA COMEBACK VIỆT NAM! Mini concert "Queens Are Back" với full setlist huyền thoại: Roly Poly, Lovey Dovey, Bo Peep Bo Peep, Day By Day, Number 9, Cry Cry. 6 thành viên hội tụ mang đến đêm nhạc retro dance bùng nổ. T-ara và fan Việt đã chờ đợi quá lâu rồi!', N'SapDienRa'),

-- Bùi Lan Hương Events
(N'Bùi Lan Hương "Thiên Thần Sa Ngã" Concert', N'Concert', 6, '2025-02-20', '20:00:00', N'/images/events/thien-than-sa-nga.jpg',
 N'Live concert "Thiên Thần Sa Ngã" - hành trình âm nhạc dream pop của Bùi Lan Hương. Đắm chìm trong không gian mộng mơ với Bùa Mê, Ngày Chưa Giông Bão, Mặt Trăng, Kiều Mệnh Khúc, Đóa Bạch Trà. Album được đề cử Cống Hiến 2019 lần đầu được trình diễn trọn vẹn trên sân khấu.', N'SapDienRa'),

-- Mỹ Tâm Events
(N'Mỹ Tâm "Tri Âm - Trở Về" Concert', N'Concert', 2, '2025-04-25', '19:30:00', N'/images/events/tri-am-my-tam.jpg',
 N'Live concert "Tri Âm - Trở Về" của Họa mi tóc nâu Mỹ Tâm. Chuỗi concert đình đám với những ballad kinh điển nhất sự nghiệp 20+ năm: Ước Gì, Như Một Giấc Mơ, Hẹn Em Ở Lần Yêu Thứ 2, Chuyện Như Chưa Bắt Đầu. Sân khấu hoành tráng, live band đỉnh cao. Một đêm nhạc không thể bỏ lỡ!', N'SapDienRa'),

-- Phan Mạnh Quỳnh Events
(N'Phan Mạnh Quỳnh "Vợ Người Ta" Acoustic Night', N'Acoustic Live', 7, '2025-03-08', '19:30:00', N'/images/events/vo-nguoi-ta.jpg',
 N'Đêm nhạc acoustic đầm ấm "Vợ Người Ta" cùng nhạc sĩ Phan Mạnh Quỳnh. Không gian gần gũi với guitar và piano, những ca khúc chạm đến trái tim: Vợ Người Ta, Có Chàng Trai Viết Lên Cây, Ngày Chưa Giông Bão, Người Yêu Cũ. Giới hạn 200 chỗ ngồi tại Đồng Dao Tea House - đặt vé ngay!', N'SapDienRa'),

-- Ái Phương Events
(N'Ái Phương "Ký Ức Tình Yêu" Live Concert', N'Concert', 7, '2025-02-14', '20:00:00', N'/images/events/ky-uc-tinh-yeu.jpg',
 N'Live concert "Ký Ức Tình Yêu" đặc biệt nhân dịp Valentine của Ái Phương. Những ca khúc tự sáng tác đầy cảm xúc về tình yêu và kỷ niệm. Không gian ấm cúng, lãng mạn tại Đồng Dao Tea House. Đêm nhạc dành tặng những trái tim đang yêu và đã yêu.', N'SapDienRa'),

-- Đông Nhi Events
(N'Đông Nhi "10 Years of Love" Concert', N'Concert', 5, '2025-06-15', '19:00:00', N'/images/events/10-years-dong-nhi.jpg',
 N'Đại nhạc hội "10 Years of Love" kỷ niệm 10 năm đỉnh cao sự nghiệp của Đông Nhi. Bùng nổ với loạt hit dance: Taxi, Bad Boy, 1234, We Belong Together. Sân khấu hoành tráng, visual đỉnh cao, vũ đạo bùng nổ. Đông Nhi sẽ mang đến một đêm nhạc sôi động chưa từng có!', N'SapDienRa'),

-- Hoàng Thùy Linh Events
(N'Hoàng Thùy Linh "LINK" Asia Tour - Vietnam', N'Concert', 5, '2025-05-20', '19:30:00', N'/images/events/link-htl.jpg',
 N'Đêm nhạc "LINK" Asia Tour của Hoàng Thùy Linh - show diễn kết nối văn hóa Á Đông và âm nhạc đương đại. Setlist đỉnh cao: See Tình, Để Mị Nói Cho Mà Nghe, Kẻ Cắp Gặp Bà Già, Bánh Trôi Nước. Sân khấu visual art đẳng cấp quốc tế với công nghệ mapping 3D.', N'SapDienRa'),

-- Bùi Công Nam Events
(N'Bùi Công Nam "Tết Phiền Vẫn Iu" Concert', N'Concert', 8, '2025-01-25', '19:00:00', N'/images/events/tet-phien-van-iu.jpg',
 N'Live concert Tết đặc biệt "Tết Phiền Vẫn Iu" cùng Ông hoàng nhạc Tết Bùi Công Nam! Không khí Tết rộn ràng với loạt hit: Tết Này Con Sẽ Về, Tết Nhà Là Vô Giá, Có Không Giữ Mất Đừng Tìm, Tết Ổn Rồi. Còn gì tuyệt hơn đón Tết cùng những giai điệu ấm áp, gần gũi!', N'SapDienRa'),

(N'Bùi Công Nam & B.O.F "Anh Trai" Reunion Concert', N'Concert', 6, '2025-07-10', '19:30:00', N'/images/events/bof-reunion.jpg',
 N'Concert reunion của nhóm B.O.F từ Anh Trai Vượt Ngàn Chông Gai! Bùi Công Nam cùng Jun Phạm, Kay Trần, BB Trần, S.T Sơn Thạch tái hiện những màn trình diễn bùng nổ. Kết hợp giữa nhạc Tết vui tươi và các ca khúc hit từ show. Đêm nhạc đầy năng lượng tích cực!', N'SapDienRa'),

-- Pu Shu (Phác Thụ) Events
(N'Phác Thụ (Pu Shu) "Bạch Dương Trắng" Vietnam Concert', N'Concert', 2, '2025-08-15', '20:00:00', N'/images/events/pu-shu-concert.jpg',
 N'Lần đầu tiên huyền thoại nhạc Hoa Phác Thụ (朴树) đến Việt Nam! Concert "Bạch Dương Trắng" với những tác phẩm kinh điển đầy chất thơ: Những Bông Hoa Đó, Rừng Bạch Dương Trắng, Con Đường Bình Thường. Đêm nhạc folk rock sâu lắng cho những tâm hồn yêu âm nhạc chân thật.', N'SapDienRa'),

-- Kim Tae-ri Events
(N'Kim Tae-ri "2521 Days" Fan Meeting Vietnam', N'Fan Meeting', 2, '2025-06-01', '14:00:00', N'/images/events/kim-taeri-fanmeeting.jpg',
 N'Fan meeting trong mơ với Kim Tae-ri lần đầu đến Việt Nam! "2521 Days" với talk show chia sẻ về Twenty Five Twenty One, The Handmaiden, Little Forest. Chơi game cùng nữ diễn viên, hi-touch, ký tặng polaroid. Cơ hội gặp gỡ Na Hee-do ngoài đời thật - mọi fan Kdrama đều phải đến!', N'SapDienRa'),

-- Multi-Artist Events
(N'Đạp Gió 2024 - "Chị Đẹp" Vietnam Concert', N'Concert', 3, '2025-04-05', '19:00:00', N'/images/events/dap-gio-2024.jpg',
 N'Đại nhạc hội "Chị Đẹp Đạp Gió Rẽ Sóng 2024" với sự góp mặt của Bùi Lan Hương cùng dàn chị đẹp đình đám: Ella (S.H.E), Twins, Joey Yung, Gillian Chung. 3 giờ bùng nổ với các stage đầy năng lượng. Đêm nhạc của sự tự tin và quyền năng phụ nữ!', N'SapDienRa'),

(N'Vpop Legends "Những Diva Vàng" Concert', N'Concert', 3, '2025-07-20', '19:00:00', N'/images/events/vpop-legends.jpg',
 N'Concert "Những Diva Vàng" quy tụ Mỹ Tâm, Đông Nhi, Hoàng Thùy Linh cùng sân khấu. Đêm nhạc tri ân những huyền thoại Vpop với loạt hit bất hủ. 3 thế hệ ca sĩ, 3 phong cách âm nhạc khác biệt hòa quyện trong một đêm diễn hoành tráng. Sự kiện âm nhạc không thể bỏ lỡ của năm!', N'SapDienRa'),

(N'V-Music Festival 2025', N'Festival', 3, '2025-09-01', '17:00:00', N'/images/events/v-music-fest.jpg',
 N'Lễ hội âm nhạc Việt Nam lớn nhất năm 2025! Lineup khủng: Mỹ Tâm, Đông Nhi, Hoàng Thùy Linh, Bùi Lan Hương, Phan Mạnh Quỳnh, Ái Phương, Bùi Công Nam. 2 ngày 2 đêm với 5 sân khấu khác nhau: Pop Stage, Indie Stage, EDM Stage, Acoustic Corner, Tết Corner. Đại tiệc âm nhạc cho mọi thế hệ!', N'SapDienRa'),

-- Y-Concert by Yeah1
(N'Y-Concert by Yeah1 "Mình Đoàn Viên Thôi"', N'Concert', 9, '2025-12-20', '19:00:00', N'/images/events/y-concert.jpg',
 N'Đại nhạc hội Y-Concert của Yeah1 Production tại Vinhomes Ocean Park 3! Lineup toàn sao đỉnh cao với hơn 50 nghệ sĩ: Bùi Lan Hương, Ái Phương, Bùi Công Nam cùng dàn sao khủng từ Anh Trai Vượt Ngàn Chông Gai, Anh Trai Say Hi, Chị Đẹp Đạp Gió. Chủ đề "Mình Đoàn Viên Thôi" - concert cuối năm hoành tráng nhất 2025!', N'SapDienRa');
GO

-- 4. INSERT SuKien_NgheSi (Gắn nghệ sĩ vào sự kiện)
INSERT INTO SuKien_NgheSi (SuKienId, NgheSiId) VALUES
-- Event 1: Red Velvet "REDMARE" in Vietnam
(1, 1),

-- Event 2: Red Velvet "R TO V" Fancon Vietnam
(2, 1),

-- Event 3: Taeyeon "The Tense" Concert in Vietnam
(3, 2),

-- Event 4: Taeyeon "Weekend" Special Fan Meeting
(4, 2),

-- Event 5: T-ARA "Queens Are Back" Vietnam Concert
(5, 3),

-- Event 6: Bùi Lan Hương "Thiên Thần Sa Ngã" Concert
(6, 4),

-- Event 7: Mỹ Tâm "Tri Âm - Trở Về" Concert
(7, 5),

-- Event 8: Phan Mạnh Quỳnh "Vợ Người Ta" Acoustic Night
(8, 6),

-- Event 9: Ái Phương "Ký Ức Tình Yêu" Live Concert
(9, 7),

-- Event 10: Đông Nhi "10 Years of Love" Concert
(10, 8),

-- Event 11: Hoàng Thùy Linh "LINK" Asia Tour - Vietnam
(11, 9),

-- Event 12: Bùi Công Nam "Tết Phiền Vẫn Iu" Concert
(12, 10),

-- Event 13: Bùi Công Nam & B.O.F "Anh Trai" Reunion Concert
(13, 10),

-- Event 14: Phác Thụ (Pu Shu) "Bạch Dương Trắng" Vietnam Concert
(14, 11),

-- Event 15: Kim Tae-ri "2521 Days" Fan Meeting Vietnam
(15, 12),

-- Event 16: Đạp Gió 2024 - "Chị Đẹp" Vietnam Concert (Bùi Lan Hương)
(16, 4),

-- Event 17: Vpop Legends "Những Diva Vàng" Concert (Mỹ Tâm, Đông Nhi, Hoàng Thùy Linh)
(17, 5),
(17, 8),
(17, 9),

-- Event 18: V-Music Festival 2025 (All Vpop artists)
(18, 4), -- Bùi Lan Hương
(18, 5), -- Mỹ Tâm
(18, 6), -- Phan Mạnh Quỳnh
(18, 7), -- Ái Phương
(18, 8), -- Đông Nhi
(18, 9), -- Hoàng Thùy Linh
(18, 10), -- Bùi Công Nam

-- Event 19: Y-Concert by Yeah1 (Bùi Lan Hương, Ái Phương, Bùi Công Nam + other artists)
(19, 4), -- Bùi Lan Hương
(19, 7), -- Ái Phương
(19, 10); -- Bùi Công Nam
GO

-- 5. INSERT LoaiVe
-- Event 1: Red Velvet "REDMARE"
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(1, N'VVIP Hi-Touch', 3500000, 50, 45),
(1, N'VIP', 2500000, 200, 180),
(1, N'Standing', 1200000, 800, 650),
(1, N'Seated', 900000, 500, 420);

-- Event 2: Red Velvet "R TO V" Fancon
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(2, N'VVIP + Photo', 2800000, 60, 55),
(2, N'VIP', 1800000, 150, 130),
(2, N'General', 1200000, 300, 250);

-- Event 3: Taeyeon "The Tense" Concert
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(3, N'VVIP Soundcheck', 4000000, 80, 70),
(3, N'VIP', 2800000, 250, 200),
(3, N'Standing A', 1500000, 600, 480),
(3, N'Seated', 1000000, 400, 320);

-- Event 4: Taeyeon "Weekend" Fan Meeting
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(4, N'VVIP Hi-Touch + Sign', 2500000, 50, 40),
(4, N'VIP Hi-Touch', 1800000, 100, 85),
(4, N'General', 1200000, 200, 150);

-- Event 5: T-ARA "Queens Are Back"
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(5, N'VVIP Group Photo', 3500000, 40, 30),
(5, N'VIP Hi-Touch', 2200000, 120, 95),
(5, N'Fan Zone', 1500000, 300, 250),
(5, N'General', 900000, 500, 400);

-- Event 6: Bùi Lan Hương "Thiên Thần Sa Ngã"
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(6, N'VIP Dream Pop', 1500000, 100, 75),
(6, N'Premium', 1000000, 200, 160),
(6, N'Standard', 600000, 400, 320);

-- Event 7: Mỹ Tâm "Tri Âm - Trở Về"
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(7, N'VIP', 1800000, 120, 95),
(7, N'Premium', 1200000, 300, 250),
(7, N'Standard', 700000, 400, 330);

-- Event 8: Phan Mạnh Quỳnh Acoustic Night
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(8, N'VIP Table', 800000, 40, 30),
(8, N'Premium Seat', 500000, 80, 65),
(8, N'Standard Seat', 300000, 80, 60);

-- Event 9: Ái Phương "Ký Ức Tình Yêu"
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(9, N'VIP Table for 2', 1000000, 30, 20),
(9, N'Premium Seat', 600000, 80, 60),
(9, N'Standard Seat', 350000, 90, 70);

-- Event 10: Đông Nhi "10 Years of Love"
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(10, N'VVIP Soundcheck', 3500000, 80, 65),
(10, N'VIP', 2000000, 250, 200),
(10, N'Standing', 1200000, 800, 650),
(10, N'Seated', 800000, 600, 480);

-- Event 11: Hoàng Thùy Linh "LINK"
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(11, N'VVIP Front Row', 3000000, 100, 80),
(11, N'VIP', 1800000, 300, 250),
(11, N'Standing', 1000000, 1000, 800),
(11, N'Seated', 700000, 800, 650);

-- Event 12: Bùi Công Nam "Tết Phiền Vẫn Iu"
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(12, N'VIP Family Pack (4 vé)', 2000000, 50, 35),
(12, N'Premium', 800000, 150, 120),
(12, N'Standard', 500000, 400, 320);

-- Event 13: B.O.F "Anh Trai" Reunion
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(13, N'VVIP Meet & Greet', 2500000, 60, 45),
(13, N'VIP', 1500000, 200, 160),
(13, N'Fan Zone', 900000, 400, 320),
(13, N'Standard', 600000, 500, 400);

-- Event 14: Pu Shu "Bạch Dương Trắng"
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(14, N'VIP', 1800000, 100, 75),
(14, N'Premium', 1200000, 250, 200),
(14, N'Standard', 800000, 450, 360);

-- Event 15: Kim Tae-ri Fan Meeting
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(15, N'VVIP Polaroid + Sign', 2500000, 50, 35),
(15, N'VIP Hi-Touch', 1800000, 100, 75),
(15, N'Premium Seat', 1200000, 200, 160),
(15, N'Standard', 800000, 450, 360);

-- Event 16: Đạp Gió 2024
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(16, N'VVIP Meet & Greet', 4000000, 30, 20),
(16, N'VIP', 2500000, 150, 120),
(16, N'Standing', 1200000, 1000, 800),
(16, N'Seated', 800000, 800, 650);

-- Event 17: Vpop Legends "Những Diva Vàng"
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(17, N'VVIP Golden Circle', 5000000, 100, 75),
(17, N'VIP', 3000000, 300, 240),
(17, N'Standing', 1500000, 1200, 950),
(17, N'Seated', 1000000, 1000, 800);

-- Event 18: V-Music Festival 2025
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(18, N'VIP All-Access 2 Ngày', 6000000, 200, 150),
(18, N'Standing 2 Ngày', 2500000, 2000, 1600),
(18, N'Seated 2 Ngày', 1800000, 1500, 1200),
(18, N'Standing 1 Ngày', 1500000, 1000, 800);

-- Event 19: Y-Concert by Yeah1
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(19, N'VVIP Royale', 5000000, 100, 80),
(19, N'VIP Diamond', 3500000, 300, 250),
(19, N'Cat A', 2000000, 1000, 850),
(19, N'Cat B', 1500000, 1500, 1280),
(19, N'Cat C', 1000000, 2000, 1700);
GO

-- 6. INSERT DonHang (Đơn hàng mẫu - khách vãng lai)
INSERT INTO DonHang (UserId, SuKienId, LoaiVeId, SoLuong, TongTien, TenKhachHang, Email, SoDienThoai, TrangThai) VALUES
-- Đơn Red Velvet REDMARE Standing (LoaiVeId = 3)
(NULL, 1, 3, 2, 2400000, N'Nguyễn Thùy Linh', 'thuylinh.reveluv@gmail.com', '0912345678', 'DaXacNhan'),

-- Đơn Red Velvet R TO V VIP (LoaiVeId = 6)
(NULL, 2, 6, 2, 3600000, N'Trần Minh Anh', 'minhanh.rv@gmail.com', '0923456789', 'DaXacNhan'),

-- Đơn Taeyeon The Tense VIP (LoaiVeId = 10)
(NULL, 3, 10, 2, 5600000, N'Lê Khánh Linh', 'khanhlinh.sone@gmail.com', '0934567890', 'DaXacNhan'),

-- Đơn T-ARA Fan Zone (LoaiVeId = 19)
(NULL, 5, 19, 3, 4500000, N'Phạm Thảo Nguyên', 'thaonguyen.queen@gmail.com', '0945678901', 'DaXacNhan'),

-- Đơn Bùi Lan Hương VIP (LoaiVeId = 22)
(NULL, 6, 22, 2, 3000000, N'Võ Hương Giang', 'huonggiang.dreampop@gmail.com', '0956789012', 'DaXacNhan'),

-- Đơn Mỹ Tâm Premium (LoaiVeId = 26)
(NULL, 7, 26, 2, 2400000, N'Đỗ Thanh Tâm', 'thanhtam.mytam@gmail.com', '0967890123', 'DaXacNhan'),

-- Đơn Phan Mạnh Quỳnh Premium (LoaiVeId = 30)
(NULL, 8, 30, 2, 1000000, N'Bùi Ngọc Ánh', 'ngocanh.acoustic@gmail.com', '0978901234', 'DaXacNhan'),

-- Đơn Ái Phương VIP Table (LoaiVeId = 33)
(NULL, 9, 33, 1, 1000000, N'Hoàng Thu Hà', 'thuha.music@gmail.com', '0989012345', 'DaXacNhan'),

-- Đơn Đông Nhi VIP (LoaiVeId = 38)
(NULL, 10, 38, 2, 4000000, N'Phan Quỳnh Anh', 'quynhanh.dongnhi@gmail.com', '0990123456', 'DaXacNhan'),

-- Đơn Kim Tae-ri VVIP (LoaiVeId = 54)
(NULL, 15, 54, 2, 5000000, N'Nguyễn Minh Châu', 'minhchau.kdrama@gmail.com', '0901234567', 'ChoDuyet'),

-- Đơn V-Music Festival VIP 2 Ngày (LoaiVeId = 66)
(NULL, 18, 66, 2, 12000000, N'Trương Thành Long', 'thanhlong.music@gmail.com', '0912345670', 'DaXacNhan'),

-- Đơn Vpop Legends VVIP (LoaiVeId = 62)
(NULL, 17, 62, 1, 5000000, N'Lý Minh Tuấn', 'minhtuan.vpop@gmail.com', '0923456781', 'DaXacNhan'),

-- Đơn Bùi Công Nam Family Pack (LoaiVeId = 46)
(NULL, 12, 46, 1, 2000000, N'Phạm Gia Hân', 'giahan.tet@gmail.com', '0934567892', 'DaXacNhan'),

-- Đơn Hoàng Thùy Linh Standing (LoaiVeId = 43)
(NULL, 11, 43, 3, 3000000, N'Đặng Quỳnh Anh', 'quynhanh.htl@gmail.com', '0945678903', 'DaXacNhan'),

-- Đơn Pu Shu Premium (LoaiVeId = 51)
(NULL, 14, 51, 2, 2400000, N'Vũ Minh Khang', 'minhkhang.pushu@gmail.com', '0956789014', 'ChoDuyet'),

-- Đơn Y-Concert VVIP Royale (LoaiVeId = 65)
(NULL, 19, 65, 2, 10000000, N'Nguyễn Hoàng Long', 'hoanglong.yeah1@gmail.com', '0967890125', 'DaXacNhan'),

-- Đơn Y-Concert Cat A (LoaiVeId = 67)
(NULL, 19, 67, 4, 8000000, N'Trần Thị Mai', 'thimai.concert@gmail.com', '0978901236', 'DaXacNhan'),

-- Đơn Y-Concert VIP Diamond (LoaiVeId = 66)
(NULL, 19, 66, 1, 3500000, N'Lê Văn Hùng', 'vanhung.yconcert@gmail.com', '0989012347', 'ChoDuyet');
GO

-- 7. Thêm FK constraint cho DonHang (chạy sau khi Identity tạo AspNetUsers)
-- LƯU Ý: Chạy script này SAU KHI chạy migrations
PRINT 'Chờ AspNetUsers được tạo bởi Identity, sau đó chạy script:';
PRINT 'ALTER TABLE DonHang ADD CONSTRAINT FK_DonHang_AspNetUsers FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE SET NULL;';
GO

PRINT '========================================';
PRINT 'Database QuanLySuKien đã được tạo thành công!';
PRINT '========================================';
PRINT 'Tổng quan:';
PRINT '- 9 Địa điểm';
PRINT '- 12 Nghệ sĩ (Kpop, Vpop, Cpop, Kdrama)';
PRINT '- 19 Sự kiện (Concert, Fan Meeting, Festival)';
PRINT '- 74 Loại vé';
PRINT '- 18 Đơn hàng mẫu';
PRINT '';
PRINT 'Sự kiện mới nhất:';
PRINT '- Y-Concert by Yeah1 "Mình Đoàn Viên Thôi" (20/12/2025)';
PRINT '  Lineup: Bùi Lan Hương, Ái Phương, Bùi Công Nam + 50+ nghệ sĩ khác';
PRINT '  Địa điểm: Vinhomes Ocean Park 3';
PRINT '';
PRINT 'LƯU Ý: Nhớ tải hình ảnh theo đường dẫn trong database!';
PRINT '========================================';
GO
