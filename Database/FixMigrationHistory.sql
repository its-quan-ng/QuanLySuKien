-- Fix migration history - Đánh dấu migration đã apply
-- Chạy script này trong SQL Server để fix lỗi migration conflict

USE QuanLySuKien;
GO

-- Insert migration record vào __EFMigrationsHistory
IF NOT EXISTS (SELECT 1 FROM __EFMigrationsHistory WHERE MigrationId = '20251124164905_add-user-data')
BEGIN
    INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion)
    VALUES ('20251124164905_add-user-data', '8.0.16');

    PRINT 'Migration record đã được thêm vào __EFMigrationsHistory';
END
ELSE
BEGIN
    PRINT 'Migration record đã tồn tại, không cần thêm';
END
GO
