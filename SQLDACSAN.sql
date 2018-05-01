use master
drop database SHOPDACSAN
-------------------------
create database SHOPDACSAN
go
use SHOPDACSAN
go
--
GO
CREATE TABLE NHACUNGCAP
(
	MANCC VARCHAR(7) ,
	TENNCC NVARCHAR(50),
	DIACHINCC NVARCHAR(50),
	DTNCC VARCHAR(50),
	EMAILNCC VARCHAR(50),
	MASOTHUE CHAR(10),
	GHICHU NVARCHAR(100),
	Constraint PK_NHACUNGCAP PRIMARY KEY(MANCC),
)
go
create table SANPHAM
(
	MASP VARCHAR(7) , 
	TENSP NVARCHAR(50) NOT NULL,
	LOAI NVARCHAR(30) NOT NULL,
	SOLUONG INT,
	MANCC VARCHAR(7),
	DONVITINH NVARCHAR(10),
	GIAVON DECIMAL,
	GIABAN DECIMAL,
	HINHANH VARCHAR(100),
	Constraint PK_SANPHAM PRIMARY KEY(MASP),
	Constraint FK_Nhacungcap Foreign Key(MANCC) References NHACUNGCAP(MANCC), 
)
GO
CREATE TABLE KHACHHANG
(
	MAKH VARCHAR(7) PRIMARY KEY,
	TK VARCHAR(50) UNIQUE,
	MK VARCHAR(50) NOT NULL,
	TENKH NVARCHAR(50),
	EMAILKH VARCHAR(100),
	DIACHIKH NVARCHAR(200),
	DTKH VARCHAR(50),
	GIOITINH NCHAR(4),
)
GO
CREATE TABLE DONDATHANG
(
	MAHD VARCHAR(7),
	DATHANHTOAN BIT,
	TINHTRANGGIAO BIT,
	NGAYDAT DATETIME,
	NGAYGIAO DATETIME,
	MAKH VARCHAR(7),
	THANHTIEN DECIMAL(18,0) CHECK (THANHTIEN>=0),
	PRIMARY KEY(MAHD),
	Constraint FK_KHACHHANG FOREIGN KEY(MAKH) REFERENCES KHACHHANG(MAKH),
)
GO
CREATE TABLE CTDONDATHANG
(
	MAHD VARCHAR(7),
	MASP VARCHAR(7),
	SL INT,
	TONGTIEN DECIMAL(18,0),
	Constraint FK_DONDATHANG FOREIGN KEY(MAHD) REFERENCES DONDATHANG(MAHD),
	Constraint FK_SANPHAM FOREIGN KEY(MASP) REFERENCES SANPHAM(MASP),	
)
GO
CREATE TABLE GIAMGIA
(
	MASP VARCHAR(7),
	GIAMGIA FLOAT,
	Constraint FK_SANPHAMGG FOREIGN KEY(MASP) REFERENCES SANPHAM(MASP),
)
GO 
CREATE TABLE PHIEUNHAP
(
	MAPN VARCHAR(7),
	MANCC VARCHAR(7),
	NGAYNHAP DATETIME,
	PRIMARY KEY(MAPN),
	Constraint FK_NHACUNGCAPPN FOREIGN KEY(MANCC) REFERENCES NHACUNGCAP(MANCC),

)
GO 
CREATE TABLE CTPHIEUNHAP
(
	MAPN VARCHAR(7),
	MASP VARCHAR(7),
	TENSP NVARCHAR(50),
	SOLUONGNHAP INT,
	GIAVON DECIMAL,
	Constraint FK_PHIEUNHAP FOREIGN KEY(MAPN) REFERENCES PHIEUNHAP(MAPN),

)
GO
CREATE TABLE ADMIN
(
	USERADMIN VARCHAR(30) PRIMARY KEY,
	PASSADMIN VARCHAR(30) NOT NULL,
	HOTEN NVARCHAR(50),
)


--Auto ID-----------
CREATE FUNCTION func_NextID(@lastID varchar(7),@prefix varchar(5),@size int)
	returns varchar(7)
as
	BEGIN
		if(@lastID = '')
			set @lastID = @prefix + REPLICATE (0, @size - LEN(@prefix))
		declare @num_nextID int, @nextID varchar(10)
		set @lastID = LTRIM(RTRIM(@lastID))
		set @num_nextID = REPLACE(@lastID, @prefix, '') + 1
		set @size = @size - LEN(@prefix)
		set @nextID = @prefix + REPLICATE(0, @size - LEN(@prefix))
		set @nextID = @prefix + RIGHT(REPLICATE(0, @size) + CONVERT (VARCHAR(MAX), @num_nextID), @size)
		return @nextID
	END
GO

CREATE TRIGGER tr_NextMAKH on [KHACHHANG]
for insert
as
	begin
		declare @lastID varchar(7)
		set @lastID = (SELECT TOP 1 MAKH from [KHACHHANG] order by MAKH desc)
		UPDATE [KHACHHANG] set MAKH = dbo.func_NextID (@lastID, 'KH', 7) where MAKH = ''
	end
GO


CREATE TRIGGER tr_NextMAHD on [DONDATHANG]
for insert
as
	begin
		declare @lastID varchar(7)
		set @lastID = (SELECT TOP 1 MAHD from [DONDATHANG] order by MAHD desc)
		UPDATE [DONDATHANG] set MAHD = dbo.func_NextID (@lastID, 'HD', 7) where MAHD = ''
	end
GO

CREATE TRIGGER tr_NextMASP on [SANPHAM]
for insert
as
	begin
		declare @lastID varchar(7)
		set @lastID = (SELECT TOP 1 MASP from [SANPHAM] order by MASP desc)
		UPDATE [SANPHAM] set MASP = dbo.func_NextID (@lastID, 'SP', 7) where MASP = ''
	end
GO

CREATE TRIGGER tr_NextMANCC on [NHACUNGCAP]
for insert
as
	begin
		declare @lastID varchar(7)
		set @lastID = (SELECT TOP 1 MANCC from [NHACUNGCAP] order by MANCC desc)
		UPDATE [NHACUNGCAP] set MANCC = dbo.func_NextID (@lastID, 'NCC', 7) where MANCC = ''
	end
GO

create view LOAI
AS
(
	SELECT LOAI
	FROM SANPHAM
	GROUP BY LOAI
)