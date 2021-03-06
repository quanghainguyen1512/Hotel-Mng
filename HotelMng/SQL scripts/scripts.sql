USE [master]
GO
/****** Object:  Database [HotelManagement]    Script Date: 09-Jun-17 9:05:06 PM ******/
CREATE DATABASE [HotelManagement]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'HotelManagement', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\HotelManagement.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'HotelManagement_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\HotelManagement_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [HotelManagement].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [HotelManagement] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [HotelManagement] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [HotelManagement] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [HotelManagement] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [HotelManagement] SET ARITHABORT OFF 
GO
ALTER DATABASE [HotelManagement] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [HotelManagement] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [HotelManagement] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [HotelManagement] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [HotelManagement] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [HotelManagement] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [HotelManagement] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [HotelManagement] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [HotelManagement] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [HotelManagement] SET  DISABLE_BROKER 
GO
ALTER DATABASE [HotelManagement] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [HotelManagement] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [HotelManagement] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [HotelManagement] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [HotelManagement] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [HotelManagement] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [HotelManagement] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [HotelManagement] SET RECOVERY FULL 
GO
ALTER DATABASE [HotelManagement] SET  MULTI_USER 
GO
ALTER DATABASE [HotelManagement] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [HotelManagement] SET DB_CHAINING OFF 
GO
ALTER DATABASE [HotelManagement] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [HotelManagement] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
USE [HotelManagement]
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [HotelManagement]
GO
/****** Object:  UserDefinedFunction [dbo].[Rental]    Script Date: 09-Jun-17 9:05:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Rental](@formid INT, @checkout DATETIME)
RETURNS MONEY
AS
BEGIN
	DECLARE @timespan INT,
			@pricebyday MONEY,
			@pricefirsthour MONEY,
			@priceperhour MONEY,
			@renterid VARCHAR(50),
			@result MONEY,
			@days INT,
			@hours INT,
			@capacity INT,
			@fee_foreigner FLOAT,
			@fee_moreguest FLOAT,
			@count_roommate INT,
			@count_foreinger INT

	SELECT @fee_foreigner = FeeForForeigner, @fee_moreguest = FeeForEachMoreGuest
	FROM dbo.FEE
	WHERE Id = 1

	SELECT	@timespan = DATEDIFF(HOUR, F.CheckIn, @checkout), 
			@pricebyday = RT.PriceByDay, 
			@pricefirsthour = RT.PriceFirstHour, 
			@priceperhour = RT.PricePerHour,
			@capacity = R.Capacity,
			@renterid = f.RenterId
	FROM dbo.REG_FORM AS F
	JOIN dbo.ROOM AS R
		ON R.RoomId = F.RoomId
	JOIN dbo.ROOM_TYPE AS RT
		ON RT.RoomTypeId = R.RoomTypeId
	WHERE F.FormId = @formid

	SELECT @count_foreinger = COUNT(RenterId)
	FROM dbo.RENTER
	WHERE RenterId = @renterid AND NatId <> 0

	SELECT @count_roommate = COUNT(*)
	FROM dbo.ROOMMATE
	WHERE FormId = @formid

	SELECT @count_foreinger = COUNT(*) + @count_foreinger
	FROM dbo.ROOMMATE
	WHERE FormId = @formid AND NatId <> 0

	SET @result = 0;
	SET @days = @timespan / 24;
	SET @hours = @timespan % 24;
	
	IF (@days > 0)
		BEGIN
			SET @pricefirsthour = 0;
			SET @result = @days * @pricebyday + @result;
		END

	IF (@hours = 0)																								--Ở tiếng đầu
		SET @result = @pricefirsthour + @result
	ELSE IF (@hours BETWEEN 1 AND 6)																			--Ở theo giờ
		SET @result = @pricefirsthour + @hours * @priceperhour + @result;
	ELSE IF (@hours BETWEEN 7 AND 12)																			--Tính theo giá nửa ngày
		SET @result = @pricebyday * 0.7 + @result;
	ELSE IF (@hours BETWEEN 13 AND 17)																			--nửa ngày và thêm giờ
		SET @result = @pricebyday * 0.7 + (@hours - 12) * @priceperhour + @result;
	ELSE IF (@hours BETWEEN 18 AND 23)
		SET @result = @result + @pricebyday;

	IF (@count_roommate + 1 > @capacity)
		SET @result = (@fee_moreguest * (@count_roommate + 1 - @capacity) + 1) * @result;
	IF (@count_foreinger > 0)
		SET @result = @fee_foreigner * @result

	RETURN @result;
END

GO
/****** Object:  Table [dbo].[ACCOUNT]    Script Date: 09-Jun-17 9:05:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ACCOUNT](
	[UserName] [varchar](20) NOT NULL,
	[Password] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BILL]    Script Date: 09-Jun-17 9:05:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BILL](
	[BillId] [varchar](20) NOT NULL,
	[TotalMoney] [money] NOT NULL,
	[PayerName] [nvarchar](30) NULL,
	[CompanyName] [nvarchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[BillId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FEE]    Script Date: 09-Jun-17 9:05:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FEE](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FeeForEachMoreGuest] [float] NULL,
	[FeeForForeigner] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[REG_FORM]    Script Date: 09-Jun-17 9:05:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[REG_FORM](
	[FormId] [int] IDENTITY(1,1) NOT NULL,
	[CheckIn] [smalldatetime] NOT NULL,
	[CheckOut] [smalldatetime] NULL,
	[RenterId] [varchar](50) NULL,
	[RoomId] [int] NULL,
	[BillId] [varchar](20) NULL,
	[Rental] [money] NULL,
 CONSTRAINT [PK_FormId] PRIMARY KEY CLUSTERED 
(
	[FormId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RENTER]    Script Date: 09-Jun-17 9:05:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RENTER](
	[RenterId] [varchar](50) NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
	[Gender] [bit] NOT NULL,
	[PhoneNum] [varchar](15) NULL,
	[NatId] [int] NULL,
	[IdentityNum] [varchar](20) NULL,
	[Address] [nvarchar](40) NULL,
 CONSTRAINT [PK_RenterId] PRIMARY KEY CLUSTERED 
(
	[RenterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ROOM]    Script Date: 09-Jun-17 9:05:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ROOM](
	[RoomId] [int] NOT NULL,
	[RoomTypeId] [char](1) NULL,
	[Description] [nvarchar](200) NULL,
	[StatusId] [int] NULL,
	[Capacity] [smallint] NULL,
PRIMARY KEY CLUSTERED 
(
	[RoomId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ROOM_STATUS]    Script Date: 09-Jun-17 9:05:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ROOM_STATUS](
	[StatusID] [int] IDENTITY(0,1) NOT NULL,
	[StatusName] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[StatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ROOM_TYPE]    Script Date: 09-Jun-17 9:05:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ROOM_TYPE](
	[RoomTypeId] [char](1) NOT NULL,
	[PriceByDay] [money] NOT NULL,
	[PriceFirstHour] [money] NOT NULL,
	[PricePerHour] [money] NOT NULL,
	[Note] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[RoomTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ROOMMATE]    Script Date: 09-Jun-17 9:05:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ROOMMATE](
	[Name] [nvarchar](30) NOT NULL,
	[IdentityNum] [varchar](10) NULL,
	[NatId] [int] NULL,
	[FormId] [int] NOT NULL,
 CONSTRAINT [PK_Roommate] PRIMARY KEY CLUSTERED 
(
	[Name] ASC,
	[FormId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SERVICE]    Script Date: 09-Jun-17 9:05:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SERVICE](
	[ServId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](15) NULL,
	[Price] [money] NOT NULL,
	[SvTypeId] [int] NULL,
	[Unit] [nvarchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[ServId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SERVICE_TYPE]    Script Date: 09-Jun-17 9:05:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SERVICE_TYPE](
	[SvTypeId] [int] IDENTITY(0,1) NOT NULL,
	[SvTypeName] [nvarchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[SvTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TABLE_NATIONALITY]    Script Date: 09-Jun-17 9:05:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TABLE_NATIONALITY](
	[NatId] [int] IDENTITY(0,1) NOT NULL,
	[NatName] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[NatId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[USE_SERVICES]    Script Date: 09-Jun-17 9:05:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USE_SERVICES](
	[ServId] [int] NOT NULL,
	[FormId] [int] NOT NULL,
	[Time] [datetime] NULL,
	[Quantity] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ServId] ASC,
	[FormId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
INSERT [dbo].[ACCOUNT] ([UserName], [Password]) VALUES (N'admin', N'admin')
INSERT [dbo].[BILL] ([BillId], [TotalMoney], [PayerName], [CompanyName]) VALUES (N'20176910112132', 273125.0000, N'Quang Hải', N'KMS')
SET IDENTITY_INSERT [dbo].[FEE] ON 

INSERT [dbo].[FEE] ([Id], [FeeForEachMoreGuest], [FeeForForeigner]) VALUES (1, 0.25, 1.5)
SET IDENTITY_INSERT [dbo].[FEE] OFF
SET IDENTITY_INSERT [dbo].[REG_FORM] ON 

INSERT [dbo].[REG_FORM] ([FormId], [CheckIn], [CheckOut], [RenterId], [RoomId], [BillId], [Rental]) VALUES (1, CAST(N'2017-06-04T00:00:00' AS SmallDateTime), CAST(N'2017-06-08T16:05:00' AS SmallDateTime), N'6B7E30C7-7B82-487F-9848-5987570E5E7A', 104, NULL, 928000.0000)
INSERT [dbo].[REG_FORM] ([FormId], [CheckIn], [CheckOut], [RenterId], [RoomId], [BillId], [Rental]) VALUES (2, CAST(N'2017-06-08T01:00:00' AS SmallDateTime), CAST(N'2017-06-08T16:18:00' AS SmallDateTime), N'C63BE82A-4093-4585-861C-C5BEBDF75DDA', 301, NULL, 230000.0000)
INSERT [dbo].[REG_FORM] ([FormId], [CheckIn], [CheckOut], [RenterId], [RoomId], [BillId], [Rental]) VALUES (3, CAST(N'2017-06-09T01:30:00' AS SmallDateTime), CAST(N'2017-06-09T10:12:00' AS SmallDateTime), N'927B304A-3E72-4866-980E-6BB12A7824F9', 105, N'20176910112132', 223125.0000)
INSERT [dbo].[REG_FORM] ([FormId], [CheckIn], [CheckOut], [RenterId], [RoomId], [BillId], [Rental]) VALUES (4, CAST(N'2017-06-09T01:32:00' AS SmallDateTime), CAST(N'2017-06-09T09:52:00' AS SmallDateTime), N'5A54D7CD-960C-436B-ACA3-90790711A853', 103, NULL, 105000.0000)
INSERT [dbo].[REG_FORM] ([FormId], [CheckIn], [CheckOut], [RenterId], [RoomId], [BillId], [Rental]) VALUES (5, CAST(N'2017-06-08T23:31:00' AS SmallDateTime), CAST(N'2017-06-09T20:32:00' AS SmallDateTime), N'90033DFE-EB4B-40FF-A63C-46F0701EE9C8', 104, NULL, 225000.0000)
SET IDENTITY_INSERT [dbo].[REG_FORM] OFF
INSERT [dbo].[RENTER] ([RenterId], [Name], [Gender], [PhoneNum], [NatId], [IdentityNum], [Address]) VALUES (N'5A54D7CD-960C-436B-ACA3-90790711A853', N'Another test', 1, N'2345676565', 0, N'234632625', N'')
INSERT [dbo].[RENTER] ([RenterId], [Name], [Gender], [PhoneNum], [NatId], [IdentityNum], [Address]) VALUES (N'6B7E30C7-7B82-487F-9848-5987570E5E7A', N'Nguyễn Quang Hải', 1, N'01273994299', 0, N'272580018', N'Đồng')
INSERT [dbo].[RENTER] ([RenterId], [Name], [Gender], [PhoneNum], [NatId], [IdentityNum], [Address]) VALUES (N'90033DFE-EB4B-40FF-A63C-46F0701EE9C8', N'Khách 123123', 1, N'12345678', 3, N'78987654', N'')
INSERT [dbo].[RENTER] ([RenterId], [Name], [Gender], [PhoneNum], [NatId], [IdentityNum], [Address]) VALUES (N'927B304A-3E72-4866-980E-6BB12A7824F9', N'kiểm tra 123', 0, N'5986734', 1, N'12341325', N'')
INSERT [dbo].[RENTER] ([RenterId], [Name], [Gender], [PhoneNum], [NatId], [IdentityNum], [Address]) VALUES (N'C63BE82A-4093-4585-861C-C5BEBDF75DDA', N'Đinh Cảnh Thịnh', 1, N'01229394822', 0, N'23876128', N'')
INSERT [dbo].[ROOM] ([RoomId], [RoomTypeId], [Description], [StatusId], [Capacity]) VALUES (101, N'A', NULL, 0, 2)
INSERT [dbo].[ROOM] ([RoomId], [RoomTypeId], [Description], [StatusId], [Capacity]) VALUES (102, N'A', NULL, 0, 2)
INSERT [dbo].[ROOM] ([RoomId], [RoomTypeId], [Description], [StatusId], [Capacity]) VALUES (103, N'A', N'', 2, 4)
INSERT [dbo].[ROOM] ([RoomId], [RoomTypeId], [Description], [StatusId], [Capacity]) VALUES (104, N'A', N'', 2, 4)
INSERT [dbo].[ROOM] ([RoomId], [RoomTypeId], [Description], [StatusId], [Capacity]) VALUES (105, N'B', N'', 2, 2)
INSERT [dbo].[ROOM] ([RoomId], [RoomTypeId], [Description], [StatusId], [Capacity]) VALUES (106, N'B', N'', 0, 4)
INSERT [dbo].[ROOM] ([RoomId], [RoomTypeId], [Description], [StatusId], [Capacity]) VALUES (201, N'A', N'', 0, 2)
INSERT [dbo].[ROOM] ([RoomId], [RoomTypeId], [Description], [StatusId], [Capacity]) VALUES (202, N'B', NULL, 0, 2)
INSERT [dbo].[ROOM] ([RoomId], [RoomTypeId], [Description], [StatusId], [Capacity]) VALUES (203, N'B', N'', 0, 4)
INSERT [dbo].[ROOM] ([RoomId], [RoomTypeId], [Description], [StatusId], [Capacity]) VALUES (204, N'B', NULL, 0, 4)
INSERT [dbo].[ROOM] ([RoomId], [RoomTypeId], [Description], [StatusId], [Capacity]) VALUES (205, N'C', NULL, 0, 2)
INSERT [dbo].[ROOM] ([RoomId], [RoomTypeId], [Description], [StatusId], [Capacity]) VALUES (301, N'C', N'', 0, 2)
INSERT [dbo].[ROOM] ([RoomId], [RoomTypeId], [Description], [StatusId], [Capacity]) VALUES (302, N'C', NULL, 0, 4)
INSERT [dbo].[ROOM] ([RoomId], [RoomTypeId], [Description], [StatusId], [Capacity]) VALUES (303, N'C', N'', 0, 4)
SET IDENTITY_INSERT [dbo].[ROOM_STATUS] ON 

INSERT [dbo].[ROOM_STATUS] ([StatusID], [StatusName]) VALUES (0, N'Trống')
INSERT [dbo].[ROOM_STATUS] ([StatusID], [StatusName]) VALUES (1, N'Có khách')
INSERT [dbo].[ROOM_STATUS] ([StatusID], [StatusName]) VALUES (2, N'Cần dọn')
INSERT [dbo].[ROOM_STATUS] ([StatusID], [StatusName]) VALUES (3, N'Cần sửa chữa')
SET IDENTITY_INSERT [dbo].[ROOM_STATUS] OFF
INSERT [dbo].[ROOM_TYPE] ([RoomTypeId], [PriceByDay], [PriceFirstHour], [PricePerHour], [Note]) VALUES (N'A', 150000.0000, 60000.0000, 20000.0000, N'')
INSERT [dbo].[ROOM_TYPE] ([RoomTypeId], [PriceByDay], [PriceFirstHour], [PricePerHour], [Note]) VALUES (N'B', 170000.0000, 80000.0000, 25000.0000, NULL)
INSERT [dbo].[ROOM_TYPE] ([RoomTypeId], [PriceByDay], [PriceFirstHour], [PricePerHour], [Note]) VALUES (N'C', 200000.0000, 100000.0000, 30000.0000, NULL)
INSERT [dbo].[ROOMMATE] ([Name], [IdentityNum], [NatId], [FormId]) VALUES (N'Chí Phèo', N'98732984', 0, 3)
INSERT [dbo].[ROOMMATE] ([Name], [IdentityNum], [NatId], [FormId]) VALUES (N'Jack', N'1235151', 0, 1)
INSERT [dbo].[ROOMMATE] ([Name], [IdentityNum], [NatId], [FormId]) VALUES (N'Lewandowski', N'436546341', 0, 1)
INSERT [dbo].[ROOMMATE] ([Name], [IdentityNum], [NatId], [FormId]) VALUES (N'Thị Nở', N'12387412', 0, 3)
INSERT [dbo].[ROOMMATE] ([Name], [IdentityNum], [NatId], [FormId]) VALUES (N'Trần Nam', N'272590293', 0, 1)
SET IDENTITY_INSERT [dbo].[SERVICE] ON 

INSERT [dbo].[SERVICE] ([ServId], [Name], [Price], [SvTypeId], [Unit]) VALUES (1, N'Coca', 10000.0000, 3, N'Lon')
INSERT [dbo].[SERVICE] ([ServId], [Name], [Price], [SvTypeId], [Unit]) VALUES (2, N'Heineken', 15000.0000, 3, N'Lon')
INSERT [dbo].[SERVICE] ([ServId], [Name], [Price], [SvTypeId], [Unit]) VALUES (3, N'Tiger', 12000.0000, 3, N'Lon')
INSERT [dbo].[SERVICE] ([ServId], [Name], [Price], [SvTypeId], [Unit]) VALUES (4, N'Thuốc lá', 19000.0000, 0, N'Gói')
INSERT [dbo].[SERVICE] ([ServId], [Name], [Price], [SvTypeId], [Unit]) VALUES (5, N'Mì tôm', 6000.0000, 2, N'Gói')
INSERT [dbo].[SERVICE] ([ServId], [Name], [Price], [SvTypeId], [Unit]) VALUES (6, N'Cơm sườn', 15000.0000, 2, N'Dĩa')
INSERT [dbo].[SERVICE] ([ServId], [Name], [Price], [SvTypeId], [Unit]) VALUES (7, N'Giặt', 5000.0000, 1, N'Kg')
INSERT [dbo].[SERVICE] ([ServId], [Name], [Price], [SvTypeId], [Unit]) VALUES (9, N'Cà phê', 10000.0000, 3, N'Ly')
INSERT [dbo].[SERVICE] ([ServId], [Name], [Price], [SvTypeId], [Unit]) VALUES (10, N'Ủi', 5000.0000, 1, N'Bộ')
INSERT [dbo].[SERVICE] ([ServId], [Name], [Price], [SvTypeId], [Unit]) VALUES (13, N'Mì xào bò', 20000.0000, 2, N'Dĩa')
INSERT [dbo].[SERVICE] ([ServId], [Name], [Price], [SvTypeId], [Unit]) VALUES (1011, N'Red Bull', 11000.0000, 3, N'Lon')
INSERT [dbo].[SERVICE] ([ServId], [Name], [Price], [SvTypeId], [Unit]) VALUES (1012, N'C2', 8000.0000, 3, N'Chai')
SET IDENTITY_INSERT [dbo].[SERVICE] OFF
SET IDENTITY_INSERT [dbo].[SERVICE_TYPE] ON 

INSERT [dbo].[SERVICE_TYPE] ([SvTypeId], [SvTypeName]) VALUES (0, N'Khác')
INSERT [dbo].[SERVICE_TYPE] ([SvTypeId], [SvTypeName]) VALUES (1, N'Dịch vụ')
INSERT [dbo].[SERVICE_TYPE] ([SvTypeId], [SvTypeName]) VALUES (2, N'Đồ ăn')
INSERT [dbo].[SERVICE_TYPE] ([SvTypeId], [SvTypeName]) VALUES (3, N'Nước uống')
INSERT [dbo].[SERVICE_TYPE] ([SvTypeId], [SvTypeName]) VALUES (4, N'Giải trí')
INSERT [dbo].[SERVICE_TYPE] ([SvTypeId], [SvTypeName]) VALUES (5, N'Đặc biệt')
SET IDENTITY_INSERT [dbo].[SERVICE_TYPE] OFF
SET IDENTITY_INSERT [dbo].[TABLE_NATIONALITY] ON 

INSERT [dbo].[TABLE_NATIONALITY] ([NatId], [NatName]) VALUES (4, N'Anh')
INSERT [dbo].[TABLE_NATIONALITY] ([NatId], [NatName]) VALUES (7, N'Ba Lan')
INSERT [dbo].[TABLE_NATIONALITY] ([NatId], [NatName]) VALUES (5, N'Hàn Quốc')
INSERT [dbo].[TABLE_NATIONALITY] ([NatId], [NatName]) VALUES (6, N'Khác')
INSERT [dbo].[TABLE_NATIONALITY] ([NatId], [NatName]) VALUES (1, N'Mỹ')
INSERT [dbo].[TABLE_NATIONALITY] ([NatId], [NatName]) VALUES (3, N'Nhật')
INSERT [dbo].[TABLE_NATIONALITY] ([NatId], [NatName]) VALUES (2, N'Pháp')
INSERT [dbo].[TABLE_NATIONALITY] ([NatId], [NatName]) VALUES (8, N'Trung Quốc')
INSERT [dbo].[TABLE_NATIONALITY] ([NatId], [NatName]) VALUES (0, N'Việt Nam')
SET IDENTITY_INSERT [dbo].[TABLE_NATIONALITY] OFF
INSERT [dbo].[USE_SERVICES] ([ServId], [FormId], [Time], [Quantity]) VALUES (1, 1, CAST(N'2017-06-04T16:31:59.910' AS DateTime), 2)
INSERT [dbo].[USE_SERVICES] ([ServId], [FormId], [Time], [Quantity]) VALUES (1, 3, CAST(N'2017-06-09T09:16:15.737' AS DateTime), 3)
INSERT [dbo].[USE_SERVICES] ([ServId], [FormId], [Time], [Quantity]) VALUES (2, 1, CAST(N'2017-06-05T01:04:30.177' AS DateTime), 3)
INSERT [dbo].[USE_SERVICES] ([ServId], [FormId], [Time], [Quantity]) VALUES (5, 1, CAST(N'2017-06-04T16:32:35.650' AS DateTime), 5)
INSERT [dbo].[USE_SERVICES] ([ServId], [FormId], [Time], [Quantity]) VALUES (9, 3, CAST(N'2017-06-09T09:16:25.133' AS DateTime), 2)
INSERT [dbo].[USE_SERVICES] ([ServId], [FormId], [Time], [Quantity]) VALUES (13, 1, CAST(N'2017-06-06T23:10:21.327' AS DateTime), 2)
INSERT [dbo].[USE_SERVICES] ([ServId], [FormId], [Time], [Quantity]) VALUES (1012, 1, CAST(N'2017-06-05T01:25:22.243' AS DateTime), 1)
SET ANSI_PADDING ON

GO
/****** Object:  Index [UC_IdentityNum]    Script Date: 09-Jun-17 9:05:07 PM ******/
ALTER TABLE [dbo].[RENTER] ADD  CONSTRAINT [UC_IdentityNum] UNIQUE NONCLUSTERED 
(
	[IdentityNum] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [DF_NatName]    Script Date: 09-Jun-17 9:05:07 PM ******/
ALTER TABLE [dbo].[TABLE_NATIONALITY] ADD  CONSTRAINT [DF_NatName] UNIQUE NONCLUSTERED 
(
	[NatName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[RENTER] ADD  CONSTRAINT [DF_RenterId]  DEFAULT (newid()) FOR [RenterId]
GO
ALTER TABLE [dbo].[SERVICE] ADD  DEFAULT ((0)) FOR [Price]
GO
ALTER TABLE [dbo].[USE_SERVICES] ADD  CONSTRAINT [DF_Time]  DEFAULT (getdate()) FOR [Time]
GO
ALTER TABLE [dbo].[REG_FORM]  WITH CHECK ADD FOREIGN KEY([BillId])
REFERENCES [dbo].[BILL] ([BillId])
GO
ALTER TABLE [dbo].[REG_FORM]  WITH CHECK ADD FOREIGN KEY([RoomId])
REFERENCES [dbo].[ROOM] ([RoomId])
GO
ALTER TABLE [dbo].[REG_FORM]  WITH CHECK ADD  CONSTRAINT [FK_RegForm_Renter] FOREIGN KEY([RenterId])
REFERENCES [dbo].[RENTER] ([RenterId])
GO
ALTER TABLE [dbo].[REG_FORM] CHECK CONSTRAINT [FK_RegForm_Renter]
GO
ALTER TABLE [dbo].[RENTER]  WITH CHECK ADD  CONSTRAINT [FK_Renter_Nat] FOREIGN KEY([NatId])
REFERENCES [dbo].[TABLE_NATIONALITY] ([NatId])
GO
ALTER TABLE [dbo].[RENTER] CHECK CONSTRAINT [FK_Renter_Nat]
GO
ALTER TABLE [dbo].[ROOM]  WITH CHECK ADD FOREIGN KEY([RoomTypeId])
REFERENCES [dbo].[ROOM_TYPE] ([RoomTypeId])
GO
ALTER TABLE [dbo].[ROOM]  WITH CHECK ADD  CONSTRAINT [FK_Room_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[ROOM_STATUS] ([StatusID])
GO
ALTER TABLE [dbo].[ROOM] CHECK CONSTRAINT [FK_Room_Status]
GO
ALTER TABLE [dbo].[ROOMMATE]  WITH CHECK ADD  CONSTRAINT [FK_Rommate_Nat] FOREIGN KEY([NatId])
REFERENCES [dbo].[TABLE_NATIONALITY] ([NatId])
GO
ALTER TABLE [dbo].[ROOMMATE] CHECK CONSTRAINT [FK_Rommate_Nat]
GO
ALTER TABLE [dbo].[ROOMMATE]  WITH CHECK ADD  CONSTRAINT [FK_Roommate_Form] FOREIGN KEY([FormId])
REFERENCES [dbo].[REG_FORM] ([FormId])
GO
ALTER TABLE [dbo].[ROOMMATE] CHECK CONSTRAINT [FK_Roommate_Form]
GO
ALTER TABLE [dbo].[SERVICE]  WITH CHECK ADD  CONSTRAINT [FK_Service_ServiceType] FOREIGN KEY([SvTypeId])
REFERENCES [dbo].[SERVICE_TYPE] ([SvTypeId])
GO
ALTER TABLE [dbo].[SERVICE] CHECK CONSTRAINT [FK_Service_ServiceType]
GO
ALTER TABLE [dbo].[USE_SERVICES]  WITH CHECK ADD FOREIGN KEY([ServId])
REFERENCES [dbo].[SERVICE] ([ServId])
GO
ALTER TABLE [dbo].[USE_SERVICES]  WITH CHECK ADD  CONSTRAINT [FK_UseService_Form] FOREIGN KEY([FormId])
REFERENCES [dbo].[REG_FORM] ([FormId])
GO
ALTER TABLE [dbo].[USE_SERVICES] CHECK CONSTRAINT [FK_UseService_Form]
GO
/****** Object:  StoredProcedure [dbo].[USP_GetAllRoomInfo]    Script Date: 09-Jun-17 9:05:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_GetAllRoomInfo]
AS
	SELECT R.RoomId, R.RoomTypeId, R.StatusId, R.Capacity ,RS.StatusName, R.Description
	FROM dbo.ROOM AS R
	JOIN dbo.ROOM_STATUS AS RS
		ON RS.StatusID = R.StatusId

GO
/****** Object:  StoredProcedure [dbo].[USP_GetAllRoommatesByFormId]    Script Date: 09-Jun-17 9:05:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_GetAllRoommatesByFormId]
	@formid INT
AS
	SELECT RM.Name, RM.IdentityNum, NAT.NatId, NAT.NatName
	FROM dbo.ROOMMATE AS RM
	JOIN dbo.TABLE_NATIONALITY AS NAT
		ON NAT.NatId = RM.NatId
	WHERE RM.FormId = @formid

GO
/****** Object:  StoredProcedure [dbo].[USP_GetAllServicesInfo]    Script Date: 09-Jun-17 9:05:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_GetAllServicesInfo]
AS
	SELECT SV.ServId, SV.Name, SV.Price, SV.SvTypeId, SV.Unit, ST.SvTypeName
	FROM dbo.SERVICE AS SV 
	JOIN dbo.SERVICE_TYPE AS ST
		ON ST.SvTypeId = SV.SvTypeId

GO
/****** Object:  StoredProcedure [dbo].[USP_GetDataForReporting]    Script Date: 09-Jun-17 9:05:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_GetDataForReporting]
	@month SMALLINT,
	@year  INT
AS
	DECLARE @temptable TABLE
	(RoomTypeId CHAR(1),
	Income		MONEY)
	DECLARE @total MONEY

	INSERT INTO @temptable ( RoomTypeId, Income )
		SELECT	RT.RoomTypeId, SUM(RF.Rental) AS Income
		FROM	dbo.ROOM_TYPE AS RT
		JOIN	dbo.ROOM AS R
			ON	R.RoomTypeId = RT.RoomTypeId
		JOIN	dbo.REG_FORM AS RF
			ON	RF.RoomId = R.RoomId
		WHERE	MONTH(RF.CheckOut) = @month AND YEAR(RF.CheckOut) = @year
		GROUP BY RT.RoomTypeId
	
	SELECT	@total = SUM(t.Income)
	FROM	@temptable AS t
	SELECT	t.RoomTypeId, t.Income, (t.Income * 100 / @total) AS Proportion
	FROM	@temptable AS t

GO
/****** Object:  StoredProcedure [dbo].[USP_GetNewestNationalityId]    Script Date: 09-Jun-17 9:05:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_GetNewestNationalityId]
AS
	SELECT TOP 1 NAT.NatId
	FROM dbo.TABLE_NATIONALITY AS NAT
	ORDER BY NAT.NatId DESC

GO
/****** Object:  StoredProcedure [dbo].[USP_GetNewestServiceInfo]    Script Date: 09-Jun-17 9:05:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_GetNewestServiceInfo]
AS
	SELECT TOP 1 S.ServId
	FROM dbo.SERVICE AS S 
	JOIN dbo.SERVICE_TYPE AS ST
		ON ST.SvTypeId = S.SvTypeId
	ORDER BY S.ServId DESC

GO
/****** Object:  StoredProcedure [dbo].[USP_GetRoomStatusInfo]    Script Date: 09-Jun-17 9:05:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_GetRoomStatusInfo]
AS
	SELECT RS.StatusId, RS.StatusName, COUNT(R.RoomId) AS ITEMCOUNT
	FROM dbo.ROOM AS R
	RIGHT JOIN dbo.ROOM_STATUS AS RS
		ON RS.StatusID = R.StatusId
	GROUP BY RS.StatusId, RS.StatusName
GO
/****** Object:  StoredProcedure [dbo].[USP_GetSelectedFormId]    Script Date: 09-Jun-17 9:05:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_GetSelectedFormId]
	@roomid INT
AS
	SELECT FormId
	FROM dbo.REG_FORM
	WHERE RoomId = @roomid AND CheckOut IS NULL

GO
/****** Object:  StoredProcedure [dbo].[USP_GetServicesBeingUsed]    Script Date: 09-Jun-17 9:05:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_GetServicesBeingUsed]
	@formid INT
AS
	SELECT US.FormId, US.Time, US.Quantity, US.ServId, S.Name
	FROM dbo.USE_SERVICES AS US
	JOIN dbo.SERVICE AS S
		ON S.ServId = US.ServId
	WHERE US.FormId = @formid

GO
/****** Object:  StoredProcedure [dbo].[USP_GetUsingRoom]    Script Date: 09-Jun-17 9:05:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_GetUsingRoom]
AS
	SELECT RF.RoomId, RF.FormId, RF.CheckIn
	FROM dbo.REG_FORM AS RF
	WHERE RF.CheckOut IS NULL

GO
/****** Object:  StoredProcedure [dbo].[USP_LogIn]    Script Date: 09-Jun-17 9:05:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_LogIn]
	@username VARCHAR(20),
	@password VARCHAR(20)
AS
	SELECT *
	FROM dbo.ACCOUNT
	WHERE Password = @password AND UserName = @username
GO
/****** Object:  StoredProcedure [dbo].[USP_ServiceCharge]    Script Date: 09-Jun-17 9:05:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_ServiceCharge]
	@formid INT
AS
	SELECT SUM(temp.Money)
	FROM dbo.REG_FORM AS F
	JOIN (SELECT US.FormId, US.Quantity * S.Price AS Money
		FROM dbo.USE_SERVICES AS US
		JOIN dbo.SERVICE AS S
			ON S.ServId = US.ServId) AS temp
		ON temp.FormId = F.FormId
	WHERE F.FormId = @formid
GO
USE [master]
GO
ALTER DATABASE [HotelManagement] SET  READ_WRITE 
GO
