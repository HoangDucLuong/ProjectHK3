USE master
GO

-- Xóa database cũ nếu tồn tại
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'ProjectHK3')
    DROP DATABASE ProjectHK3
GO

-- Tạo database mới
CREATE DATABASE ProjectHK3
GO

USE ProjectHK3
GO

-- Bảng TaiKhoanMatKhau (đã có trong mã ban đầu)
CREATE TABLE TaiKhoanMatKhau (
    MaTaiKhoan INT PRIMARY KEY IDENTITY,
    TaiKhoan VARCHAR(50) UNIQUE NOT NULL,
    MatKhau VARCHAR(256) NOT NULL,
    Role INT NOT NULL -- Vai trò của người dùng: 1 là admin, 2 là nhân viên, 3 là khách hàng
);

-- Bảng NhanVien (đã có trong mã ban đầu)
CREATE TABLE NhanVien (
    MaNhanVien INT PRIMARY KEY IDENTITY,
    TenNhanVien NVARCHAR(255) NOT NULL,
    MaTaiKhoan INT UNIQUE NOT NULL,
    FOREIGN KEY (MaTaiKhoan) REFERENCES TaiKhoanMatKhau(MaTaiKhoan)
);

-- Bảng Khách hàng (đã có trong mã ban đầu)
CREATE TABLE KhachHang (
    MaKhachHang INT PRIMARY KEY IDENTITY,
    TenKhachHang NVARCHAR(255) NOT NULL,
    DiaChi NVARCHAR(255) NOT NULL,
    Email VARCHAR(100),
    SoDienThoai VARCHAR(15),
    MaTaiKhoan INT UNIQUE NOT NULL,
    FOREIGN KEY (MaTaiKhoan) REFERENCES TaiKhoanMatKhau(MaTaiKhoan)
);

-- Bảng Loại sản phẩm (đã có trong mã ban đầu)
CREATE TABLE LoaiSanPham (
    MaLoai INT PRIMARY KEY IDENTITY,
    TenLoai NVARCHAR(100) NOT NULL
);

-- Bảng SanPham (đã có trong mã ban đầu)
CREATE TABLE SanPham (
    MaSanPham INT PRIMARY KEY IDENTITY(1,1),
    MaSoSanPham NVARCHAR(7) NULL,
    TenSanPham NVARCHAR(255) NOT NULL,
    MaLoai INT,
    MoTaSanPham NVARCHAR(255),
    SoLuong INT NOT NULL DEFAULT 0,
    Gia DECIMAL(10, 2) NOT NULL
);

-- Bảng Loại Giao hàng (đã có trong mã ban đầu)
CREATE TABLE LoaiGiaoHang (
    MaLoaiGiaoHang INT PRIMARY KEY IDENTITY,
    TenLoaiGiaoHang NVARCHAR(100) NOT NULL
);
GO

-- Sửa lại định nghĩa bảng DonDatHang để tạo cột tính toán MaDonHang làm khóa chính
CREATE TABLE DonDatHang (
    MaDonHang NVARCHAR(16) PRIMARY KEY, -- Định nghĩa MaDonHang là khóa chính
    MaKhachHang INT,
    MaSanPham INT,
    LoaiGiaoHang INT NOT NULL,
    NgayDat DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ThanhToan VARCHAR(255) NOT NULL,
    MaLoaiGiaoHang INT NOT NULL,
    FOREIGN KEY (MaKhachHang) REFERENCES KhachHang(MaKhachHang),
    FOREIGN KEY (MaSanPham) REFERENCES SanPham(MaSanPham),
    FOREIGN KEY (MaLoaiGiaoHang) REFERENCES LoaiGiaoHang(MaLoaiGiaoHang)
);
GO

-- Tiếp tục thêm bảng ThanhToan như trước
CREATE TABLE ThanhToan (
    MaThanhToan INT PRIMARY KEY IDENTITY,
    MaDonHang NVARCHAR(16) NOT NULL,
    MaKhachHang INT,
    PhuongThucThanhToan VARCHAR(255) NOT NULL,
    ThoiGian DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (MaDonHang) REFERENCES DonDatHang(MaDonHang),
    FOREIGN KEY (MaKhachHang) REFERENCES KhachHang(MaKhachHang)
);
GO

-- Trigger mới cho bảng SanPham để tạo mã sản phẩm
CREATE TRIGGER Trg_SanPham_Insert
ON SanPham
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @InsertedTable TABLE (MaSanPham INT, MaLoai INT, SoLuong INT);
    INSERT INTO @InsertedTable (MaSanPham, MaLoai, SoLuong)
    SELECT MaSanPham, MaLoai, SoLuong FROM INSERTED;

    -- Cập nhật mã sản phẩm dựa trên MaLoai và MaSanPham
    UPDATE sp
    SET sp.MaSoSanPham = RIGHT('00' + CAST(it.MaSanPham AS NVARCHAR(2)), 2) + RIGHT('00000' + CAST(it.SoLuong AS NVARCHAR(5)), 5),
        sp.SoLuong = it.SoLuong
    FROM SanPham sp
    INNER JOIN @InsertedTable it ON sp.MaSanPham = it.MaSanPham;
END;
GO

-- Trigger mới cho bảng DonDatHang để tạo mã đơn hàng 16 ký tự
CREATE TRIGGER Trg_DonDatHang_Insert
ON DonDatHang
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Id NVARCHAR(16), @LoaiGiaoHang INT, @MaSanPham INT, @Timestamp BIGINT, @SoLuong INT;
    SELECT @LoaiGiaoHang = LoaiGiaoHang, @MaSanPham = SanPham.MaSanPham, @SoLuong = (SELECT SoLuong FROM INSERTED WHERE MaSanPham = SanPham.MaSanPham)
    FROM INSERTED
    JOIN SanPham ON INSERTED.MaSanPham = SanPham.MaSanPham;
    SELECT @Id = MaSoSanPham FROM SanPham WHERE MaSanPham = @MaSanPham;
    SET @Timestamp = DATEDIFF(SECOND, '19700101', GETDATE()) % 100000000;
    UPDATE DonDatHang
    SET MaDonHang = CONVERT(VARCHAR(1), @LoaiGiaoHang) + @Id + RIGHT('00000000' + CONVERT(VARCHAR(8), @Timestamp), 8)
    WHERE MaDonHang IS NULL; 

    -- Cập nhật số lượng sản phẩm
    UPDATE SanPham
    SET SoLuong = SoLuong - @SoLuong
    WHERE MaSanPham = @MaSanPham;
END;
GO
