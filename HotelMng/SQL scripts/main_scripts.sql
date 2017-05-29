USE [master]
GO
/****** Object:  Database [HotelManagement]    Script Date: 29-May-17 1:30:08 PM ******/
CREATE DATABASE [HotelManagement]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'HotelManagement', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\HotelManagement.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'HotelManagement_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\HotelManagement_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [HotelManagement] SET COMPATIBILITY_LEVEL = 130
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
ALTER DATABASE [HotelManagement] SET  ENABLE_BROKER 
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
ALTER DATABASE [HotelManagement] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [HotelManagement] SET QUERY_STORE = OFF
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
/****** Object:  Table [dbo].[ACCOUNT]    Script Date: 29-May-17 1:30:08 PM ******/
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
/****** Object:  Table [dbo].[BILL]    Script Date: 29-May-17 1:30:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BILL](
	[BillId] [varchar](20) NOT NULL,
	[RenterId] [varchar](20) NULL,
	[RoomId] [int] NULL,
	[TotalMoney] [money] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[BillId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[REG_FORM]    Script Date: 29-May-17 1:30:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[REG_FORM](
	[FormId] [int] IDENTITY(1,1) NOT NULL,
	[CheckIn] [smalldatetime] NOT NULL,
	[CheckOut] [smalldatetime] NULL,
	[RenterId] [varchar](20) NULL,
	[RoomId] [int] NULL,
	[BillId] [varchar](20) NULL,
	[Rental] [money] NULL,
PRIMARY KEY CLUSTERED 
(
	[FormId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RENTER]    Script Date: 29-May-17 1:30:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RENTER](
	[RenterId] [varchar](20) NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
	[Gender] [bit] NOT NULL,
	[PhoneNum] [varchar](15) NULL,
	[NatId] [int] NULL,
	[IDENTITYNum] [varchar](20) NULL,
	[Address] [varchar](40) NULL,
PRIMARY KEY CLUSTERED 
(
	[RenterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ROOM]    Script Date: 29-May-17 1:30:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ROOM](
	[RoomId] [int] NOT NULL,
	[RoomTypeId] [char](1) NULL,
	[Description] [nvarchar](200) NULL,
	[StatusId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[RoomId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ROOM_STATUS]    Script Date: 29-May-17 1:30:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ROOM_STATUS](
	[StatusID] [int] IDENTITY(1,1) NOT NULL,
	[StatusName] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[StatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ROOM_TYPE]    Script Date: 29-May-17 1:30:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ROOM_TYPE](
	[RoomTypeId] [char](1) NOT NULL,
	[PriceByDay] [money] NOT NULL,
	[Price1stHour] [money] NOT NULL,
	[PricePerHour] [money] NOT NULL,
	[Note] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[RoomTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ROOMMATE]    Script Date: 29-May-17 1:30:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ROOMMATE](
	[Name] [nvarchar](30) NULL,
	[IdentityNum] [varchar](10) NULL,
	[NatId] [int] NULL,
	[FormId] [int] NOT NULL,
	[RenterId] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[FormId] ASC,
	[RenterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SERVICE]    Script Date: 29-May-17 1:30:08 PM ******/
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
/****** Object:  Table [dbo].[SERVICE_TYPE]    Script Date: 29-May-17 1:30:08 PM ******/
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
/****** Object:  Table [dbo].[TABLE_NATIONALITY]    Script Date: 29-May-17 1:30:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TABLE_NATIONALITY](
	[NatId] [int] IDENTITY(1,1) NOT NULL,
	[NatName] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[NatId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[USE_SERVICES]    Script Date: 29-May-17 1:30:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USE_SERVICES](
	[ServId] [int] NOT NULL,
	[FormId] [int] NOT NULL,
	[Time] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ServId] ASC,
	[FormId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[SERVICE] ADD  DEFAULT ((0)) FOR [Price]
GO
ALTER TABLE [dbo].[BILL]  WITH CHECK ADD FOREIGN KEY([RenterId])
REFERENCES [dbo].[RENTER] ([RenterId])
GO
ALTER TABLE [dbo].[BILL]  WITH CHECK ADD FOREIGN KEY([RoomId])
REFERENCES [dbo].[ROOM] ([RoomId])
GO
ALTER TABLE [dbo].[REG_FORM]  WITH CHECK ADD FOREIGN KEY([BillId])
REFERENCES [dbo].[BILL] ([BillId])
GO
ALTER TABLE [dbo].[REG_FORM]  WITH CHECK ADD FOREIGN KEY([RenterId])
REFERENCES [dbo].[RENTER] ([RenterId])
GO
ALTER TABLE [dbo].[REG_FORM]  WITH CHECK ADD FOREIGN KEY([RoomId])
REFERENCES [dbo].[ROOM] ([RoomId])
GO
ALTER TABLE [dbo].[RENTER]  WITH CHECK ADD FOREIGN KEY([NatId])
REFERENCES [dbo].[TABLE_NATIONALITY] ([NatId])
GO
ALTER TABLE [dbo].[ROOM]  WITH CHECK ADD FOREIGN KEY([RoomTypeId])
REFERENCES [dbo].[ROOM_TYPE] ([RoomTypeId])
GO
ALTER TABLE [dbo].[ROOM]  WITH CHECK ADD FOREIGN KEY([StatusId])
REFERENCES [dbo].[ROOM_STATUS] ([StatusID])
GO
ALTER TABLE [dbo].[ROOMMATE]  WITH CHECK ADD FOREIGN KEY([FormId])
REFERENCES [dbo].[REG_FORM] ([FormId])
GO
ALTER TABLE [dbo].[ROOMMATE]  WITH CHECK ADD FOREIGN KEY([NatId])
REFERENCES [dbo].[TABLE_NATIONALITY] ([NatId])
GO
ALTER TABLE [dbo].[ROOMMATE]  WITH CHECK ADD FOREIGN KEY([RenterId])
REFERENCES [dbo].[RENTER] ([RenterId])
GO
ALTER TABLE [dbo].[SERVICE]  WITH CHECK ADD FOREIGN KEY([SvTypeId])
REFERENCES [dbo].[SERVICE_TYPE] ([SvTypeId])
GO
ALTER TABLE [dbo].[USE_SERVICES]  WITH CHECK ADD FOREIGN KEY([FormId])
REFERENCES [dbo].[REG_FORM] ([FormId])
GO
ALTER TABLE [dbo].[USE_SERVICES]  WITH CHECK ADD FOREIGN KEY([ServId])
REFERENCES [dbo].[SERVICE] ([ServId])
GO
/****** Object:  StoredProcedure [dbo].[USP_GetAllRoomInfo]    Script Date: 29-May-17 1:30:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_GetAllRoomInfo]
AS
	SELECT R.RoomId, R.RoomTypeId, R.StatusId, RS.StatusName, R.Description
	FROM dbo.ROOM AS R
	JOIN dbo.ROOM_STATUS AS RS
		ON RS.StatusID = R.StatusId

GO
/****** Object:  StoredProcedure [dbo].[USP_GetAllRoommatesByRenterId]    Script Date: 29-May-17 1:30:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Procedure--
CREATE PROC [dbo].[USP_GetAllRoommatesByRenterId]
	@renterid VARCHAR(20)
AS
	SELECT RM.Name, RM.IdentityNum, NAT.NatName
	FROM dbo.ROOMMATE AS RM
	JOIN dbo.TABLE_NATIONALITY AS NAT
		ON NAT.NatId = RM.NatId
	WHERE RM.RenterId = @renterid

GO
/****** Object:  StoredProcedure [dbo].[USP_GetAllServicesInfo]    Script Date: 29-May-17 1:30:08 PM ******/
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
/****** Object:  StoredProcedure [dbo].[USP_GetDataForReporting]    Script Date: 29-May-17 1:30:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_GetDataForReporting]
	@month SMALLINT
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
		WHERE	MONTH(RF.CheckOut) = @month
		GROUP BY RT.RoomTypeId
	
	SELECT	@total = SUM(t.Income)
	FROM	@temptable AS t
	SELECT	t.RoomTypeId, t.Income, t.Income / @total AS Proportion
	FROM	@temptable AS t

GO
/****** Object:  StoredProcedure [dbo].[USP_GetNewestServiceInfo]    Script Date: 29-May-17 1:30:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_GetNewestServiceInfo]
AS
	SELECT TOP 1 S.ServId, ST.SvTypeName
	FROM dbo.SERVICE AS S 
	JOIN dbo.SERVICE_TYPE AS ST
		ON ST.SvTypeId = S.SvTypeId
	ORDER BY S.ServId DESC
GO
/****** Object:  StoredProcedure [dbo].[USP_GetRoomStatusInfo]    Script Date: 29-May-17 1:30:08 PM ******/
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
USE [master]
GO
ALTER DATABASE [HotelManagement] SET  READ_WRITE 
GO
