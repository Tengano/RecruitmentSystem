-- =============================================
-- Script tạo hoàn chỉnh Database HeThongTuyenDung (Phiên bản tiếng Việt)
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

-- Tạo database mới với collation tiếng Việt
CREATE DATABASE HeThongTuyenDung COLLATE Vietnamese_CI_AS;
GO

USE HeThongTuyenDung;
GO

-- =============================================
-- PHẦN 3: TẠO CÁC BẢNG CHÍNH
-- =============================================

-- Bảng NguoiDung (Users)
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

-- Bảng CongViec (Jobs)
CREATE TABLE CongViec (
    MaCongViec INT PRIMARY KEY IDENTITY(1,1),
    TieuDe NVARCHAR(200) NOT NULL,
    MoTa NVARCHAR(MAX) NOT NULL,
    YeuCau NVARCHAR(MAX) NOT NULL,
    PhucLoi NVARCHAR(MAX),
    DiaDiem NVARCHAR(100) NOT NULL,
    LoaiCongViec NVARCHAR(50) NOT NULL, -- Toàn thời gian, Bán thời gian, Hợp đồng
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
    MaUngVien INT PRIMARY KEY IDENTITY(1,1),
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
    MaDonUngTuyen INT PRIMARY KEY IDENTITY(1,1),
    MaCongViec INT NOT NULL,
    HoTen NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    SoDienThoai NVARCHAR(20),
    DiaChi NVARCHAR(200),
    NgaySinh DATE,
    GioiTinh NVARCHAR(10),
    TrinhDoHocVan NVARCHAR(50),
    KinhNghiem NVARCHAR(MAX),
    KyNang NVARCHAR(MAX),
    NgayUngTuyen DATETIME NOT NULL DEFAULT GETDATE(),
    TrangThai NVARCHAR(50) NOT NULL DEFAULT 'Chờ xem xét', -- Chờ xem xét, Đã phỏng vấn, Đã từ chối
    FOREIGN KEY (MaCongViec) REFERENCES CongViec(MaCongViec) ON DELETE CASCADE
);

-- =============================================
-- PHẦN 4: CHÈN DỮ LIỆU MẪU
-- =============================================

-- Tạo tài khoản admin
INSERT INTO NguoiDung (TenDangNhap, Email, MatKhau, HoTen, VaiTro) 
VALUES ('admin', 'admin@tuyendung.com', 'admin123', 'Quản trị viên hệ thống', 'Admin');

-- Tạo tài khoản user thường
INSERT INTO NguoiDung (TenDangNhap, Email, MatKhau, HoTen, VaiTro) 
VALUES ('user1', 'user1@email.com', 'user123', 'Nguyễn Văn A', 'User');

INSERT INTO NguoiDung (TenDangNhap, Email, MatKhau, HoTen, VaiTro) 
VALUES ('user2', 'user2@email.com', 'user123', 'Trần Thị B', 'User');

-- Chèn dữ liệu mẫu cho công việc
INSERT INTO CongViec (TieuDe, MoTa, YeuCau, PhucLoi, DiaDiem, LoaiCongViec, LuongToiThieu, LuongToiDa, CongTy, DanhMuc, NamKinhNghiem) VALUES
('Lập trình viên .NET Senior', 'Phát triển và duy trì các ứng dụng web sử dụng .NET Core, ASP.NET MVC. Làm việc trong môi trường năng động, có cơ hội thăng tiến cao.', 'Tốt nghiệp Đại học chuyên ngành CNTT hoặc tương đương. Thành thạo C#, ASP.NET Core, Entity Framework, SQL Server. Có ít nhất 3 năm kinh nghiệm phát triển web.', 'Lương cạnh tranh 15-25 triệu, bảo hiểm đầy đủ, thưởng theo hiệu suất, môi trường làm việc năng động, cơ hội thăng tiến', 'Hà Nội', 'Toàn thời gian', 15000000, 25000000, 'Công ty TNHH Công nghệ ABC', 'Công nghệ thông tin', 3),

('Thiết kế UI/UX Designer', 'Thiết kế giao diện người dùng cho các ứng dụng web và mobile. Làm việc với team phát triển để tạo ra trải nghiệm người dùng tốt nhất.', 'Tốt nghiệp Cao đẳng trở lên chuyên ngành Thiết kế đồ họa hoặc tương đương. Thành thạo Figma, Adobe XD, Photoshop. Có kinh nghiệm thiết kế responsive web.', 'Lương 12-20 triệu, thưởng sáng tạo, môi trường làm việc sáng tạo, được học hỏi công nghệ mới', 'TP. Hồ Chí Minh', 'Toàn thời gian', 12000000, 20000000, 'Công ty TNHH Thiết kế XYZ', 'Thiết kế', 2),

('Chuyên viên Marketing Digital', 'Xây dựng và thực hiện các chiến lược marketing digital, quản lý các kênh social media, chạy quảng cáo Google Ads và Facebook Ads.', 'Tốt nghiệp Đại học chuyên ngành Marketing hoặc tương đương. Có kinh nghiệm digital marketing, SEO, Google Ads, Facebook Ads. Kỹ năng phân tích dữ liệu tốt.', 'Lương 10-18 triệu, thưởng theo hiệu suất, cơ hội học hỏi, môi trường năng động', 'Đà Nẵng', 'Toàn thời gian', 10000000, 18000000, 'Công ty TNHH Marketing DEF', 'Marketing', 2),

('Kế toán viên', 'Thực hiện các công việc kế toán, quản lý tài chính, lập báo cáo tài chính. Làm việc trong môi trường chuyên nghiệp.', 'Tốt nghiệp Cao đẳng trở lên chuyên ngành Kế toán. Thành thạo Excel, phần mềm kế toán. Có chứng chỉ kế toán viên là một lợi thế.', 'Lương 8-15 triệu, bảo hiểm đầy đủ, môi trường làm việc ổn định, cơ hội phát triển nghề nghiệp', 'Hà Nội', 'Toàn thời gian', 8000000, 15000000, 'Công ty TNHH Tài chính GHI', 'Tài chính - Kế toán', 1),

('Nhân viên bán hàng', 'Tìm kiếm và phát triển khách hàng mới, duy trì mối quan hệ với khách hàng hiện tại. Đạt doanh số bán hàng theo mục tiêu.', 'Tốt nghiệp Trung cấp trở lên. Có kỹ năng giao tiếp tốt, năng động, nhiệt tình. Có kinh nghiệm bán hàng là một lợi thế.', 'Lương cơ bản 6-12 triệu + hoa hồng, thưởng doanh số, cơ hội thăng tiến, môi trường làm việc năng động', 'TP. Hồ Chí Minh', 'Toàn thời gian', 6000000, 12000000, 'Công ty TNHH Thương mại JKL', 'Bán hàng', 0),

('Lập trình viên Frontend', 'Phát triển giao diện người dùng cho các ứng dụng web sử dụng React, Vue.js. Làm việc với team backend để tích hợp API.', 'Tốt nghiệp Đại học chuyên ngành CNTT. Thành thạo HTML, CSS, JavaScript, React/Vue.js. Có kinh nghiệm làm việc với REST API.', 'Lương 12-22 triệu, thưởng dự án, môi trường làm việc hiện đại, cơ hội học hỏi công nghệ mới', 'Hà Nội', 'Toàn thời gian', 12000000, 22000000, 'Công ty TNHH Phần mềm MNO', 'Công nghệ thông tin', 2),

('Nhân viên nhân sự', 'Tuyển dụng, quản lý hồ sơ nhân viên, tổ chức các hoạt động team building. Làm việc trong môi trường năng động.', 'Tốt nghiệp Đại học chuyên ngành Quản trị nhân lực hoặc tương đương. Có kỹ năng giao tiếp tốt, tổ chức sự kiện. Thành thạo Excel, PowerPoint.', 'Lương 8-14 triệu, bảo hiểm đầy đủ, môi trường làm việc thân thiện, cơ hội phát triển', 'Đà Nẵng', 'Toàn thời gian', 8000000, 14000000, 'Công ty TNHH Dịch vụ PQR', 'Nhân sự', 1),

('Kỹ sư DevOps', 'Xây dựng và quản lý hệ thống CI/CD, triển khai ứng dụng lên cloud. Làm việc với các công nghệ container hóa.', 'Tốt nghiệp Đại học chuyên ngành CNTT. Thành thạo Docker, Kubernetes, AWS/Azure. Có kinh nghiệm với Linux, Git, Jenkins.', 'Lương 18-30 triệu, thưởng dự án, môi trường làm việc hiện đại, cơ hội học hỏi công nghệ cloud', 'TP. Hồ Chí Minh', 'Toàn thời gian', 18000000, 30000000, 'Công ty TNHH Cloud STU', 'Công nghệ thông tin', 3);

-- Chèn dữ liệu mẫu cho ứng viên
INSERT INTO UngVien (HoTen, Email, SoDienThoai, DiaChi, NgaySinh, GioiTinh, TrinhDoHocVan, KinhNghiem, KyNang) VALUES
('Nguyễn Văn An', 'nguyenvanan@email.com', '0123456789', '123 Đường Láng, Đống Đa, Hà Nội', '1990-01-15', 'Nam', 'Đại học', '3 năm kinh nghiệm lập trình .NET, đã từng làm việc tại các công ty công nghệ lớn', 'C#, ASP.NET Core, Entity Framework, SQL Server, JavaScript, HTML/CSS, Git'),

('Trần Thị Bình', 'tranthibinh@email.com', '0987654321', '456 Nguyễn Huệ, Quận 1, TP.HCM', '1992-05-20', 'Nữ', 'Cao đẳng', '2 năm kinh nghiệm thiết kế UI/UX, đã thiết kế nhiều ứng dụng web và mobile', 'Figma, Adobe XD, Photoshop, Illustrator, Sketch, HTML/CSS, JavaScript'),

('Lê Văn Cường', 'levancuong@email.com', '0369258147', '789 Lê Duẩn, Hải Châu, Đà Nẵng', '1988-12-10', 'Nam', 'Đại học', '4 năm kinh nghiệm marketing digital, đã quản lý nhiều chiến dịch quảng cáo thành công', 'SEO, Google Ads, Facebook Ads, Google Analytics, Content Marketing, Social Media Marketing'),

('Phạm Thị Dung', 'phamthidung@email.com', '0147258369', '321 Trần Hưng Đạo, Hoàn Kiếm, Hà Nội', '1991-08-25', 'Nữ', 'Đại học', '3 năm kinh nghiệm kế toán, đã làm việc tại các công ty tài chính', 'Excel, SAP, QuickBooks, Kế toán doanh nghiệp, Thuế, Báo cáo tài chính'),

('Hoàng Văn Em', 'hoangvanem@email.com', '0258147369', '654 Nguyễn Văn Cừ, Quận 5, TP.HCM', '1993-03-30', 'Nam', 'Cao đẳng', '2 năm kinh nghiệm bán hàng, đã đạt nhiều thành tích xuất sắc', 'Giao tiếp, Thuyết trình, CRM, Bán hàng B2B, Chăm sóc khách hàng'),

('Võ Thị Phương', 'vothiphuong@email.com', '0369258147', '987 Lê Lợi, Hải Châu, Đà Nẵng', '1994-07-12', 'Nữ', 'Đại học', '1 năm kinh nghiệm frontend, đã tham gia nhiều dự án web', 'HTML, CSS, JavaScript, React, Vue.js, Bootstrap, Git'),

('Đặng Văn Giang', 'dangvangiang@email.com', '0147258369', '147 Hoàng Hoa Thám, Tân Bình, TP.HCM', '1989-11-05', 'Nam', 'Đại học', '5 năm kinh nghiệm DevOps, chuyên về cloud và container', 'Docker, Kubernetes, AWS, Azure, Linux, Git, Jenkins, CI/CD'),

('Bùi Thị Hoa', 'buithihoa@email.com', '0258147369', '258 Cầu Giấy, Cầu Giấy, Hà Nội', '1992-09-18', 'Nữ', 'Đại học', '2 năm kinh nghiệm nhân sự, đã tổ chức nhiều sự kiện thành công', 'Tuyển dụng, Quản lý nhân sự, Tổ chức sự kiện, Excel, PowerPoint, HRIS');

-- Chèn dữ liệu mẫu cho đơn ứng tuyển
INSERT INTO DonUngTuyen (MaCongViec, HoTen, Email, SoDienThoai, DiaChi, NgaySinh, GioiTinh, TrinhDoHocVan, KinhNghiem, KyNang, TrangThai) VALUES
(1, 'Nguyễn Văn An', 'nguyenvanan@email.com', '0123456789', '123 Đường Láng, Đống Đa, Hà Nội', '1990-01-15', 'Nam', 'Đại học', '3 năm kinh nghiệm lập trình .NET', 'C#, ASP.NET Core, Entity Framework, SQL Server', 'Chờ xem xét'),

(2, 'Trần Thị Bình', 'tranthibinh@email.com', '0987654321', '456 Nguyễn Huệ, Quận 1, TP.HCM', '1992-05-20', 'Nữ', 'Cao đẳng', '2 năm kinh nghiệm thiết kế UI/UX', 'Figma, Adobe XD, Photoshop, Illustrator', 'Đã phỏng vấn'),

(3, 'Lê Văn Cường', 'levancuong@email.com', '0369258147', '789 Lê Duẩn, Hải Châu, Đà Nẵng', '1988-12-10', 'Nam', 'Đại học', '4 năm kinh nghiệm marketing digital', 'SEO, Google Ads, Facebook Ads, Google Analytics', 'Đã từ chối'),

(4, 'Phạm Thị Dung', 'phamthidung@email.com', '0147258369', '321 Trần Hưng Đạo, Hoàn Kiếm, Hà Nội', '1991-08-25', 'Nữ', 'Đại học', '3 năm kinh nghiệm kế toán', 'Excel, SAP, QuickBooks, Kế toán doanh nghiệp', 'Chờ xem xét'),

(5, 'Hoàng Văn Em', 'hoangvanem@email.com', '0258147369', '654 Nguyễn Văn Cừ, Quận 5, TP.HCM', '1993-03-30', 'Nam', 'Cao đẳng', '2 năm kinh nghiệm bán hàng', 'Giao tiếp, Thuyết trình, CRM, Bán hàng B2B', 'Đã phỏng vấn'),

(6, 'Võ Thị Phương', 'vothiphuong@email.com', '0369258147', '987 Lê Lợi, Hải Châu, Đà Nẵng', '1994-07-12', 'Nữ', 'Đại học', '1 năm kinh nghiệm frontend', 'HTML, CSS, JavaScript, React, Vue.js', 'Chờ xem xét'),

(7, 'Đặng Văn Giang', 'dangvangiang@email.com', '0147258369', '147 Hoàng Hoa Thám, Tân Bình, TP.HCM', '1989-11-05', 'Nam', 'Đại học', '5 năm kinh nghiệm DevOps', 'Docker, Kubernetes, AWS, Azure, Linux', 'Đã phỏng vấn'),

(8, 'Bùi Thị Hoa', 'buithihoa@email.com', '0258147369', '258 Cầu Giấy, Cầu Giấy, Hà Nội', '1992-09-18', 'Nữ', 'Đại học', '2 năm kinh nghiệm nhân sự', 'Tuyển dụng, Quản lý nhân sự, Tổ chức sự kiện', 'Chờ xem xét');

-- =============================================
-- PHẦN 5: TẠO INDEX
-- =============================================

CREATE INDEX IX_NguoiDung_TenDangNhap ON NguoiDung(TenDangNhap);
CREATE INDEX IX_NguoiDung_Email ON NguoiDung(Email);
CREATE INDEX IX_CongViec_DiaDiem ON CongViec(DiaDiem);
CREATE INDEX IX_CongViec_LoaiCongViec ON CongViec(LoaiCongViec);
CREATE INDEX IX_CongViec_DanhMuc ON CongViec(DanhMuc);
CREATE INDEX IX_CongViec_HoatDong ON CongViec(HoatDong);
CREATE INDEX IX_DonUngTuyen_MaCongViec ON DonUngTuyen(MaCongViec);
CREATE INDEX IX_DonUngTuyen_TrangThai ON DonUngTuyen(TrangThai);
CREATE INDEX IX_DonUngTuyen_NgayUngTuyen ON DonUngTuyen(NgayUngTuyen);
CREATE INDEX IX_UngVien_Email ON UngVien(Email);
CREATE INDEX IX_UngVien_NgayTao ON UngVien(NgayTao);
GO

-- =============================================
-- PHẦN 6: TẠO VIEW
-- =============================================

CREATE VIEW ThongKeCongViec AS
SELECT 
    COUNT(*) as TongCongViec,
    COUNT(CASE WHEN HoatDong = 1 THEN 1 END) as CongViecHoatDong,
    COUNT(CASE WHEN HoatDong = 0 THEN 1 END) as CongViecKhongHoatDong,
    AVG(LuongToiThieu) as LuongTrungBinh,
    COUNT(CASE WHEN NgayDang >= DATEADD(day, -30, GETDATE()) THEN 1 END) as CongViecMoiTrong30Ngay
FROM CongViec;
GO

CREATE VIEW ThongKeUngTuyen AS
SELECT 
    COUNT(*) as TongDonUngTuyen,
    COUNT(CASE WHEN TrangThai = 'Chờ xem xét' THEN 1 END) as ChoXemXet,
    COUNT(CASE WHEN TrangThai = 'Đã phỏng vấn' THEN 1 END) as DaPhongVan,
    COUNT(CASE WHEN TrangThai = 'Đã từ chối' THEN 1 END) as DaTuChoi,
    COUNT(CASE WHEN NgayUngTuyen >= DATEADD(day, -7, GETDATE()) THEN 1 END) as DonMoiTrong7Ngay
FROM DonUngTuyen;
GO

CREATE VIEW ThongKeNguoiDung AS
SELECT 
    COUNT(*) as TongNguoiDung,
    COUNT(CASE WHEN VaiTro = 'Admin' THEN 1 END) as SoAdmin,
    COUNT(CASE WHEN VaiTro = 'User' THEN 1 END) as SoUser,
    COUNT(CASE WHEN HoatDong = 1 THEN 1 END) as NguoiDungHoatDong,
    COUNT(CASE WHEN NgayTao >= DATEADD(day, -30, GETDATE()) THEN 1 END) as NguoiDungMoiTrong30Ngay
FROM NguoiDung;
GO

-- =============================================
-- PHẦN 7: TẠO STORED PROCEDURE
-- =============================================

CREATE PROCEDURE TimKiemCongViec
    @DiaDiem NVARCHAR(100) = NULL,
    @LoaiCongViec NVARCHAR(50) = NULL,
    @DanhMuc NVARCHAR(100) = NULL,
    @TimKiem NVARCHAR(200) = NULL,
    @LuongToiThieu DECIMAL(18,2) = NULL,
    @LuongToiDa DECIMAL(18,2) = NULL
AS
BEGIN
    SELECT * FROM CongViec 
    WHERE HoatDong = 1
    AND (@DiaDiem IS NULL OR DiaDiem LIKE '%' + @DiaDiem + '%')
    AND (@LoaiCongViec IS NULL OR LoaiCongViec = @LoaiCongViec)
    AND (@DanhMuc IS NULL OR DanhMuc = @DanhMuc)
    AND (@TimKiem IS NULL OR TieuDe LIKE '%' + @TimKiem + '%' OR MoTa LIKE '%' + @TimKiem + '%' OR CongTy LIKE '%' + @TimKiem + '%')
    AND (@LuongToiThieu IS NULL OR LuongToiThieu >= @LuongToiThieu)
    AND (@LuongToiDa IS NULL OR LuongToiDa <= @LuongToiDa)
    ORDER BY NgayDang DESC;
END
GO

CREATE PROCEDURE DangNhap
    @TenDangNhap NVARCHAR(50),
    @MatKhau NVARCHAR(100)
AS
BEGIN
    SELECT Id, TenDangNhap, Email, HoTen, VaiTro, HoatDong, NgayTao
    FROM NguoiDung 
    WHERE TenDangNhap = @TenDangNhap 
    AND MatKhau = @MatKhau 
    AND HoatDong = 1;
END
GO

CREATE PROCEDURE CapNhatTrangThaiUngTuyen
    @MaDonUngTuyen INT,
    @TrangThai NVARCHAR(50)
AS
BEGIN
    UPDATE DonUngTuyen 
    SET TrangThai = @TrangThai 
    WHERE MaDonUngTuyen = @MaDonUngTuyen;
    
    SELECT @@ROWCOUNT as SoDongDuocCapNhat;
END
GO

CREATE PROCEDURE LayThongKeTongQuan
AS
BEGIN
    SELECT 
        (SELECT TongCongViec FROM ThongKeCongViec) as TongCongViec,
        (SELECT CongViecHoatDong FROM ThongKeCongViec) as CongViecHoatDong,
        (SELECT TongDonUngTuyen FROM ThongKeUngTuyen) as TongDonUngTuyen,
        (SELECT ChoXemXet FROM ThongKeUngTuyen) as DonChoXemXet,
        (SELECT TongNguoiDung FROM ThongKeNguoiDung) as TongNguoiDung,
        (SELECT LuongTrungBinh FROM ThongKeCongViec) as LuongTrungBinh;
END
GO

CREATE PROCEDURE LayCongViecNoiBat
    @SoLuong INT = 6
AS
BEGIN
    SELECT TOP(@SoLuong) * FROM CongViec 
    WHERE HoatDong = 1 
    ORDER BY NgayDang DESC, LuotXem DESC;
END
GO

-- =============================================
-- PHẦN 8: TẠO TRIGGER
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

CREATE TRIGGER TR_LogDonUngTuyen
ON DonUngTuyen
AFTER INSERT
AS
BEGIN
    PRINT 'Có đơn ứng tuyển mới được tạo';
END
GO

-- =============================================
-- PHẦN 9: TẠO USER VÀ PHÂN QUYỀN
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
-- PHẦN 10: KIỂM TRA VÀ HOÀN THÀNH
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

-- Kiểm tra thống kê
PRINT 'THỐNG KÊ TỔNG QUAN:';
EXEC LayThongKeTongQuan;

PRINT '=============================================';
PRINT 'ĐÃ TẠO THÀNH CÔNG DATABASE HeThongTuyenDung (PHIÊN BẢN TIẾNG VIỆT)';
PRINT '=============================================';
PRINT 'THÔNG TIN ĐĂNG NHẬP ADMIN:';
PRINT 'Tên đăng nhập: admin';
PRINT 'Mật khẩu: admin123';
PRINT '=============================================';
PRINT 'CÁC BẢNG ĐÃ TẠO:';
PRINT '- NguoiDung (Người dùng)';
PRINT '- CongViec (Công việc)';
PRINT '- UngVien (Ứng viên)';
PRINT '- DonUngTuyen (Đơn ứng tuyển)';
PRINT '=============================================';
PRINT 'DỮ LIỆU MẪU:';
PRINT '- 3 người dùng (1 admin, 2 user)';
PRINT '- 8 công việc đa dạng';
PRINT '- 8 ứng viên';
PRINT '- 8 đơn ứng tuyển';
PRINT '=============================================';
PRINT 'STORED PROCEDURES:';
PRINT '- TimKiemCongViec: Tìm kiếm công việc nâng cao';
PRINT '- DangNhap: Đăng nhập người dùng';
PRINT '- CapNhatTrangThaiUngTuyen: Cập nhật trạng thái ứng tuyển';
PRINT '- LayThongKeTongQuan: Lấy thống kê tổng quan';
PRINT '- LayCongViecNoiBat: Lấy công việc nổi bật';
PRINT '=============================================';
PRINT 'VIEWS:';
PRINT '- ThongKeCongViec: Thống kê công việc';
PRINT '- ThongKeUngTuyen: Thống kê ứng tuyển';
PRINT '- ThongKeNguoiDung: Thống kê người dùng';
PRINT '=============================================';
PRINT 'TÍNH NĂNG ĐẶC BIỆT:';
PRINT '- Hỗ trợ tìm kiếm nâng cao theo nhiều tiêu chí';
PRINT '- Thống kê chi tiết và real-time';
PRINT '- Trigger tự động log các thay đổi';
PRINT '- Index tối ưu hiệu suất truy vấn';
PRINT '- Collation tiếng Việt hỗ trợ tìm kiếm tiếng Việt';
PRINT '=============================================';
GO