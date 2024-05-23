IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'DBQuanLyXe')
BEGIN
    CREATE DATABASE DBQuanLyXe;
END
GO

USE DBQuanLyXe;

CREATE TABLE KhachHang (
    IdKhachHang INT PRIMARY KEY,
    HoTen NVARCHAR(MAX) NOT NULL,
    SoDienThoai NVARCHAR(MAX) NOT NULL,
    Email NVARCHAR(MAX) NOT NULL,
    TenDangNhap NVARCHAR(MAX) NOT NULL,
    MatKhau NVARCHAR(MAX) NOT NULL,
    CHECK (LEN(SoDienThoai) > 0) -- Kiểm tra độ dài số điện thoại
);

CREATE TABLE HangXe (
    IdHangXe INT PRIMARY KEY,
    TenHangXe NVARCHAR(MAX) NOT NULL,
    DiaChiHangXe NVARCHAR(MAX) NOT NULL,
    TenDangNhap NVARCHAR(MAX) NOT NULL,
    MatKhau NVARCHAR(MAX) NOT NULL,
    email NVARCHAR(MAX) NOT NULL,
    SoDienThoai INT,
    CHECK (SoDienThoai >= 0) -- Kiểm tra số điện thoại không âm
);

CREATE TABLE NhanVien (
    IdNhanVien INT PRIMARY KEY,
    HoTen NVARCHAR(MAX) NOT NULL,
    NgaySinh DATETIME,
    SoDienThoai INT,
    ChucVu NVARCHAR(MAX) NOT NULL,
    email NVARCHAR(MAX) NOT NULL,
    IdHangXe INT
);

CREATE TABLE XeKhach (
    IdXeKhach INT PRIMARY KEY,
    BienSoXe NVARCHAR(MAX) NOT NULL,
    SoGhe INT,
    LoaiXe NVARCHAR(MAX) NOT NULL,
    TenLoaiXe NVARCHAR(MAX) NOT NULL,
    IdHangXe INT
);

CREATE TABLE DanhSachGhe (
    IdGhe INT PRIMARY KEY,
    IdXeKhach INT,
    SoThuTu INT,
    TinhTrang NVARCHAR(MAX)
);

CREATE TABLE LichTrinh (
    IdLichTrinh INT PRIMARY KEY,
    IdVeXe INT,
    NgayDi DATE,
    GioDi TIME
);

CREATE TABLE VeXe (
    IdVeXe INT PRIMARY KEY,
    ThanhTien DECIMAL,
    SoLuongVe TINYINT,
    NgayDat DATETIME,
    TrangThai NVARCHAR(MAX),
    IdLichTrinh INT,
    IdKhachHang INT,
    GhiChu NVARCHAR(MAX)
);

CREATE TABLE ChuyenDi (
    IdChuyenDi INT PRIMARY KEY,
    DiemXuatPhat NVARCHAR(MAX) NOT NULL,
    DiemDen NVARCHAR(MAX) NOT NULL,
    Gia DECIMAL,
    ThoiGianDi DATETIME,
    ThoiGianDen DATETIME,
    IdNhanVien INT,
    IdXeKhach INT,
    IdLichTrinh INT,
    DiemDiCuThe NVARCHAR(MAX) NOT NULL,
    DiemDenCuThe NVARCHAR(MAX) NOT NULL,
    ThoiGianDuKien TINYINT
);

CREATE TABLE ThanhToan (
    IdThanhToan INT PRIMARY KEY,
    IdVeXe INT,
    SoTienThanhToan DECIMAL,
    NgayThanhToan DATETIME,
    ThoiGianThanhToan DATETIME
);

-- Thêm các khóa ngoại sau khi tất cả các bảng đã được tạo
ALTER TABLE NhanVien
ADD CONSTRAINT FK_NhanVien_HangXe FOREIGN KEY (IdHangXe) REFERENCES HangXe(IdHangXe);

ALTER TABLE XeKhach
ADD CONSTRAINT FK_XeKhach_HangXe FOREIGN KEY (IdHangXe) REFERENCES HangXe(IdHangXe);

ALTER TABLE DanhSachGhe
ADD CONSTRAINT FK_DanhSachGhe_XeKhach FOREIGN KEY (IdXeKhach) REFERENCES XeKhach(IdXeKhach);

ALTER TABLE LichTrinh
ADD CONSTRAINT FK_LichTrinh_VeXe FOREIGN KEY (IdVeXe) REFERENCES VeXe(IdVeXe);


ALTER TABLE VeXe ADD CONSTRAINT FK_VeXe_KhachHang FOREIGN KEY (IdKhachHang) REFERENCES KhachHang(IdKhachHang)
ALTER TABLE VeXe ADD CONSTRAINT FK_VeXe_LichTrinh FOREIGN KEY (IdLichTrinh) REFERENCES LichTrinh(IdLichTrinh)


ALTER TABLE ChuyenDi ADD CONSTRAINT FK_ChuyenDi_NhanVien FOREIGN KEY (IdNhanVien) REFERENCES NhanVien(IdNhanVien)
ALTER TABLE ChuyenDi ADD CONSTRAINT FK_ChuyenDi_XeKhach FOREIGN KEY (IdXeKhach) REFERENCES XeKhach(IdXeKhach)
ALTER TABLE ChuyenDi ADD CONSTRAINT FK_ChuyenDi_LichTrinh FOREIGN KEY (IdLichTrinh) REFERENCES LichTrinh(IdLichTrinh)

ALTER TABLE ThanhToan
ADD CONSTRAINT FK_ThanhToan_VeXe FOREIGN KEY (IdVeXe) REFERENCES VeXe(IdVeXe);
