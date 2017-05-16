CREATE DATABASE HotelManagement
GO
USE HotelManagement
GO


CREATE TABLE ACCOUNT
(
	UserName	varchar(20) not null PRIMARY KEY,
	Password	varchar(20) not null,
)

GO
CREATE TABLE ROOM_TYPE												--Loại phòng
(
	RoomTypeId	char(1) not null PRIMARY KEY,		--QD1--
	Price		money not null,
	Note		varchar(100)
)												
GO
CREATE TABLE ROOM_STATUS
(
	StatusID	int identity not null PRIMARY KEY,
	StatusName	nvarchar(20)
)													
GO
CREATE TABLE ROOM													--Phòng
(
	RoomId		int not null PRIMARY KEY,
	RoomTypeId	char(1),
	Description	nvarchar(200),
	StatusId	int 
	FOREIGN KEY (RoomTypeId) REFERENCES ROOM_TYPE(RoomTypeId),
	FOREIGN KEY (StatusId) REFERENCES ROOM_STATUS(StatusId)
)
GO
CREATE TABLE NATIONALITY
(
	NatId		int identity not null PRIMARY KEY,
	Name		nvarchar(20)
)
CREATE TABLE RENTER														--Người Thuê
(
	RenterId	varchar(20) not null PRIMARY KEY,
	Name		nvarchar(30) not null,
	Gender		bit not null,
	PhoneNum	varchar(15),
	NatId		int,													--QD2--
	IdentityNum	varchar(20),
	Address		varchar(40)

	FOREIGN KEY (NatId) REFERENCES NATIONALITY(NatId)
)
GO
CREATE TABLE SERVICE_TYPE
(
	SvTypeId	int identity PRIMARY KEY,
	SvTypeName	nvarchar(30),
)
GO
CREATE TABLE SERVICE 
(
	ServId	int identity PRIMARY KEY,
	Name	varchar(15),
	Price	money not null default 0,
	SvTypeId int,
	FOREIGN KEY (SvTypeId) REFERENCES SERVICE_TYPE(SvTypeId)
)
GO
CREATE TABLE BILL														--hóa đơn
(
	BillId		varchar(20) not null PRIMARY KEY,
	RenterId	varchar(20),
	RoomId		int,
	TotalMoney	money not null,
	
	FOREIGN KEY (RoomId) REFERENCES ROOM(RoomId),
	FOREIGN KEY (RenterId) REFERENCES RENTER(RenterId),)
GO
CREATE TABLE TABLE_NATIONALITY
(
	NatId	int identity PRIMARY KEY,
	NatName nvarchar(20)
)
GO
CREATE TABLE REG_FORM														--phiếu thuê phòng
(
	FormId		int identity PRIMARY KEY,
	CheckIn		smalldatetime not null,
	CheckOut	smalldatetime,
	RenterId	varchar(20),
	RoomId		int,
	BillId		varchar(20),
	NatId		int,

	FOREIGN KEY (RenterId) REFERENCES RENTER(RenterId),
	FOREIGN KEY (RoomId) REFERENCES ROOM(RoomId),
	FOREIGN KEY (BillId) REFERENCES BILL(BillId),
	FOREIGN KEY (NatId) REFERENCES TABLE_NATIONALITY(NatId)
)
GO
CREATE TABLE USE_SERVICES
(
	ServId	int,
	FormId	int,
	Time	DateTime,

	PRIMARY KEY (ServId, FormId),
	FOREIGN KEY (ServId) REFERENCES SERVICE(ServId),
	FOREIGN KEY (FormId) REFERENCES REG_FORM(FormId)
)
GO
CREATE TABLE APPLIANCE
(
	ApplianceId	int identity PRIMARY KEY,
	Name		nvarchar(30)
)
GO
CREATE TABLE HAVE_APPLIANCE
(
	ApplianceId int,
	RoomId		int,
	PRIMARY KEY(ApplianceId, RoomId),
	FOREIGN KEY(ApplianceId) REFERENCES APPLIANCE(ApplianceId),
	FOREIGN KEY(RoomId) REFERENCES ROOM(RoomId)
)
GO
CREATE TABLE ROOMMATE
(
	Name	varchar(30),
	NatId	int,
	FormId	int,
	RenterId varchar(20),

	PRIMARY KEY (FormId,RenterId),
	FOREIGN KEY (FormId) REFERENCES REG_FORM(FormId),
	FOREIGN KEY (RenterId) REFERENCES RENTER(RenterId),
	FOREIGN KEY (NatId) REFERENCES TABLE_NATIONALITY(NatId)
)
GO
