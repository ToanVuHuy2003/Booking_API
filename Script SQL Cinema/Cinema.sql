create database cinemaAPI
go
use cinemaAPI
go

create table KhachHang(
	MaKH int primary key,
	HoTen nvarchar(max),
	Email nvarchar(max),
	MatKhau nvarchar(max),
	STD char(10),
	AnhDaiDien nvarchar(max)
);

create table Phim(
	MaPhim int primary key,
	TenPhim nvarchar(max),
	AnhPhim nvarchar(max),
	DaoDien nvarchar(max),
	MaTL int,
	NgonNgu nvarchar(max),
	MoTa nvarchar(max)
);

create table TheLoai(
	MaTL int primary key,
	TenTL nvarchar(max)
);

create table SuatChieu(
	MaSC int primary key,
	MaPhim int,
	ThoiGianBD nvarchar(max),
	ThoiGianKT nvarchar(max),
	NgayChieu nvarchar(max),
	RapChieu nvarchar(max)
);

create table DatVe(
	MaDat int primary key,
	MaPhim int,
	MaKH int,
	Ghe nvarchar(max),
	GiaTien float,
	ThoiGianDat datetime
);

create table HoaDon(
	MaHD int primary key,
	MaKH int,
	MaDat int,
	SoLuong int,
	ThoiGianTT datetime
);

alter table Phim add constraint fk_Phim_TheLoai foreign key (MaTL) references TheLoai(MaTL);
alter table SuatChieu add constraint fk_SC_Phim foreign key (MaPhim) references Phim(MaPhim);
alter table DatVe add constraint fk_DatVe_Phim foreign key (MaPhim) references Phim(MaPhim);
alter table DatVe add constraint fk_DatVe_KH foreign key (MaKH) references KhachHang(MaKH);
alter table HoaDon add constraint fk_HoaDon_DatVe foreign key (MaDat) references DatVe(MaDat);
alter table HoaDon add constraint fk_HoaDon_KH foreign key (MaKH) references KhachHang(MaKH);