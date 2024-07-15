create database cinemaAPI
go
use cinemaAPI
go

create table KhachHang(
	MaKH char(10) primary key,
	HoTen nvarchar(max),
	Email nvarchar(max),
	MatKhau nvarchar(max),
	STD char(10),
	AnhDaiDien nvarchar(max)
);

create table Phim(
	MaPhim char(10) primary key,
	TenPhim nvarchar(max),
	AnhPhim nvarchar(max),
	DaoDien nvarchar(max),
	MaTL char(10),
	NgonNgu nvarchar(max),
	MoTa nvarchar(max)
);

create table TheLoai(
	MaTL char(10) primary key,
	TenTL nvarchar(max)
);

create table SuatChieu(
	MaSC char(10) primary key,
	MaPhim char(10),
	ThoiGianBD nvarchar(max),
	ThoiGianKT nvarchar(max),
	NgayChieu nvarchar(max),
	RapChieu nvarchar(max)
);

create table DatVe(
	MaDat char(10) primary key,
	MaSC char(10),
	MaKH char(10),
	Ghe nvarchar(max),
	GiaTien float,
	ThoiGianDat datetime
);

create table HoaDon(
	MaHD char(10) primary key,
	MaKH char(10),
	MaSC char(10),
	MaDat char(10),
	SoLuong int,
	ThoiGianTT datetime
);

alter table Phim add constraint fk_Phim_TheLoai foreign key (MaTL) references TheLoai(MaTL);
alter table SuatChieu add constraint fk_SC_Phim foreign key (MaPhim) references Phim(MaPhim);
alter table DatVe add constraint fk_DatVe_SC foreign key (MaSC) references SuatChieu(MaSC);
alter table DatVe add constraint fk_DatVe_KH foreign key (MaKH) references KhachHang(MaKH);
alter table HoaDon add constraint fk_HoaDon_DatVe foreign key (MaDat) references DatVe(MaDat);
alter table HoaDon add constraint fk_HoaDon_KH foreign key (MaKH) references KhachHang(MaKH);
alter table HoaDon add constraint fk_HoaDon_SC foreign key (MaSC) references SuatChieu(MaSC);