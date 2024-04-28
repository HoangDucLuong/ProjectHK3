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

-- Bảng TaiKhoanMatKhau
CREATE TABLE TaiKhoanMatKhau (
    MaTaiKhoan INT PRIMARY KEY IDENTITY,
    TaiKhoan VARCHAR(50) UNIQUE NOT NULL,
    MatKhau VARCHAR(256) NOT NULL,
    Role INT NOT NULL -- Vai trò của người dùng: 1 là admin, 2 là nhân viên, 3 là khách hàng
);

-- Bảng NhanVien
CREATE TABLE NhanVien (
    MaNhanVien INT PRIMARY KEY IDENTITY,
    TenNhanVien NVARCHAR(255) NOT NULL,
    MaTaiKhoan INT UNIQUE NOT NULL,
    FOREIGN KEY (MaTaiKhoan) REFERENCES TaiKhoanMatKhau(MaTaiKhoan)
);

-- Bảng Khách hàng
CREATE TABLE KhachHang (
    MaKhachHang INT PRIMARY KEY IDENTITY,
    TenKhachHang NVARCHAR(255),
    DiaChi NVARCHAR(255),
    Email VARCHAR(100),
    SoDienThoai VARCHAR(15),
    MaTaiKhoan INT UNIQUE NOT NULL,
    FOREIGN KEY (MaTaiKhoan) REFERENCES TaiKhoanMatKhau(MaTaiKhoan)
);

-- Bảng Loại sản phẩm
CREATE TABLE LoaiSanPham (
    MaLoai INT PRIMARY KEY IDENTITY,
    TenLoai NVARCHAR(100) NOT NULL
);

-- Bảng SanPham
CREATE TABLE SanPham (
    MaSanPham INT PRIMARY KEY IDENTITY(1,1),
    MaSoSanPham NVARCHAR(7) UNIQUE NULL,
    TenSanPham NVARCHAR(255) NOT NULL,
    MaLoai INT,
    MoTaSanPham NVARCHAR(255),
    SoLuong INT NOT NULL DEFAULT 0,
    Gia DECIMAL(10, 2) NOT NULL
);

-- Bảng Loại Giao hàng
CREATE TABLE LoaiGiaoHang (
    MaLoaiGiaoHang INT PRIMARY KEY IDENTITY,
    TenLoaiGiaoHang NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE DonDatHang (
    MaDonHang NVARCHAR(16) PRIMARY KEY, 
    MaKhachHang INT,
    MaSoSanPham NVARCHAR(7) NULL,
	SoLuongMua INT NOT NULL DEFAULT 1,
    NgayDat DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ThanhToan VARCHAR(255) NOT NULL,
    MaLoaiGiaoHang INT NOT NULL,
    FOREIGN KEY (MaKhachHang) REFERENCES KhachHang(MaKhachHang),
    FOREIGN KEY (MaSoSanPham) REFERENCES SanPham(MaSoSanPham),
    FOREIGN KEY (MaLoaiGiaoHang) REFERENCES LoaiGiaoHang(MaLoaiGiaoHang)
);
GO

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

    UPDATE sp
    SET sp.MaSoSanPham = RIGHT('00' + CAST(it.MaSanPham AS NVARCHAR(2)), 2) + RIGHT('00000' + CAST(it.SoLuong AS NVARCHAR(5)), 5),
        sp.SoLuong = it.SoLuong
    FROM SanPham sp
    INNER JOIN @InsertedTable it ON sp.MaSanPham = it.MaSanPham;
END;
GO

CREATE TRIGGER Trg_DonDatHang_Insert
ON DonDatHang
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO DonDatHang(MaDonHang, MaKhachHang, MaSoSanPham, SoLuongMua, NgayDat, ThanhToan, MaLoaiGiaoHang)
    SELECT 
        FORMAT(i.MaLoaiGiaoHang, 'D1') -- Loại giao hàng 1 chữ số
        + RIGHT('0000000' + RTRIM(i.MaSoSanPham), 7) -- Mã sản phẩm 7 chữ số, đảm bảo mã sản phẩm không dài hơn 7 ký tự
        + RIGHT('00000000' + CONVERT(VARCHAR, CAST(DATEDIFF(SECOND, '1970-01-01', GETDATE()) % 100000000 AS INT)), 8), -- Số đơn hàng 8 chữ số
        i.MaKhachHang,
        i.MaSoSanPham,
        i.SoLuongMua,
        i.NgayDat,
        i.ThanhToan,
        i.MaLoaiGiaoHang
    FROM inserted i;
    
    -- Cập nhật số lượng tồn sau khi đơn đặt hàng được tạo
    UPDATE sp
    SET sp.SoLuong = sp.SoLuong - ins.SoLuongMua
    FROM SanPham sp
    INNER JOIN INSERTED ins ON sp.MaSoSanPham = ins.MaSoSanPham;
END;
GO

CREATE TRIGGER Trg_SanPham_Update
ON SanPham
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF UPDATE(SoLuong)
    BEGIN
        DECLARE @Count int;

        SELECT @Count = COUNT(*)
        FROM DonDatHang
        WHERE MaSoSanPham IN (SELECT MaSoSanPham FROM inserted);

        -- Nếu MaSoSanPham đã được tham chiếu, không cập nhật MaSoSanPham
        IF @Count = 0
        BEGIN
            UPDATE sp
            SET sp.MaSoSanPham = RIGHT('00' + CAST(sp.MaSanPham AS NVARCHAR(2)), 2) + RIGHT('00000' + CAST(sp.SoLuong AS NVARCHAR(5)), 5)
            FROM SanPham sp
            INNER JOIN inserted i ON sp.MaSanPham = i.MaSanPham;
        END
        ELSE
        BEGIN
            -- Handle the case where MaSoSanPham is referenced in DonDatHang
            PRINT 'Cannot update MaSoSanPham because it is referenced in DonDatHang';
        END
    END
END;
GO

CREATE TRIGGER trg_AutoCreateKhachHang
ON TaiKhoanMatKhau
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO KhachHang (TenKhachHang, DiaChi, Email, SoDienThoai, MaTaiKhoan)
    SELECT NULL, NULL, i.TaiKhoan, NULL, i.MaTaiKhoan
    FROM inserted i
    WHERE i.Role = 3;
END;
GO


