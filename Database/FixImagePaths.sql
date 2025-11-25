-- =============================================
-- FIX IMAGE PATHS - Cập nhật đường dẫn ảnh cho khớp với file thực tế
-- Giữ nguyên tất cả data, chỉ sửa đường dẫn ảnh
-- =============================================
USE QuanLySuKien;
GO

PRINT 'Bắt đầu cập nhật đường dẫn ảnh...';
GO

-- UPDATE ảnh sự kiện để khớp với file có sẵn trong wwwroot/images/events/
UPDATE SuKien SET AnhBia = '/images/events/red-mare.jpg'
WHERE TenSuKien LIKE '%Red Velvet%' AND TenSuKien LIKE '%REDMARE%';

UPDATE SuKien SET AnhBia = '/images/events/r-to-v.jpg'
WHERE TenSuKien LIKE '%Red Velvet%' AND TenSuKien LIKE '%R TO V%';

UPDATE SuKien SET AnhBia = '/images/events/taeyeon-tense.jpg'
WHERE TenSuKien LIKE '%Taeyeon%' AND TenSuKien LIKE '%Tense%';

UPDATE SuKien SET AnhBia = '/images/events/taeyeon-weekend.jpg'
WHERE TenSuKien LIKE '%Taeyeon%' AND TenSuKien LIKE '%Weekend%';

UPDATE SuKien SET AnhBia = '/images/events/t-ara-queens.jpg'
WHERE TenSuKien LIKE '%T-ARA%' OR TenSuKien LIKE '%T-ara%';

UPDATE SuKien SET AnhBia = '/images/events/thien-than-sa-nga.jpg'
WHERE TenSuKien LIKE '%Bùi Lan Hương%' AND TenSuKien LIKE '%Thiên Thần%';

UPDATE SuKien SET AnhBia = '/images/events/tri-am-my-tam.jpg'
WHERE TenSuKien LIKE '%Mỹ Tâm%';

UPDATE SuKien SET AnhBia = '/images/events/vo-nguoi-ta.jpg'
WHERE TenSuKien LIKE '%Phan Mạnh Quỳnh%';

UPDATE SuKien SET AnhBia = '/images/events/ky-uc-tinh-yeu.jpg'
WHERE TenSuKien LIKE '%Ái Phương%';

UPDATE SuKien SET AnhBia = '/images/events/10-years-dong-nhi.jpg'
WHERE TenSuKien LIKE '%Đông Nhi%';

UPDATE SuKien SET AnhBia = '/images/events/link-htl.jpg'
WHERE TenSuKien LIKE '%Hoàng Thùy Linh%';

UPDATE SuKien SET AnhBia = '/images/events/tet-phien-van-iu.jpg'
WHERE TenSuKien LIKE '%Bùi Công Nam%' AND TenSuKien LIKE '%Tết%';

UPDATE SuKien SET AnhBia = '/images/events/bof-reunion.jpg'
WHERE TenSuKien LIKE '%B.O.F%' OR TenSuKien LIKE '%Anh Trai%';

UPDATE SuKien SET AnhBia = '/images/events/pu-shu-concert.jpg'
WHERE TenSuKien LIKE '%Phác Thụ%' OR TenSuKien LIKE '%Pu Shu%';

UPDATE SuKien SET AnhBia = '/images/events/kim-taeri-fanmeeting.jpg'
WHERE TenSuKien LIKE '%Kim%' AND (TenSuKien LIKE '%Tae%' OR TenSuKien LIKE '%2521%');

UPDATE SuKien SET AnhBia = '/images/events/dap-gio-2024.jpg'
WHERE TenSuKien LIKE '%Đạp Gió%' OR TenSuKien LIKE '%Chị Đẹp%';

UPDATE SuKien SET AnhBia = '/images/events/vpop-legends.jpg'
WHERE TenSuKien LIKE '%Vpop Legends%' OR TenSuKien LIKE '%Diva%';

UPDATE SuKien SET AnhBia = '/images/events/v-music-fest.jpg'
WHERE TenSuKien LIKE '%V-Music Festival%';

UPDATE SuKien SET AnhBia = '/images/events/y-concert.jpg'
WHERE TenSuKien LIKE '%Y-Concert%';

GO

PRINT 'Đã cập nhật đường dẫn ảnh thành công!';
PRINT '';
PRINT 'Kiểm tra kết quả:';
GO

-- Hiển thị tất cả sự kiện với đường dẫn ảnh mới
SELECT
    Id,
    TenSuKien,
    AnhBia,
    CASE
        WHEN AnhBia IS NULL OR AnhBia = '' THEN '❌ Thiếu ảnh'
        ELSE '✅ Có ảnh'
    END AS TrangThaiAnh
FROM SuKien
ORDER BY Id;
GO

PRINT '';
PRINT '========================================';
PRINT 'Hoàn tất! Hãy refresh browser để xem kết quả.';
PRINT '========================================';
GO
