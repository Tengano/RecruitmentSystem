-- =============================================
-- Script tạo hoàn chỉnh Database HeThongTuyenDung (Phiên bản đơn giản)
-- Bao gồm: Login, Database, Tables, Data, Indexes, Views, Procedures
-- =============================================

-- =============================================
-- PHẦN 1: TẠO LOGIN USER
-- =============================================

USE master;
GO

-- Xóa login cũ nếu tồn tại
IF EXISTS (SELECT name FROM sys.server_principals WHERE name = 'HeThongTuyenDung_App')
BEGIN
    DROP LOGIN HeThongTuyenDung_App;
END
GO

-- Tạo login mới
CREATE LOGIN HeThongTuyenDung_App 
WITH PASSWORD = 'Khoi@123456',
DEFAULT_DATABASE = HeThongTuyenDung;
GO

PRINT 'Đã tạo login HeThongTuyenDung_App thành công!';
GO

-- =============================================
-- PHẦN 2: TẠO DATABASE
-- =============================================

-- Xóa database cũ nếu tồn tại
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'HeThongTuyenDung')
BEGIN
    ALTER DATABASE HeThongTuyenDung SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE HeThongTuyenDung;
END
GO

-- Tạo database mới
CREATE DATABASE HeThongTuyenDung COLLATE Vietnamese_CI_AS;
GO

USE HeThongTuyenDung;
GO

-- =============================================
-- PHẦN 3: TẠO CÁC BẢNG CHÍNH
-- =============================================

-- Bảng CongViec (Jobs)
CREATE TABLE CongViec (
    JobId INT PRIMARY KEY IDENTITY(1,1),
    TieuDe NVARCHAR(200) NOT NULL,
    MoTa NVARCHAR(MAX) NOT NULL,
    YeuCau NVARCHAR(MAX) NOT NULL,
    PhucLoi NVARCHAR(MAX),
    DiaDiem NVARCHAR(100) NOT NULL,
    LoaiCongViec NVARCHAR(50) NOT NULL,
    LuongToiThieu DECIMAL(18, 2),
    LuongToiDa DECIMAL(18, 2),
    CongTy NVARCHAR(100),
    NgayDang DATETIME NOT NULL DEFAULT GETDATE(),
    NgayKetThuc DATETIME,
    HoatDong BIT NOT NULL DEFAULT 1,
    LuotXem INT NOT NULL DEFAULT 0,
    DanhMuc NVARCHAR(100),
    NamKinhNghiem INT NOT NULL DEFAULT 0
);

-- Bảng UngVien (Candidates)
CREATE TABLE UngVien (
    CandidateId INT PRIMARY KEY IDENTITY(1,1),
    HoTen NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    SoDienThoai NVARCHAR(20),
    DiaChi NVARCHAR(200),
    NgaySinh DATE,
    GioiTinh NVARCHAR(10),
    TrinhDoHocVan NVARCHAR(50),
    KinhNghiem NVARCHAR(MAX),
    KyNang NVARCHAR(MAX),
    NgayTao DATETIME NOT NULL DEFAULT GETDATE()
);

-- Bảng DonUngTuyen (Applications)
CREATE TABLE DonUngTuyen (
    ApplicationId INT PRIMARY KEY IDENTITY(1,1),
    HoTen NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    SoDienThoai NVARCHAR(20),
    DiaChi NVARCHAR(200),
    NgaySinh DATE,
    GioiTinh NVARCHAR(10),
    TrinhDoHocVan NVARCHAR(50),
    KinhNghiem NVARCHAR(MAX),
    KyNang NVARCHAR(MAX),
    JobId INT NOT NULL,
    NgayUngTuyen DATETIME NOT NULL DEFAULT GETDATE(),
    TrangThai NVARCHAR(50) NOT NULL DEFAULT 'Chờ xem xét',
    FOREIGN KEY (JobId) REFERENCES CongViec(JobId) ON DELETE CASCADE
);

-- =============================================
-- PHẦN 4: TẠO BẢNG NGƯỜI DÙNG ĐƠN GIẢN
-- =============================================

-- Bảng NguoiDung (Users) - Đơn giản
CREATE TABLE NguoiDung (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TenDangNhap NVARCHAR(50) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    MatKhau NVARCHAR(100) NOT NULL,
    HoTen NVARCHAR(100) NOT NULL,
    VaiTro NVARCHAR(20) NOT NULL DEFAULT 'User', -- Admin, User
    NgayTao DATETIME NOT NULL DEFAULT GETDATE(),
    HoatDong BIT NOT NULL DEFAULT 1
);

-- =============================================
-- PHẦN 5: CHÈN DỮ LIỆU MẪU
-- =============================================

-- Tạo tài khoản admin
INSERT INTO NguoiDung (TenDangNhap, Email, MatKhau, HoTen, VaiTro) 
VALUES ('admin', 'admin@tuyendung.com', 'admin123', 'Quản trị viên', 'Admin');

-- Tạo tài khoản user thường
INSERT INTO NguoiDung (TenDangNhap, Email, MatKhau, HoTen, VaiTro) 
VALUES ('user1', 'user1@email.com', 'user123', 'Người dùng 1', 'User');

-- Chèn dữ liệu mẫu cho công việc
INSERT INTO CongViec (TieuDe, MoTa, YeuCau, PhucLoi, DiaDiem, LoaiCongViec, LuongToiThieu, LuongToiDa, CongTy, DanhMuc, NamKinhNghiem) VALUES
('Lập trình viên .NET', 'Phát triển ứng dụng web sử dụng .NET Core', 'Có kinh nghiệm với C#, ASP.NET Core, Entity Framework', 'Lương cạnh tranh, bảo hiểm đầy đủ', 'Hà Nội', 'Toàn thời gian', 15000000, 25000000, 'Công ty ABC', 'Công nghệ thông tin', 2),
('Thiết kế UI/UX', 'Thiết kế giao diện người dùng cho ứng dụng web', 'Thành thạo Figma, Adobe XD, có kinh nghiệm thiết kế responsive', 'Môi trường làm việc năng động', 'TP.HCM', 'Toàn thời gian', 12000000, 20000000, 'Công ty XYZ', 'Thiết kế', 1),
('Chuyên viên Marketing', 'Xây dựng chiến lược marketing và quảng bá sản phẩm', 'Có kinh nghiệm digital marketing, SEO, Google Ads', 'Thưởng theo hiệu suất', 'Đà Nẵng', 'Toàn thời gian', 10000000, 18000000, 'Công ty DEF', 'Marketing', 2),
('Kế toán viên', 'Thực hiện các công việc kế toán và tài chính', 'Tốt nghiệp chuyên ngành kế toán, thành thạo Excel', 'Lương ổn định, môi trường làm việc tốt', 'Hà Nội', 'Toàn thời gian', 8000000, 15000000, 'Công ty GHI', 'Tài chính', 1),
('Nhân viên bán hàng', 'Tìm kiếm và phát triển khách hàng mới', 'Có kỹ năng giao tiếp tốt, năng động', 'Hoa hồng cao, cơ hội thăng tiến', 'TP.HCM', 'Toàn thời gian', 6000000, 12000000, 'Công ty JKL', 'Bán hàng', 0);

-- Chèn dữ liệu mẫu cho ứng viên
INSERT INTO UngVien (HoTen, Email, SoDienThoai, DiaChi, NgaySinh, GioiTinh, TrinhDoHocVan, KinhNghiem, KyNang) VALUES
('Nguyễn Văn A', 'nguyenvana@email.com', '0123456789', 'Hà Nội', '1990-01-01', 'Nam', 'Đại học', '2 năm kinh nghiệm lập trình', 'C#, JavaScript, HTML/CSS'),
('Trần Thị B', 'tranthib@email.com', '0987654321', 'TP.HCM', '1992-05-15', 'Nữ', 'Cao đẳng', '1 năm kinh nghiệm thiết kế', 'Figma, Photoshop, Illustrator'),
('Lê Văn C', 'levanc@email.com', '0369258147', 'Đà Nẵng', '1988-12-20', 'Nam', 'Đại học', '3 năm kinh nghiệm marketing', 'SEO, Google Ads, Facebook Ads'),
('Phạm Thị D', 'phamthid@email.com', '0147258369', 'Hà Nội', '1991-08-10', 'Nữ', 'Đại học', '2 năm kinh nghiệm kế toán', 'Excel, SAP, QuickBooks'),
('Hoàng Văn E', 'hoangvane@email.com', '0258147369', 'TP.HCM', '1993-03-25', 'Nam', 'Cao đẳng', '1 năm kinh nghiệm bán hàng', 'Giao tiếp, thuyết trình, CRM');

-- Chèn dữ liệu mẫu cho đơn ứng tuyển
INSERT INTO DonUngTuyen (HoTen, Email, SoDienThoai, DiaChi, NgaySinh, GioiTinh, TrinhDoHocVan, KinhNghiem, KyNang, JobId, TrangThai) VALUES
('Nguyễn Văn F', 'nguyenvand@email.com', '0123456789', 'Hà Nội', '1991-03-10', 'Nam', 'Đại học', '1 năm kinh nghiệm lập trình', 'C#, ASP.NET', 1, 'Chờ xem xét'),
('Trần Thị G', 'tranthie@email.com', '0987654321', 'TP.HCM', '1993-07-25', 'Nữ', 'Cao đẳng', '6 tháng kinh nghiệm thiết kế', 'Figma, Adobe XD', 2, 'Đã phỏng vấn'),
('Lê Văn H', 'levanf@email.com', '0369258147', 'Đà Nẵng', '1989-11-30', 'Nam', 'Đại học', '2 năm kinh nghiệm marketing', 'SEO, Content Marketing', 3, 'Đã từ chối'),
('Phạm Thị I', 'phamthii@email.com', '0147258369', 'Hà Nội', '1992-06-15', 'Nữ', 'Đại học', '1 năm kinh nghiệm kế toán', 'Excel, SAP', 4, 'Chờ xem xét'),
('Hoàng Văn K', 'hoangvank@email.com', '0258147369', 'TP.HCM', '1994-09-20', 'Nam', 'Cao đẳng', '6 tháng kinh nghiệm bán hàng', 'Giao tiếp, CRM', 5, 'Đã phỏng vấn');

-- =============================================
-- PHẦN 6: TẠO INDEX
-- =============================================

CREATE INDEX IX_CongViec_DiaDiem ON CongViec(DiaDiem);
CREATE INDEX IX_CongViec_LoaiCongViec ON CongViec(LoaiCongViec);
CREATE INDEX IX_CongViec_DanhMuc ON CongViec(DanhMuc);
CREATE INDEX IX_DonUngTuyen_JobId ON DonUngTuyen(JobId);
CREATE INDEX IX_DonUngTuyen_TrangThai ON DonUngTuyen(TrangThai);
CREATE INDEX IX_NguoiDung_TenDangNhap ON NguoiDung(TenDangNhap);
CREATE INDEX IX_NguoiDung_Email ON NguoiDung(Email);
GO

-- =============================================
-- PHẦN 7: TẠO VIEW
-- =============================================

CREATE VIEW ThongKeCongViec AS
SELECT 
    COUNT(*) as TongCongViec,
    COUNT(CASE WHEN HoatDong = 1 THEN 1 END) as CongViecHoatDong,
    COUNT(CASE WHEN HoatDong = 0 THEN 1 END) as CongViecKhongHoatDong,
    AVG(LuongToiThieu) as LuongTrungBinh
FROM CongViec;
GO

CREATE VIEW ThongKeUngTuyen AS
SELECT 
    COUNT(*) as TongDonUngTuyen,
    COUNT(CASE WHEN TrangThai = 'Chờ xem xét' THEN 1 END) as ChoXemXet,
    COUNT(CASE WHEN TrangThai = 'Đã phỏng vấn' THEN 1 END) as DaPhongVan,
    COUNT(CASE WHEN TrangThai = 'Đã từ chối' THEN 1 END) as DaTuChoi
FROM DonUngTuyen;
GO

-- =============================================
-- PHẦN 8: TẠO STORED PROCEDURE
-- =============================================

CREATE PROCEDURE TimKiemCongViec
    @DiaDiem NVARCHAR(100) = NULL,
    @LoaiCongViec NVARCHAR(50) = NULL,
    @DanhMuc NVARCHAR(100) = NULL,
    @TimKiem NVARCHAR(200) = NULL
AS
BEGIN
    SELECT * FROM CongViec 
    WHERE HoatDong = 1
    AND (@DiaDiem IS NULL OR DiaDiem LIKE '%' + @DiaDiem + '%')
    AND (@LoaiCongViec IS NULL OR LoaiCongViec = @LoaiCongViec)
    AND (@DanhMuc IS NULL OR DanhMuc = @DanhMuc)
    AND (@TimKiem IS NULL OR TieuDe LIKE '%' + @TimKiem + '%' OR MoTa LIKE '%' + @TimKiem + '%')
    ORDER BY NgayDang DESC;
END
GO

CREATE PROCEDURE DangNhap
    @TenDangNhap NVARCHAR(50),
    @MatKhau NVARCHAR(100)
AS
BEGIN
    SELECT Id, TenDangNhap, Email, HoTen, VaiTro, HoatDong
    FROM NguoiDung 
    WHERE TenDangNhap = @TenDangNhap 
    AND MatKhau = @MatKhau 
    AND HoatDong = 1;
END
GO

CREATE PROCEDURE CapNhatTrangThaiUngTuyen
    @ApplicationId INT,
    @TrangThai NVARCHAR(50)
AS
BEGIN
    UPDATE DonUngTuyen 
    SET TrangThai = @TrangThai 
    WHERE ApplicationId = @ApplicationId;
END
GO

-- =============================================
-- PHẦN 9: TẠO TRIGGER
-- =============================================

CREATE TRIGGER TR_TangLuotXemCongViec
ON CongViec
AFTER UPDATE
AS
BEGIN
    IF UPDATE(LuotXem)
    BEGIN
        PRINT 'Đã cập nhật lượt xem công việc';
    END
END
GO

CREATE TRIGGER TR_LogDangNhap
ON NguoiDung
AFTER UPDATE
AS
BEGIN
    IF UPDATE(HoatDong)
    BEGIN
        PRINT 'Đã cập nhật trạng thái người dùng';
    END
END
GO

-- =============================================
-- PHẦN 10: TẠO USER VÀ PHÂN QUYỀN
-- =============================================

-- Tạo user cho ứng dụng
CREATE USER HeThongTuyenDung_App FOR LOGIN HeThongTuyenDung_App;
GO

-- Phân quyền cho user
ALTER ROLE db_datareader ADD MEMBER HeThongTuyenDung_App;
ALTER ROLE db_datawriter ADD MEMBER HeThongTuyenDung_App;
ALTER ROLE db_ddladmin ADD MEMBER HeThongTuyenDung_App;
GO

-- =============================================
-- PHẦN 11: KIỂM TRA VÀ HOÀN THÀNH
-- =============================================

-- Kiểm tra các bảng đã tạo
PRINT '=============================================';
PRINT 'KIỂM TRA CÁC BẢNG ĐÃ TẠO:';
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME;

-- Kiểm tra số lượng dữ liệu
PRINT 'SỐ LƯỢNG DỮ LIỆU:';
SELECT 'NguoiDung' as Bang, COUNT(*) as SoLuong FROM NguoiDung
UNION ALL SELECT 'CongViec', COUNT(*) FROM CongViec
UNION ALL SELECT 'UngVien', COUNT(*) FROM UngVien
UNION ALL SELECT 'DonUngTuyen', COUNT(*) FROM DonUngTuyen;

-- Kiểm tra admin
PRINT 'TÀI KHOẢN ADMIN:';
SELECT TenDangNhap, Email, HoTen, VaiTro FROM NguoiDung WHERE VaiTro = 'Admin';

PRINT '=============================================';
PRINT 'ĐÃ TẠO THÀNH CÔNG DATABASE HeThongTuyenDung (PHIÊN BẢN ĐƠN GIẢN)';
PRINT '=============================================';
PRINT 'THÔNG TIN ĐĂNG NHẬP ADMIN:';
PRINT 'Tên đăng nhập: admin';
PRINT 'Mật khẩu: admin123';
PRINT '=============================================';
PRINT 'CÁC BẢNG ĐÃ TẠO:';
PRINT '- CongViec (Công việc)';
PRINT '- UngVien (Ứng viên)';
PRINT '- DonUngTuyen (Đơn ứng tuyển)';
PRINT '- NguoiDung (Người dùng - đơn giản)';
PRINT '=============================================';
PRINT 'DỮ LIỆU MẪU:';
PRINT '- 2 người dùng (1 admin, 1 user)';
PRINT '- 5 công việc';
PRINT '- 5 ứng viên';
PRINT '- 5 đơn ứng tuyển';
PRINT '=============================================';
PRINT 'STORED PROCEDURES:';
PRINT '- TimKiemCongViec: Tìm kiếm công việc';
PRINT '- DangNhap: Đăng nhập người dùng';
PRINT '- CapNhatTrangThaiUngTuyen: Cập nhật trạng thái ứng tuyển';
PRINT '=============================================';
PRINT 'VIEWS:';
PRINT '- ThongKeCongViec: Thống kê công việc';
PRINT '- ThongKeUngTuyen: Thống kê ứng tuyển';
PRINT '=============================================';
GO