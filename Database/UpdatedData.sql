-- ========================================
-- SCRIPT CẬP NHẬT DỮ LIỆU HỆ THỐNG QUẢN LÝ SỰ KIỆN
-- Tạo data mẫu cho Demo/Testing
-- ========================================

-- XÓA DỮ LIỆU CŨ (theo thứ tự foreign key)
DELETE FROM DonHang;
DELETE FROM LoaiVe;
DELETE FROM SuKien_NgheSi;
DELETE FROM SuKien;
DELETE FROM NgheSi;
DELETE FROM DiaDiem;
GO

-- Reset IDENTITY seeds
DBCC CHECKIDENT ('DonHang', RESEED, 0);
DBCC CHECKIDENT ('LoaiVe', RESEED, 0);
DBCC CHECKIDENT ('SuKien', RESEED, 0);
DBCC CHECKIDENT ('NgheSi', RESEED, 0);
DBCC CHECKIDENT ('DiaDiem', RESEED, 0);
GO

-- 1. INSERT DiaDiem (Giữ nguyên)
INSERT INTO DiaDiem (TenDiaDiem, DiaChi, SucChua, MoTa) VALUES
(N'Nhà hát Lớn Hà Nội', N'01 Tràng Tiền, Hoàn Kiếm, Hà Nội', 1000, N'Nhà hát lịch sử với kiến trúc Pháp cổ điển'),
(N'Cung Văn hóa Hữu nghị', N'91 Trần Hưng Đạo, Hoàn Kiếm, Hà Nội', 800, N'Địa điểm tổ chức sự kiện văn hóa lớn'),
(N'Sân vận động Mỹ Đình', N'Từ Liêm, Hà Nội', 40000, N'Sân vận động quốc gia, phù hợp concert lớn'),
(N'The Opera House', N'Số 10, Đồng Khởi, Quận 1, TP.HCM', 500, N'Nhà hát Opera sang trọng tại Sài Gòn'),
(N'SECC Hall', N'799 Nguyễn Văn Linh, Quận 7, TP.HCM', 5000, N'Trung tâm hội chợ triển lãm lớn'),
(N'Nhà hát Hòa Bình', N'240 Điện Biên Phủ, Bình Thạnh, TP.HCM', 1200, N'Nhà hát đa năng hiện đại');
GO

-- 2. INSERT NgheSi (Giữ theo bản đã sửa)
INSERT INTO NgheSi (TenNgheSi, TheLoai, TieuSu, AnhDaiDien) VALUES
-- Kpop Artists
(N'Red Velvet', N'Kpop/Pop/R&B', N'Nhóm nhạc nữ Hàn Quốc thuộc SM Entertainment. Nổi tiếng với concept "dual" kết hợp giữa Red (sôi động) và Velvet (mềm mại). Thành viên: Irene, Seulgi, Wendy, Joy, Yeri.', '/images/artists/red-velvet.jpg'),
(N'Bùi Lan Hương', N'VPop/Jazz/Dream Pop', N'Ca sĩ, diễn viên Việt Nam tham gia Đạp Gió 2024. Nhiều ca khúc hit như Ngày chưa giông bão, Mê muội, Bùa mê.', '/images/artists/bui-lan-huong.jpg'),
(N'Taeyeon (SNSD)', N'Kpop/R&B', N'Là một nữ ca sĩ người Hàn Quốc. Cô nổi tiếng với tư cách là thành viên hát chính và đội trưởng của nhóm nhạc nữ Hàn Quốc Girls Generation với ca khúc I và các album nổi bật: My Voice, Purpose, INVU.', '/images/artists/taeyeon.jpg'),
(N'Ái Phương', N'Vpop', N'Là một nghệ sĩ đa năng của làng giải trí Việt Nam, hoạt động trong các lĩnh vực ca hát, sáng tác nhạc, đóng phim và làm MC', '/images/artists/ai-phuong.jpg'),
(N'Mỹ Tâm', N'Ballad/Pop', N'Nữ ca sĩ, nhạc sĩ được mệnh danh là "Họa mi tóc nâu". Có nhiều album thành công và giải thưởng danh giá.', '/images/artists/my-tam.jpg'),
(N'Phác Thụ (Pu Shu)', N'Pop-rock, Folk rock Trung Quốc', N'Ca sĩ kiêm nhạc sĩ người Trung Quốc nổi tiếng với phong cách âm nhạc sâu sắc, giàu chất thơ và kín tiếng trong đời tư, có các tác phẩm nổi bật như "Những bông hoa đó" và "Rừng bạch dương trắng".', '/images/artists/pu-shu.jpg'),
(N'Phan Mạnh Quỳnh', N'VPop/Pop/Ballad', N'Nhạc sĩ kiêm ca sĩ với sở trường sáng tác ca từ tự sự, sâu sắc, giai điệu nhẹ nhàng chạm đến cảm xúc người nghe, nổi tiếng với các hit "Vợ người ta", "Từ đó".', '/images/artists/phan-manh-quynh.jpg'),
(N'Hoàng Thùy Linh', N'Pop/EDM', N'Ca sĩ nữ với phong cách âm nhạc đương đại, kết hợp yếu tố dân gian hiện đại.', '/images/artists/hoang-thuy-linh.jpg'),
(N'Đông Nhi', N'Vpop/Pop/Dance', N'Ca sĩ nữ hàng đầu Việt Nam, nổi tiếng với nhiều ca khúc hit và phong cách trình diễn sôi động, từng giành giải MTV EMA 2016, có lượng người hâm mộ đông đảo.', '/images/artists/dong-nhi.jpg'),
(N'Bùi Công Nam', N'Pop/Ballad/Indie', N'Ca sĩ, nhạc sĩ tài năng được mệnh danh "Ông hoàng nhạc Tết", nổi tiếng với ca từ mộc mạc, gần gũi, truyền tải năng lượng tích cực qua các ca khúc hit như "Có không giữ mất đừng tìm", "Tết phiền vẫn iu".', '/images/artists/bui-cong-nam.jpg'),
(N'Kim Taeri', N'Actress', N'Nữ diễn viên Hàn Quốc đình đám với các phim Twenty Five Twenty One, The Handmaiden, Little Forest. Được yêu mến bởi diễn xuất tự nhiên và nụ cười tỏa nắng.', '/images/artists/kim-taeri.jpg'),
(N'T-ARA', N'Kpop/Dance', N'Nhóm nhạc nữ huyền thoại Kpop thế hệ 2. Queen of retro concept với loạt hit đình đám: Roly Poly, Lovey Dovey, Bo Peep Bo Peep, Number 9. Fanbase mạnh tại Việt Nam.', '/images/artists/t-ara.jpg');
GO

-- 3. INSERT SuKien (12 sự kiện phù hợp với nghệ sĩ)
INSERT INTO SuKien (TenSuKien, LoaiSuKien, DiaDiemId, NgayToChuc, GioToChuc, AnhBia, MoTa, TrangThai) VALUES
-- 1. Red Velvet Concert
(N'Red Velvet "Happiness" Concert in Vietnam', N'Concert', 3, '2025-01-15', '19:00:00', '/images/events/red-velvet-happiness.jpg',
 N'Lần đầu tiên Red Velvet đến Việt Nam với concert "Happiness"! Trải nghiệm sân khấu hoành tráng với các hit đình đám: Psycho, Feel My Rhythm, Red Flavor, Queendom. ReVeluv Việt Nam chuẩn bị lightstick đỏ rực nhé!', 'SapDienRa'),

-- 2. Bùi Lan Hương Solo Concert
(N'Bùi Lan Hương Live Concert "Mê Muội"', N'Concert', 2, '2025-01-20', '20:00:00', '/images/events/bui-lan-huong-me-muoi.jpg',
 N'Sau thành công tại Đạp Gió 2024, Bùi Lan Hương mang đến live concert với những ca khúc sâu lắng: Ngày chưa giông bão, Mê muội, Bùa mê. Không gian âm nhạc dreamy pop kết hợp jazz đầy cảm xúc.', 'SapDienRa'),

-- 3. Taeyeon Solo Concert
(N'TAEYEON "The ODD of LOVE" in Vietnam', N'Concert', 5, '2025-02-01', '19:00:00', '/images/events/taeyeon-odd-of-love.jpg',
 N'Main vocal huyền thoại của SNSD - Taeyeon đến Việt Nam với tour "The ODD of LOVE"! Thưởng thức giọng hát thiên thần qua các bản hit: INVU, Weekend, I, Four Seasons. SONE Việt Nam đã chờ đợi quá lâu!', 'SapDienRa'),

-- 4. Mỹ Tâm Concert
(N'Mỹ Tâm Live Concert "Tri Âm"', N'Concert', 3, '2025-02-14', '19:30:00', '/images/events/my-tam-tri-am.jpg',
 N'Concert Tri Âm - tâm huyết của Mỹ Tâm với những ca khúc ballad kinh điển: Ước gì, Đừng hỏi em, Họa mi tóc nâu, Người hãy quên em đi. Một đêm nhạc đầy cảm xúc dành cho những tri âm của "Họa mi tóc nâu".', 'SapDienRa'),

-- 5. Đạp Gió 2024 Concert
(N'Chị Đẹp Đạp Gió Rẽ Sóng 2024 - Vietnam Tour', N'Concert', 5, '2025-01-25', '19:00:00', '/images/events/dap-gio-2024.jpg',
 N'Các "Chị Đẹp" từ show Đạp Gió Rẽ Sóng ghé Việt Nam! Bùi Lan Hương và dàn nghệ sĩ châu Á đình đám cùng biểu diễn những màn trình diễn bùng nổ từ chương trình. 2 tiếng đồng hồ năng lượng không ngừng nghỉ!', 'SapDienRa'),

-- 6. Phác Thụ (Pu Shu) Concert
(N'Phác Thụ "Những Bông Hoa Đó" Vietnam Tour', N'Concert', 2, '2025-03-01', '20:00:00', '/images/events/pu-shu-flowers.jpg',
 N'Lần đầu tiên huyền thoại C-pop Phác Thụ đến Việt Nam! Trải nghiệm âm nhạc folk rock sâu lắng với những bài hát huyền thoại: Những bông hoa đó (那些花儿), Rừng bạch dương trắng (白桦林). Concert dành cho những tâm hồn yêu âm nhạc thơ.', 'SapDienRa'),

-- 7. Phan Mạnh Quỳnh Live Show
(N'Phan Mạnh Quỳnh Liveshow "Từ Đó"', N'Liveshow', 1, '2025-02-20', '20:00:00', '/images/events/phan-manh-quynh-tu-do.jpg',
 N'Liveshow "Từ Đó" của nhạc sĩ Phan Mạnh Quỳnh - hành trình âm nhạc với những ca khúc tự sự chạm đến tim: Vợ người ta, Từ đó, Người ấy, Nơi này có anh. Không gian acoustic ấm áp và đầy cảm xúc.', 'SapDienRa'),

-- 8. Hoàng Thùy Linh Concert
(N'Hoàng Thùy Linh "LINK" Concert', N'Concert', 5, '2025-03-10', '19:00:00', '/images/events/hoang-thuy-linh-link.jpg',
 N'Concert "LINK" của Hoàng Thùy Linh kết hợp âm nhạc đương đại với yếu tố dân gian Việt Nam. Sân khấu đỉnh cao với các hit: Để Mị nói cho mà nghe, See Tình, Kẻ Cắp Gặp Bà God. Visual và âm nhạc đỉnh cao!', 'SapDienRa'),

-- 9. Đông Nhi Concert
(N'Đông Nhi "10 Years of Love" Concert', N'Concert', 6, '2025-03-15', '19:00:00', '/images/events/dong-nhi-10-years.jpg',
 N'Concert kỷ niệm 10 năm ca hát của Đông Nhi! Loạt hit đình đám: Taxi, Khóc, Bad Boy, Xin anh đừng. Dance hits sôi động với giọng ca nội lực và vũ đạo bùng nổ. Một đêm nhạc không thể bỏ lỡ!', 'SapDienRa'),

-- 10. Bùi Công Nam Acoustic Night
(N'Bùi Công Nam "Mùa Xuân Của Chúng Ta" Acoustic Night', N'Acoustic', 4, '2025-01-28', '19:30:00', '/images/events/bui-cong-nam-mua-xuan.jpg',
 N'Acoustic night ấm áp với Bùi Công Nam - "Ông hoàng nhạc Tết". Thưởng thức các ca khúc năng lượng tích cực: Có không giữ mất đừng tìm, Tết phiền vẫn iu, Một nhà. Đón Tết cùng âm nhạc dễ thương!', 'SapDienRa'),

-- 11. Kim Taeri Fan Meeting
(N'Kim Taeri "2521 Days" Vietnam Fan Meeting', N'Fan Meeting', 1, '2025-02-08', '14:00:00', '/images/events/kim-taeri-2521.jpg',
 N'Lần đầu tiên Kim Taeri đến Việt Nam! Gặp gỡ nữ diễn viên Twenty Five Twenty One với talk show, games, photo time. Cơ hội duy nhất để gặp Na Hee Do ngoài đời thực. Kdrama fans đừng bỏ lỡ!', 'SapDienRa'),

-- 12. T-ARA Concert
(N'T-ARA "Queens Are Back" Vietnam Concert', N'Concert', 6, '2025-03-20', '18:00:00', '/images/events/t-ara-queens-back.jpg',
 N'T-ara trở lại Việt Nam sau nhiều năm! "Queens Are Back" với full setlist huyền thoại: Roly Poly, Lovey Dovey, Day By Day, Number 9, Bo Peep Bo Peep. 6 thành viên cùng nhau tái hiện thời kỳ hoàng kim Kpop gen 2. T-ara Việt Nam ready!', 'SapDienRa');
GO

-- 4. INSERT SuKien_NgheSi (Gắn nghệ sĩ vào sự kiện)
INSERT INTO SuKien_NgheSi (SuKienId, NgheSiId) VALUES
-- 1. Red Velvet Concert → Red Velvet (#1)
(1, 1),

-- 2. Bùi Lan Hương Concert → Bùi Lan Hương (#2)
(2, 2),

-- 3. Taeyeon Concert → Taeyeon (#3)
(3, 3),

-- 4. Mỹ Tâm Concert → Mỹ Tâm (#5)
(4, 5),

-- 5. Đạp Gió → Bùi Lan Hương (#2) + có thể thêm nghệ sĩ khác
(5, 2),

-- 6. Phác Thụ Concert → Phác Thụ (#6)
(6, 6),

-- 7. Phan Mạnh Quỳnh → Phan Mạnh Quỳnh (#7)
(7, 7),

-- 8. Hoàng Thùy Linh → Hoàng Thùy Linh (#8)
(8, 8),

-- 9. Đông Nhi → Đông Nhi (#9)
(9, 9),

-- 10. Bùi Công Nam → Bùi Công Nam (#10)
(10, 10),

-- 11. Kim Taeri Fan Meeting → Kim Taeri (#11)
(11, 11),

-- 12. T-ARA Concert → T-ARA (#12)
(12, 12);
GO

-- 5. INSERT LoaiVe
-- 1. Red Velvet "Happiness" Concert
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(1, N'VVIP Soundcheck + Hi-Touch', 4500000, 80, 65),
(1, N'VIP Standing', 2800000, 300, 180),
(1, N'Standing A', 1500000, 1000, 520),
(1, N'Seated', 900000, 800, 450);

-- 2. Bùi Lan Hương "Mê Muội"
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(2, N'VIP', 1200000, 150, 80),
(2, N'Premium', 800000, 300, 150),
(2, N'Standard', 500000, 350, 200);

-- 3. Taeyeon "The ODD of LOVE"
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(3, N'VVIP Meet & Greet', 5000000, 50, 15),
(3, N'VIP', 3500000, 200, 90),
(3, N'Standing', 1800000, 800, 350),
(3, N'Seated', 1200000, 600, 280);

-- 4. Mỹ Tâm "Tri Âm"
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(4, N'VVIP', 2500000, 100, 40),
(4, N'VIP', 1500000, 300, 120),
(4, N'Standard', 800000, 600, 300);

-- 5. Đạp Gió 2024
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(5, N'VVIP Package', 4000000, 60, 20),
(5, N'VIP', 2200000, 200, 85),
(5, N'Standing', 1200000, 500, 250),
(5, N'Seated', 800000, 400, 200);

-- 6. Phác Thụ Concert
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(6, N'VIP', 1800000, 120, 60),
(6, N'Premium', 1200000, 250, 130),
(6, N'Standard', 700000, 430, 250);

-- 7. Phan Mạnh Quỳnh Liveshow
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(7, N'VIP Table', 1500000, 80, 35),
(7, N'Premium', 800000, 200, 100),
(7, N'Standard', 500000, 300, 180);

-- 8. Hoàng Thùy Linh "LINK"
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(8, N'VVIP', 2800000, 100, 45),
(8, N'VIP', 1800000, 250, 110),
(8, N'Standing', 1000000, 600, 300),
(8, N'Seated', 700000, 500, 250);

-- 9. Đông Nhi "10 Years of Love"
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(9, N'VVIP Fan Zone', 2500000, 80, 30),
(9, N'VIP', 1500000, 200, 90),
(9, N'Standing', 900000, 500, 250),
(9, N'Seated', 600000, 400, 200);

-- 10. Bùi Công Nam Acoustic
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(10, N'VIP Table', 1200000, 60, 25),
(10, N'Premium', 700000, 150, 70),
(10, N'Standard', 400000, 290, 150);

-- 11. Kim Taeri Fan Meeting
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(11, N'VVIP Polaroid + Autograph', 3000000, 40, 8),
(11, N'VIP Hi-Touch', 2000000, 100, 35),
(11, N'Premium Seat', 1200000, 200, 90),
(11, N'Standard', 700000, 400, 220);

-- 12. T-ARA "Queens Are Back"
INSERT INTO LoaiVe (SuKienId, TenLoai, GiaVe, TongSoLuong, SoLuongConLai) VALUES
(12, N'VVIP Group Photo', 3500000, 50, 15),
(12, N'VIP Hi-Touch', 2200000, 150, 60),
(12, N'Fan Zone', 1400000, 400, 180),
(12, N'Standard', 900000, 600, 320);
GO

-- 6. INSERT DonHang (Đơn hàng mẫu - khách vãng lai)
INSERT INTO DonHang (UserId, SuKienId, LoaiVeId, SoLuong, TongTien, TenKhachHang, Email, SoDienThoai, TrangThai) VALUES
-- Red Velvet Concert - VIP Standing
(NULL, 1, 2, 2, 5600000, N'Nguyễn Thùy Linh', 'thuylinh.reveluv@gmail.com', '0912345678', 'DaXacNhan'),

-- Bùi Lan Hương - Premium
(NULL, 2, 6, 1, 800000, N'Trần Minh Anh', 'minhanh@gmail.com', '0923456789', 'DaXacNhan'),

-- Taeyeon - VVIP Meet & Greet
(NULL, 3, 9, 1, 5000000, N'Phạm Thu Hà', 'thuha.sone@gmail.com', '0934567890', 'ChoDuyet'),

-- Mỹ Tâm - Standard
(NULL, 4, 16, 3, 2400000, N'Lê Khánh Vi', 'khanhvi@gmail.com', '0945678901', 'DaXacNhan'),

-- Đạp Gió - VIP
(NULL, 5, 18, 2, 4400000, N'Võ Thanh Tâm', 'thanhtam@gmail.com', '0956789012', 'DaXacNhan'),

-- Phác Thụ - Premium
(NULL, 6, 22, 2, 2400000, N'Hoàng Minh Quân', 'minhquan.music@gmail.com', '0967890123', 'DaXacNhan'),

-- Phan Mạnh Quỳnh - VIP Table
(NULL, 7, 25, 1, 1500000, N'Bùi Ngọc Ánh', 'ngocanh@gmail.com', '0978901234', 'DaXacNhan'),

-- Hoàng Thùy Linh - VIP
(NULL, 8, 30, 2, 3600000, N'Đỗ Hương Giang', 'huonggiang.htl@gmail.com', '0989012345', 'ChoDuyet'),

-- Đông Nhi - Standing
(NULL, 9, 35, 2, 1800000, N'Phan Quỳnh Như', 'quynhnhu@gmail.com', '0990123456', 'DaXacNhan'),

-- Kim Taeri Fan Meeting - VIP Hi-Touch
(NULL, 11, 42, 1, 2000000, N'Nguyễn Kim Chi', 'kimchi.2521@gmail.com', '0901234567', 'DaXacNhan'),

-- T-ARA - VIP Hi-Touch
(NULL, 12, 46, 2, 4400000, N'Trần Thanh Thảo', 'thanhthao.tara@gmail.com', '0912345670', 'DaXacNhan');
GO

-- ========================================
-- HOÀN TẤT CẬP NHẬT DỮ LIỆU
-- ========================================
PRINT 'Đã cập nhật xong dữ liệu mẫu!'
PRINT '- 6 Địa điểm'
PRINT '- 12 Nghệ sĩ'
PRINT '- 12 Sự kiện'
PRINT '- 48 Loại vé'
PRINT '- 11 Đơn hàng mẫu'
GO
