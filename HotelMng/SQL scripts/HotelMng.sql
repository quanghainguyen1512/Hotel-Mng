CREATE DATABASE HotelManagement
GO
USE HotelManagement
GO

CREATE TABLE ACCOUNT
(
	UserName	VARCHAR(20) NOT NULL PRIMARY KEY,
	Password	VARCHAR(20) NOT NULL,
)
GO
CREATE TABLE TABLE_NATIONALITY
(
	NatId	INT IDENTITY(0,1) PRIMARY KEY,
	NatName NVARCHAR(20)
)
GO
--EXEC sp_rename 'RENTER.[IDENTITYNum]', 'IdentityNum', 'COLUMN'
CREATE TABLE ROOM_TYPE												
(
	RoomTypeId		CHAR(1) NOT NULL PRIMARY KEY,		
	PriceByDay		MONEY NOT NULL,
	PriceFirstHour	MONEY NOT NULL,
	PricePerHour	MONEY NOT NULL,
	Note			NVARCHAR(100)
)
GO
CREATE TABLE ROOM_STATUS
(
	StatusID	INT IDENTITY (0,1) NOT NULL PRIMARY KEY,
	StatusName	NVARCHAR(20)
)													
GO
CREATE TABLE ROOM													
(
	RoomId		INT NOT NULL PRIMARY KEY,
	RoomTypeId	CHAR(1),
	Description	NVARCHAR(200),
	StatusId	INT,
	Capacity	SMALLINT
	FOREIGN KEY (RoomTypeId) REFERENCES ROOM_TYPE(RoomTypeId),
	FOREIGN KEY (StatusId) REFERENCES ROOM_STATUS(StatusId)
)
GO
CREATE TABLE RENTER														
(
	RenterId	VARCHAR(50) NOT NULL PRIMARY KEY DEFAULT NEWID(),
	Name		NVARCHAR(30) NOT NULL,
	Gender		bit NOT NULL,
	PhoneNum	VARCHAR(15),
	NatId		INT,													
	IdentityNum	VARCHAR(20) UNIQUE,
	Address		VARCHAR(40)

	FOREIGN KEY (NatId) REFERENCES TABLE_NATIONALITY(NatId)
)
GO
CREATE TABLE SERVICE_TYPE
(
	SvTypeId	INT IDENTITY(0,1) PRIMARY KEY,
	SvTypeName	NVARCHAR(30),
)
GO
CREATE TABLE SERVICE 
(
	ServId	INT IDENTITY PRIMARY KEY,
	Name	VARCHAR(15),
	Price	MONEY NOT NULL DEFAULT 0,
	Unit	NVARCHAR(10),
	SvTypeId INT,
)
GO
CREATE TABLE BILL														
(
	BillId		VARCHAR(20) NOT NULL PRIMARY KEY,
	RenterId	VARCHAR(20),
	RoomId		INT,
	TotalMoney	MONEY NOT NULL,
	RenterName VARCHAR(30) NOT NULL,
	CompanyName VARCHAR(MAX) NOT NULL,
	
	FOREIGN KEY (RoomId) REFERENCES ROOM(RoomId),
	FOREIGN KEY (RenterId) REFERENCES RENTER(RenterId)
)
GO
CREATE TABLE REG_FORM														
(
	FormId		INT IDENTITY PRIMARY KEY,
	CheckIn		SMALLDATETIME NOT NULL,
	CheckOut	SMALLDATETIME,
	RenterId	VARCHAR(50),
	RoomId		INT,
	BillId		VARCHAR(20),
	Rental		MONEY,

	FOREIGN KEY (RenterId) REFERENCES RENTER(RenterId),
	FOREIGN KEY (RoomId) REFERENCES ROOM(RoomId),
	FOREIGN KEY (BillId) REFERENCES BILL(BillId),
)
GO
CREATE TABLE USE_SERVICES
(
	ServId	INT,
	FormId	INT,
	Time	DateTime,
	Quantity INT,

	PRIMARY KEY (ServId, FormId),
	FOREIGN KEY (ServId) REFERENCES SERVICE(ServId),
	FOREIGN KEY (FormId) REFERENCES REG_FORM(FormId)
)
GO
CREATE TABLE ROOMMATE
(
	Name	NVARCHAR(30),
	IdentityNum VARCHAR(10),
	NatId	INT,
	FormId	INT,
	RenterId VARCHAR(50),

	PRIMARY KEY (FormId, Name),
	FOREIGN KEY (FormId) REFERENCES REG_FORM(FormId),
	FOREIGN KEY (NatId) REFERENCES TABLE_NATIONALITY(NatId)
)
GO
CREATE TABLE FEE
(
	Id	INT IDENTITY PRIMARY KEY,
	FeeForEachMoreGuest	FLOAT NOT NULL,
	FeeForForeigner FLOAT NOT NULL
)
GO
------------------------Procedure--
CREATE PROC USP_GetAllRoommatesByFormId
	@formid INT
AS
	SELECT RM.Name, RM.IdentityNum, NAT.NatId, NAT.NatName
	FROM dbo.ROOMMATE AS RM
	JOIN dbo.TABLE_NATIONALITY AS NAT
		ON NAT.NatId = RM.NatId
	WHERE RM.FormId = @formid
GO
CREATE PROC USP_GetAllServicesInfo
AS
	SELECT SV.ServId, SV.Name, SV.Price, SV.SvTypeId, SV.Unit, ST.SvTypeName
	FROM dbo.SERVICE AS SV 
	JOIN dbo.SERVICE_TYPE AS ST
		ON ST.SvTypeId = SV.SvTypeId
GO
CREATE PROC USP_GetAllRoomInfo
AS
	SELECT R.RoomId, R.RoomTypeId, R.StatusId, R.Capacity ,RS.StatusName, R.Description
	FROM dbo.ROOM AS R
	JOIN dbo.ROOM_STATUS AS RS
		ON RS.StatusID = R.StatusId
GO
CREATE PROC USP_GetRoomStatusInfo
AS
	SELECT RS.StatusId, RS.StatusName, COUNT(R.RoomId) AS ITEMCOUNT
	FROM dbo.ROOM AS R
	RIGHT JOIN dbo.ROOM_STATUS AS RS
		ON RS.StatusID = R.StatusId
	GROUP BY RS.StatusId, RS.StatusName
GO
CREATE PROC USP_GetSelectedFormId
	@roomid INT
AS
	SELECT FormId
	FROM dbo.REG_FORM
	WHERE RoomId = @roomid AND CheckOut IS NULL
GO
CREATE PROC USP_GetServicesBeingUsed
	@formid INT
AS
	SELECT US.FormId, US.Time, US.Quantity, US.ServId, S.Name
	FROM dbo.USE_SERVICES AS US
	JOIN dbo.SERVICE AS S
		ON S.ServId = US.ServId
	WHERE US.FormId = @formid
GO
CREATE PROC USP_GetDataForReporting
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
CREATE PROC USP_GetNewestServiceInfo
AS
	SELECT TOP 1 S.ServId
	FROM dbo.SERVICE AS S 
	JOIN dbo.SERVICE_TYPE AS ST
		ON ST.SvTypeId = S.SvTypeId
	ORDER BY S.ServId DESC
GO
CREATE PROC USP_GetNewestNationalityId
AS
	SELECT TOP 1 NAT.NatId
	FROM dbo.TABLE_NATIONALITY AS NAT
	ORDER BY NAT.NatId DESC
GO
CREATE PROC USP_LogIn
	@username VARCHAR(20),
	@password VARCHAR(20)
AS
	SELECT *
	FROM dbo.ACCOUNT
	WHERE Password = @password AND UserName = @username
GO
CREATE PROC USP_ServiceCharge
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

ALTER FUNCTION Rental(@formid INT, @checkout DATETIME)
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
SELECT dbo.Rental(3, GETDATE())
GO
CREATE PROC USP_GetUsingRoom
AS
	SELECT RF.RoomId, RF.FormId, RF.CheckIn
	FROM dbo.REG_FORM AS RF
	WHERE RF.CheckOut IS NULL
GO
