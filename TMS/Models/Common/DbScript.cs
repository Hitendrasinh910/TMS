
/*
CREATE TABLE [dbo].[Master_AccountType](
	[IDAccountType] [int] IDENTITY(1,1) NOT NULL,
	[AccountType] [nvarchar](255) NULL,
	[E_Date] [datetime] NULL,
	[E_By] [nvarchar](255) NULL,
	[E_ById] [int] NULL,
	[U_Date] [datetime] NULL,
	[U_By] [nvarchar](255) NULL,
	[U_ById] [int] NULL,
	[D_Date] [datetime] NULL,
	[D_By] [nvarchar](255) NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[IDAccountType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Master_BalanceType](
	[IDBalanceType] [int] IDENTITY(1,1) NOT NULL,
	[BalanceType] [nvarchar](100) NULL,
	[E_Date] [datetime] NULL,
	[E_By] [nvarchar](100) NULL,
	[E_ById] [int] NULL,
	[U_Date] [datetime] NULL,
	[U_By] [nvarchar](100) NULL,
	[U_ById] [int] NULL,
	[D_Date] [datetime] NULL,
	[D_By] [nvarchar](100) NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[IDBalanceType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Master_BillToParty](
	[IDPartyBill] [int] IDENTITY(1,1) NOT NULL,
	[SrNo] [int] NULL,
	[IDConsignor] [int] NULL,
	[IDConsignee] [int] NULL,
	[BillTo] [int] NULL,
	[Remarks] [nvarchar](max) NULL,
	[E_Date] [datetime] NULL,
	[E_By] [nvarchar](255) NULL,
	[E_ById] [int] NULL,
	[U_Date] [datetime] NULL,
	[U_By] [nvarchar](255) NULL,
	[U_ById] [int] NULL,
	[D_Date] [datetime] NULL,
	[D_By] [nvarchar](255) NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[IDPartyBill] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Master_City](
	[IDCity] [int] IDENTITY(1,1) NOT NULL,
	[IDState] [int] NULL,
	[City] [nvarchar](50) NULL,
	[E_Date] [datetime] NULL,
	[E_By] [nvarchar](100) NULL,
	[E_ById] [int] NULL,
	[U_Date] [datetime] NULL,
	[U_By] [nvarchar](100) NULL,
	[U_ById] [int] NULL,
	[D_Date] [datetime] NULL,
	[D_By] [nvarchar](100) NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[IDCity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Master_Country](
	[IDCountry] [int] IDENTITY(1,1) NOT NULL,
	[Country] [nvarchar](50) NULL,
	[E_Date] [datetime] NULL,
	[E_By] [nvarchar](100) NULL,
	[E_ById] [int] NULL,
	[U_Date] [datetime] NULL,
	[U_By] [nvarchar](100) NULL,
	[U_ById] [int] NULL,
	[D_Date] [datetime] NULL,
	[D_By] [nvarchar](100) NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[IDCountry] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Master_Driver](
	[IDDriver] [int] IDENTITY(1,1) NOT NULL,
	[DriverName] [nvarchar](100) NULL,
	[Address] [nvarchar](255) NULL,
	[ContactNo] [nvarchar](20) NULL,
	[EmergencyContactNo] [nvarchar](20) NULL,
	[DrivingLicenceNo] [nvarchar](20) NULL,
	[DLValidTill] [date] NULL,
	[E_Date] [datetime] NULL,
	[E_By] [nvarchar](100) NULL,
	[E_ById] [int] NULL,
	[U_Date] [datetime] NULL,
	[U_By] [nvarchar](100) NULL,
	[U_ById] [int] NULL,
	[D_Date] [datetime] NULL,
	[D_By] [nvarchar](100) NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[IDDriver] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Master_PartyAccount](
	[IDPartyAccount] [int] IDENTITY(1,1) NOT NULL,
	[AccountSrNo] [int] NULL,
	[PartyCode] [nvarchar](50) NULL,
	[IDAccountType] [int] NULL,
	[PartyName] [nvarchar](100) NULL,
	[Address] [nvarchar](255) NULL,
	[IDState] [int] NULL,
	[IDCity] [int] NULL,
	[ContactNo1] [nvarchar](15) NULL,
	[ContactNo2] [nvarchar](15) NULL,
	[Email] [nvarchar](50) NULL,
	[OpeningBalance] [decimal](18, 2) NULL,
	[IDBalanceType] [int] NULL,
	[GSTNo] [nvarchar](100) NULL,
	[PanNo] [nvarchar](100) NULL,
	[E_Date] [datetime] NULL,
	[E_By] [nvarchar](100) NULL,
	[E_ById] [int] NULL,
	[U_Date] [datetime] NULL,
	[U_By] [nvarchar](100) NULL,
	[U_ById] [int] NULL,
	[D_Date] [datetime] NULL,
	[D_By] [nvarchar](100) NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[IDPartyAccount] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Master_State](
	[IDState] [int] IDENTITY(1,1) NOT NULL,
	[IDCountry] [int] NULL,
	[State] [nvarchar](255) NULL,
	[E_Date] [datetime] NULL,
	[E_By] [nvarchar](255) NULL,
	[E_ById] [int] NULL,
	[U_Date] [datetime] NULL,
	[U_By] [nvarchar](255) NULL,
	[U_ById] [int] NULL,
	[D_Date] [datetime] NULL,
	[D_By] [nvarchar](255) NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[IDState] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Master_Truck](
	[IDTruck] [int] IDENTITY(1,1) NOT NULL,
	[TruckNumber] [nvarchar](20) NULL,
	[PanCardHolder] [nvarchar](100) NULL,
	[PanCardNo] [nvarchar](20) NULL,
	[Remarks] [nvarchar](max) NULL,
	[E_Date] [datetime] NULL,
	[E_By] [nvarchar](100) NULL,
	[E_ById] [int] NULL,
	[U_Date] [datetime] NULL,
	[U_By] [nvarchar](100) NULL,
	[U_ById] [int] NULL,
	[D_Date] [datetime] NULL,
	[D_By] [nvarchar](100) NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[IDTruck] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Master_User](
	[IDUser] [int] IDENTITY(1,1) NOT NULL,
	[UserType] [nvarchar](30) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
	[FullName] [nvarchar](150) NOT NULL,
	[Email] [nvarchar](150) NULL,
	[ContactNo] [nvarchar](15) NULL,
	[E_Date] [datetime] NULL,
	[E_By] [nvarchar](100) NULL,
	[E_ById] [int] NULL,
	[U_Date] [datetime] NULL,
	[U_By] [nvarchar](100) NULL,
	[U_ById] [int] NULL,
	[D_Date] [datetime] NULL,
	[D_By] [nvarchar](100) NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[IDUser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Master_AccountType] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Master_BalanceType] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Master_BillToParty] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Master_City] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Master_Country] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Master_Driver] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Master_PartyAccount] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Master_State] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Master_Truck] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Master_User] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- DELETE
CREATE PROCEDURE [dbo].[usp_Master_City_Delete] (@IDCity INT, @DeletedBy NVARCHAR(100))
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE dbo.Master_City SET IsDeleted = 1, D_Date = GETDATE(), D_By = @DeletedBy WHERE IDCity = @IDCity AND IsDeleted = 0;
        SELECT Result = 1, Message = 'City deleted successfully', NewId = @IDCity, ErrorCode = '';
    END TRY
    BEGIN CATCH SELECT Result = -1, Message = ERROR_MESSAGE(), NewId = NULL, ErrorCode = 'SQL_ERROR'; END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SAVE
CREATE PROCEDURE [dbo].[usp_Master_City_Save]
(
    @IDCity   INT = 0,
    @IDState  INT,
    @City     NVARCHAR(50),
    @UserName NVARCHAR(100)
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM dbo.Master_City WHERE City = @City AND IDState = @IDState AND IsDeleted = 0 AND IDCity <> @IDCity)
        BEGIN SELECT Result = -1, Message = 'City already exists for this state', NewId = NULL, ErrorCode = 'DUPLICATE'; RETURN; END

        IF @IDCity = 0
        BEGIN
            INSERT INTO dbo.Master_City (IDState, City, E_Date, E_By, IsDeleted) VALUES (@IDState, @City, GETDATE(), @UserName, 0);
            SELECT Result = 1, Message = 'City created successfully', NewId = SCOPE_IDENTITY(), ErrorCode = '';
        END
        ELSE
        BEGIN
            UPDATE dbo.Master_City SET IDState = @IDState, City = @City, U_Date = GETDATE(), U_By = @UserName WHERE IDCity = @IDCity AND IsDeleted = 0;
            SELECT Result = 1, Message = 'City updated successfully', NewId = @IDCity, ErrorCode = '';
        END
    END TRY
    BEGIN CATCH SELECT Result = -1, Message = ERROR_MESSAGE(), NewId = NULL, ErrorCode = 'SQL_ERROR'; END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SELECT
CREATE PROCEDURE [dbo].[usp_Master_City_Select]
AS BEGIN SET NOCOUNT ON; SELECT c.*, s.State AS StateName FROM dbo.Master_City c LEFT JOIN dbo.Master_State s ON c.IDState = s.IDState WHERE c.IsDeleted = 0 ORDER BY c.City; END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SELECT BY ID
CREATE PROCEDURE [dbo].[usp_Master_City_SelectById] (@IDCity INT)
AS BEGIN SET NOCOUNT ON; SELECT * FROM dbo.Master_City WHERE IDCity = @IDCity AND IsDeleted = 0; END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create   PROCEDURE [dbo].[usp_Master_Country_Delete]
(
    @IDCountry INT,
    @DeletedBy NVARCHAR(100)
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        UPDATE dbo.Master_Country
        SET
            IsDeleted = 1,
            D_Date = GETDATE(),
            D_By = @DeletedBy
        WHERE IDCountry = @IDCountry
          AND IsDeleted = 0;

        IF @@ROWCOUNT = 0
        BEGIN
            SELECT
                Result = -1,
                Message = 'Country not found or already deleted',
                NewId = NULL,
                ErrorCode = 'NOT_FOUND';
            RETURN;
        END

        SELECT
            Result = 1,
            Message = 'Country deleted successfully',
            NewId = @IDCountry,
            ErrorCode = '';

    END TRY
    BEGIN CATCH
        SELECT
            Result = -1,
            Message = ERROR_MESSAGE(),
            NewId = NULL,
            ErrorCode = 'SQL_ERROR';
    END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_Master_Country_Save]
(
    @IDCountry  INT = 0,
    @Country    NVARCHAR(100),
    @UserName   NVARCHAR(100)
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        IF EXISTS (
            SELECT 1 
            FROM dbo.Master_Country 
            WHERE Country = @Country 
              AND IsDeleted = 0 
              AND IDCountry <> @IDCountry
        )
        BEGIN
            SELECT 
                Result = -1, 
                Message = 'Country name already exists', 
                NewId = NULL, 
                ErrorCode = 'DUPLICATE';
            RETURN;
        END

        IF @IDCountry = 0
        BEGIN
            INSERT INTO dbo.Master_Country
            (
                Country,
                E_Date,
                E_By,
                IsDeleted
            )
            VALUES
            (
                @Country,
                GETDATE(),
                @UserName,
                0
            );

            DECLARE @NewId INT = SCOPE_IDENTITY();

            SELECT 
                Result = 1, 
                Message = 'Country created successfully', 
                NewId = @NewId, 
                ErrorCode = '';
            RETURN;
        END

        UPDATE dbo.Master_Country
        SET 
            Country = @Country,
            U_Date  = GETDATE(),
            U_By    = @UserName
        WHERE IDCountry = @IDCountry 
          AND IsDeleted = 0;

        IF @@ROWCOUNT = 0
        BEGIN
            SELECT 
                Result = -1, 
                Message = 'Country not found or already deleted', 
                NewId = NULL, 
                ErrorCode = 'NOT_FOUND';
            RETURN;
        END

        SELECT 
            Result = 1, 
            Message = 'Country updated successfully', 
            NewId = @IDCountry, 
            ErrorCode = '';

    END TRY
    BEGIN CATCH
        SELECT 
            Result = -1, 
            Message = ERROR_MESSAGE(), 
            NewId = NULL, 
            ErrorCode = 'SQL_ERROR';
    END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create   PROCEDURE [dbo].[usp_Master_Country_Select]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IDCountry,
        Country,
        E_Date,
        E_By
    FROM dbo.Master_Country
    WHERE IsDeleted = 0
    ORDER BY Country;
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create   PROCEDURE [dbo].[usp_Master_Country_SelectById]
(
    @IDCountry INT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
       *
    FROM dbo.Master_Country
    WHERE IDCountry = @IDCountry
      AND IsDeleted = 0;
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- DELETE
CREATE PROCEDURE [dbo].[usp_Master_Driver_Delete] (@IDDriver INT, @DeletedBy NVARCHAR(100))
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE dbo.Master_Driver SET IsDeleted = 1, D_Date = GETDATE(), D_By = @DeletedBy WHERE IDDriver = @IDDriver AND IsDeleted = 0;
        SELECT Result = 1, Message = 'Driver deleted successfully', NewId = @IDDriver, ErrorCode = '';
    END TRY
    BEGIN CATCH SELECT Result = -1, Message = ERROR_MESSAGE(), NewId = NULL, ErrorCode = 'SQL_ERROR'; END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- SAVE
CREATE PROCEDURE [dbo].[usp_Master_Driver_Save]
(
    @IDDriver           INT = 0,
    @DriverName         NVARCHAR(100),
    @Address            NVARCHAR(255) = NULL,
    @ContactNo          NVARCHAR(20),
    @EmergencyContactNo NVARCHAR(20) = NULL,
    @DrivingLicenceNo   NVARCHAR(20) = NULL,
    @DLValidTill        DATE = NULL,
    @UserName           NVARCHAR(100)
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF @IDDriver = 0
        BEGIN
            INSERT INTO dbo.Master_Driver (DriverName, Address, ContactNo, EmergencyContactNo, DrivingLicenceNo, DLValidTill, E_Date, E_By, IsDeleted) 
            VALUES (@DriverName, @Address, @ContactNo, @EmergencyContactNo, @DrivingLicenceNo, @DLValidTill, GETDATE(), @UserName, 0);
            SELECT Result = 1, Message = 'Driver created successfully', NewId = SCOPE_IDENTITY(), ErrorCode = '';
        END
        ELSE
        BEGIN
            UPDATE dbo.Master_Driver SET DriverName = @DriverName, Address = @Address, ContactNo = @ContactNo, EmergencyContactNo = @EmergencyContactNo, DrivingLicenceNo = @DrivingLicenceNo, DLValidTill = @DLValidTill, U_Date = GETDATE(), U_By = @UserName 
            WHERE IDDriver = @IDDriver AND IsDeleted = 0;
            SELECT Result = 1, Message = 'Driver updated successfully', NewId = @IDDriver, ErrorCode = '';
        END
    END TRY
    BEGIN CATCH SELECT Result = -1, Message = ERROR_MESSAGE(), NewId = NULL, ErrorCode = 'SQL_ERROR'; END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SELECT & SELECT BY ID
CREATE PROCEDURE [dbo].[usp_Master_Driver_Select] AS BEGIN SET NOCOUNT ON; SELECT * FROM dbo.Master_Driver WHERE IsDeleted = 0 ORDER BY DriverName; END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_Master_Driver_SelectById] (@IDDriver INT) AS BEGIN SET NOCOUNT ON; SELECT * FROM dbo.Master_Driver WHERE IDDriver = @IDDriver AND IsDeleted = 0; END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- DELETE
CREATE PROCEDURE [dbo].[usp_Master_PartyAccount_Delete] (@IDPartyAccount INT, @DeletedBy NVARCHAR(100))
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE dbo.Master_PartyAccount SET IsDeleted = 1, D_Date = GETDATE(), D_By = @DeletedBy WHERE IDPartyAccount = @IDPartyAccount AND IsDeleted = 0;
        SELECT Result = 1, Message = 'Party Account deleted successfully', NewId = @IDPartyAccount, ErrorCode = '';
    END TRY
    BEGIN CATCH SELECT Result = -1, Message = ERROR_MESSAGE(), NewId = NULL, ErrorCode = 'SQL_ERROR'; END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SAVE
CREATE PROCEDURE [dbo].[usp_Master_PartyAccount_Save]
(
    @IDPartyAccount INT = 0,
    @AccountSrNo    INT = NULL,
    @PartyCode      NVARCHAR(50) = NULL,
    @IDAccountType  INT = NULL,
    @PartyName      NVARCHAR(100),
    @Address        NVARCHAR(255) = NULL,
    @IDState        INT = NULL,
    @IDCity         INT = NULL,
    @ContactNo1     NVARCHAR(15),
    @ContactNo2     NVARCHAR(15) = NULL,
    @Email          NVARCHAR(50) = NULL,
    @OpeningBalance DECIMAL(18,2) = 0,
    @IDBalanceType  INT = NULL,
    @GSTNo          NVARCHAR(100) = NULL,
    @PanNo          NVARCHAR(100) = NULL,
    @UserName       NVARCHAR(100)
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM dbo.Master_PartyAccount WHERE PartyName = @PartyName AND IsDeleted = 0 AND IDPartyAccount <> @IDPartyAccount)
        BEGIN SELECT Result = -1, Message = 'Party Name already exists', NewId = NULL, ErrorCode = 'DUPLICATE'; RETURN; END

        IF @IDPartyAccount = 0
        BEGIN
            INSERT INTO dbo.Master_PartyAccount (AccountSrNo, PartyCode, IDAccountType, PartyName, Address, IDState, IDCity, ContactNo1, ContactNo2, Email, OpeningBalance, IDBalanceType, GSTNo, PanNo, E_Date, E_By, IsDeleted)
            VALUES (@AccountSrNo, @PartyCode, @IDAccountType, @PartyName, @Address, @IDState, @IDCity, @ContactNo1, @ContactNo2, @Email, @OpeningBalance, @IDBalanceType, @GSTNo, @PanNo, GETDATE(), @UserName, 0);
            
            SELECT Result = 1, Message = 'Party Account created successfully', NewId = SCOPE_IDENTITY(), ErrorCode = '';
        END
        ELSE
        BEGIN
            UPDATE dbo.Master_PartyAccount SET AccountSrNo = @AccountSrNo, PartyCode = @PartyCode, IDAccountType = @IDAccountType, PartyName = @PartyName, Address = @Address, IDState = @IDState, IDCity = @IDCity, ContactNo1 = @ContactNo1, ContactNo2 = @ContactNo2, Email = @Email, OpeningBalance = @OpeningBalance, IDBalanceType = @IDBalanceType, GSTNo = @GSTNo, PanNo = @PanNo, U_Date = GETDATE(), U_By = @UserName
            WHERE IDPartyAccount = @IDPartyAccount AND IsDeleted = 0;
            
            SELECT Result = 1, Message = 'Party Account updated successfully', NewId = @IDPartyAccount, ErrorCode = '';
        END
    END TRY
    BEGIN CATCH SELECT Result = -1, Message = ERROR_MESSAGE(), NewId = NULL, ErrorCode = 'SQL_ERROR'; END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SELECT
CREATE PROCEDURE [dbo].[usp_Master_PartyAccount_Select] 
AS BEGIN 
    SET NOCOUNT ON; 
    SELECT p.*, c.City AS CityName 
    FROM dbo.Master_PartyAccount p 
    LEFT JOIN dbo.Master_City c ON p.IDCity = c.IDCity 
    WHERE p.IsDeleted = 0 
    ORDER BY p.PartyName; 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SELECT BY ID
CREATE PROCEDURE [dbo].[usp_Master_PartyAccount_SelectById] (@IDPartyAccount INT) AS BEGIN SET NOCOUNT ON; SELECT * FROM dbo.Master_PartyAccount WHERE IDPartyAccount = @IDPartyAccount AND IsDeleted = 0; END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- DELETE
CREATE PROCEDURE [dbo].[usp_Master_State_Delete] (@IDState INT, @DeletedBy NVARCHAR(255))
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE dbo.Master_State SET IsDeleted = 1, D_Date = GETDATE(), D_By = @DeletedBy WHERE IDState = @IDState AND IsDeleted = 0;
        SELECT Result = 1, Message = 'State deleted successfully', NewId = @IDState, ErrorCode = '';
    END TRY
    BEGIN CATCH SELECT Result = -1, Message = ERROR_MESSAGE(), NewId = NULL, ErrorCode = 'SQL_ERROR'; END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- SAVE
CREATE PROCEDURE [dbo].[usp_Master_State_Save]
(
    @IDState   INT = 0,
    @IDCountry INT,
    @State     NVARCHAR(255),
    @UserName  NVARCHAR(255)
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        -- Duplicate Check
        IF EXISTS (SELECT 1 FROM dbo.Master_State WHERE State = @State AND IDCountry = @IDCountry AND IsDeleted = 0 AND IDState <> @IDState)
        BEGIN
            SELECT Result = -1, Message = 'State name already exists for this country', NewId = NULL, ErrorCode = 'DUPLICATE';
            RETURN;
        END

        IF @IDState = 0
        BEGIN
            INSERT INTO dbo.Master_State (IDCountry, State, E_Date, E_By, IsDeleted)
            VALUES (@IDCountry, @State, GETDATE(), @UserName, 0);

            SELECT Result = 1, Message = 'State created successfully', NewId = SCOPE_IDENTITY(), ErrorCode = '';
        END
        ELSE
        BEGIN
            UPDATE dbo.Master_State SET IDCountry = @IDCountry, State = @State, U_Date = GETDATE(), U_By = @UserName
            WHERE IDState = @IDState AND IsDeleted = 0;

            SELECT Result = 1, Message = 'State updated successfully', NewId = @IDState, ErrorCode = '';
        END
    END TRY
    BEGIN CATCH
        SELECT Result = -1, Message = ERROR_MESSAGE(), NewId = NULL, ErrorCode = 'SQL_ERROR';
    END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SELECT
CREATE PROCEDURE [dbo].[usp_Master_State_Select]
AS BEGIN SET NOCOUNT ON; SELECT s.*, c.Country AS CountryName FROM dbo.Master_State s LEFT JOIN dbo.Master_Country c ON s.IDCountry = c.IDCountry WHERE s.IsDeleted = 0 ORDER BY s.State; END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SELECT BY ID
CREATE PROCEDURE [dbo].[usp_Master_State_SelectById] (@IDState INT)
AS BEGIN SET NOCOUNT ON; SELECT * FROM dbo.Master_State WHERE IDState = @IDState AND IsDeleted = 0; END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- DELETE
CREATE PROCEDURE [dbo].[usp_Master_Truck_Delete] (@IDTruck INT, @DeletedBy NVARCHAR(100))
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE dbo.Master_Truck SET IsDeleted = 1, D_Date = GETDATE(), D_By = @DeletedBy WHERE IDTruck = @IDTruck AND IsDeleted = 0;
        SELECT Result = 1, Message = 'Truck deleted successfully', NewId = @IDTruck, ErrorCode = '';
    END TRY
    BEGIN CATCH SELECT Result = -1, Message = ERROR_MESSAGE(), NewId = NULL, ErrorCode = 'SQL_ERROR'; END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- SAVE
CREATE PROCEDURE [dbo].[usp_Master_Truck_Save]
(
    @IDTruck       INT = 0,
    @TruckNumber   NVARCHAR(20),
    @PanCardHolder NVARCHAR(100) = NULL,
    @PanCardNo     NVARCHAR(20) = NULL,
    @Remarks       NVARCHAR(MAX) = NULL,
    @UserName      NVARCHAR(100)
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM dbo.Master_Truck WHERE TruckNumber = @TruckNumber AND IsDeleted = 0 AND IDTruck <> @IDTruck)
        BEGIN SELECT Result = -1, Message = 'Truck Number already exists', NewId = NULL, ErrorCode = 'DUPLICATE'; RETURN; END

        IF @IDTruck = 0
        BEGIN
            INSERT INTO dbo.Master_Truck (TruckNumber, PanCardHolder, PanCardNo, Remarks, E_Date, E_By, IsDeleted) 
            VALUES (@TruckNumber, @PanCardHolder, @PanCardNo, @Remarks, GETDATE(), @UserName, 0);
            SELECT Result = 1, Message = 'Truck created successfully', NewId = SCOPE_IDENTITY(), ErrorCode = '';
        END
        ELSE
        BEGIN
            UPDATE dbo.Master_Truck SET TruckNumber = @TruckNumber, PanCardHolder = @PanCardHolder, PanCardNo = @PanCardNo, Remarks = @Remarks, U_Date = GETDATE(), U_By = @UserName 
            WHERE IDTruck = @IDTruck AND IsDeleted = 0;
            SELECT Result = 1, Message = 'Truck updated successfully', NewId = @IDTruck, ErrorCode = '';
        END
    END TRY
    BEGIN CATCH SELECT Result = -1, Message = ERROR_MESSAGE(), NewId = NULL, ErrorCode = 'SQL_ERROR'; END CATCH
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SELECT
CREATE PROCEDURE [dbo].[usp_Master_Truck_Select] AS BEGIN SET NOCOUNT ON; SELECT * FROM dbo.Master_Truck WHERE IsDeleted = 0 ORDER BY TruckNumber; END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SELECT BY ID
CREATE PROCEDURE [dbo].[usp_Master_Truck_SelectById] (@IDTruck INT) AS BEGIN SET NOCOUNT ON; SELECT * FROM dbo.Master_Truck WHERE IDTruck = @IDTruck AND IsDeleted = 0; END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create   PROCEDURE [dbo].[usp_Master_User_Login]
(
    @UserName NVARCHAR(50),
    @Password NVARCHAR(100)
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        *
    FROM dbo.Master_User 
    WHERE UserName = @UserName
      AND Password = @Password
      AND IsDeleted = 0;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[usp_Master_User_Save]
(
    @IDUser    INT = 0,
    @UserType  NVARCHAR(30),
    @UserName  NVARCHAR(50),
    @Password  NVARCHAR(100),
    @FullName  NVARCHAR(150),
    @Email     NVARCHAR(150) = NULL,
    @ContactNo NVARCHAR(15)  = NULL,
    @ActionBy  NVARCHAR(100),
	@ActionById INT = 0
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        
        IF EXISTS (
            SELECT 1
            FROM dbo.Master_User
            WHERE UserName = @UserName
              AND IsDeleted = 0
              AND IDUser <> @IDUser
        )
        BEGIN
            SELECT
                Result = -1,
                Message = 'Username already exists',
                NewId = NULL,
                ErrorCode = 'DUPLICATE';
            RETURN;
        END

       
        IF @IDUser = 0
        BEGIN
            INSERT INTO dbo.Master_User
            (
                UserName,
                Password,
                FullName,
                Email,
                ContactNo,
                UserType,
                E_Date,
                E_By,
				E_ById,
                IsDeleted
            )
            VALUES
            (
                @UserName,
                @Password,
                @FullName,
                @Email,
                @ContactNo,
                @UserType,
                GETDATE(),
                @ActionBy,
				@ActionById,
                0
            );

            SELECT
                Result = 1,
                Message = 'User created successfully',
                NewId = SCOPE_IDENTITY(),
                ErrorCode = '';
            RETURN;
        END

        
        UPDATE dbo.Master_User
        SET
            UserName = @UserName,
            Password = @Password,
            FullName = @FullName,
            Email    = @Email,
            ContactNo = @ContactNo,
            UserType = @UserType,
            U_Date   = GETDATE(),
            U_By     = @ActionBy
        WHERE IDUser = @IDUser
          AND IsDeleted = 0;

        IF @@ROWCOUNT = 0
        BEGIN
            SELECT
                Result = -1,
                Message = 'User not found or deleted',
                NewId = NULL,
                ErrorCode = 'NOT_FOUND';
            RETURN;
        END

        SELECT
            Result = 1,
            Message = 'User updated successfully',
            NewId = @IDUser,
            ErrorCode = '';

    END TRY
    BEGIN CATCH
        SELECT
            Result = -1,
            Message = ERROR_MESSAGE(),
            NewId = NULL,
            ErrorCode = 'SQL_ERROR';
    END CATCH
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create   PROCEDURE [dbo].[usp_Master_User_Select]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IDUser,
        UserName,
        FullName,
        Email,
        ContactNo,
        UserType
    FROM dbo.Master_User
    WHERE IsDeleted = 0
    ORDER BY FullName;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create   PROCEDURE [dbo].[usp_Master_User_SelectById]
(
    @IDUser INT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
       *
    FROM dbo.Master_User
    WHERE IDUser = @IDUser
      AND IsDeleted = 0;
END
GO
USE [master]
GO
ALTER DATABASE [dbTMS] SET  READ_WRITE 
GO

CREATE TABLE [dbo].[Master_AccountType](
	[IDAccountType] [int] IDENTITY(1,1) NOT NULL,
	[AccountType] [nvarchar](255) NULL,
	[E_Date] [datetime] NULL,
	[E_By] [nvarchar](255) NULL,
	[E_ById] [int] NULL,
	[U_Date] [datetime] NULL,
	[U_By] [nvarchar](255) NULL,
	[U_ById] [int] NULL,
	[D_Date] [datetime] NULL,
	[D_By] [nvarchar](255) NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[IDAccountType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Master_BalanceType](
	[IDBalanceType] [int] IDENTITY(1,1) NOT NULL,
	[BalanceType] [nvarchar](100) NULL,
	[E_Date] [datetime] NULL,
	[E_By] [nvarchar](100) NULL,
	[E_ById] [int] NULL,
	[U_Date] [datetime] NULL,
	[U_By] [nvarchar](100) NULL,
	[U_ById] [int] NULL,
	[D_Date] [datetime] NULL,
	[D_By] [nvarchar](100) NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[IDBalanceType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Master_BillToParty](
	[IDPartyBill] [int] IDENTITY(1,1) NOT NULL,
	[SrNo] [int] NULL,
	[IDConsignor] [int] NULL,
	[IDConsignee] [int] NULL,
	[BillTo] [int] NULL,
	[Remarks] [nvarchar](max) NULL,
	[E_Date] [datetime] NULL,
	[E_By] [nvarchar](255) NULL,
	[E_ById] [int] NULL,
	[U_Date] [datetime] NULL,
	[U_By] [nvarchar](255) NULL,
	[U_ById] [int] NULL,
	[D_Date] [datetime] NULL,
	[D_By] [nvarchar](255) NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[IDPartyBill] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Master_City](
	[IDCity] [int] IDENTITY(1,1) NOT NULL,
	[IDState] [int] NULL,
	[City] [nvarchar](50) NULL,
	[E_Date] [datetime] NULL,
	[E_By] [nvarchar](100) NULL,
	[E_ById] [int] NULL,
	[U_Date] [datetime] NULL,
	[U_By] [nvarchar](100) NULL,
	[U_ById] [int] NULL,
	[D_Date] [datetime] NULL,
	[D_By] [nvarchar](100) NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[IDCity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Master_Country](
	[IDCountry] [int] IDENTITY(1,1) NOT NULL,
	[Country] [nvarchar](50) NULL,
	[E_Date] [datetime] NULL,
	[E_By] [nvarchar](100) NULL,
	[E_ById] [int] NULL,
	[U_Date] [datetime] NULL,
	[U_By] [nvarchar](100) NULL,
	[U_ById] [int] NULL,
	[D_Date] [datetime] NULL,
	[D_By] [nvarchar](100) NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[IDCountry] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Master_Driver](
	[IDDriver] [int] IDENTITY(1,1) NOT NULL,
	[DriverName] [nvarchar](100) NULL,
	[Address] [nvarchar](255) NULL,
	[ContactNo] [nvarchar](20) NULL,
	[EmergencyContactNo] [nvarchar](20) NULL,
	[DrivingLicenceNo] [nvarchar](20) NULL,
	[DLValidTill] [date] NULL,
	[E_Date] [datetime] NULL,
	[E_By] [nvarchar](100) NULL,
	[E_ById] [int] NULL,
	[U_Date] [datetime] NULL,
	[U_By] [nvarchar](100) NULL,
	[U_ById] [int] NULL,
	[D_Date] [datetime] NULL,
	[D_By] [nvarchar](100) NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[IDDriver] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Master_PartyAccount](
	[IDPartyAccount] [int] IDENTITY(1,1) NOT NULL,
	[AccountSrNo] [int] NULL,
	[PartyCode] [nvarchar](50) NULL,
	[IDAccountType] [int] NULL,
	[PartyName] [nvarchar](100) NULL,
	[Address] [nvarchar](255) NULL,
	[IDState] [int] NULL,
	[IDCity] [int] NULL,
	[ContactNo1] [nvarchar](15) NULL,
	[ContactNo2] [nvarchar](15) NULL,
	[Email] [nvarchar](50) NULL,
	[OpeningBalance] [decimal](18, 2) NULL,
	[IDBalanceType] [int] NULL,
	[GSTNo] [nvarchar](100) NULL,
	[PanNo] [nvarchar](100) NULL,
	[E_Date] [datetime] NULL,
	[E_By] [nvarchar](100) NULL,
	[E_ById] [int] NULL,
	[U_Date] [datetime] NULL,
	[U_By] [nvarchar](100) NULL,
	[U_ById] [int] NULL,
	[D_Date] [datetime] NULL,
	[D_By] [nvarchar](100) NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[IDPartyAccount] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Master_State](
	[IDState] [int] IDENTITY(1,1) NOT NULL,
	[IDCountry] [int] NULL,
	[State] [nvarchar](255) NULL,
	[E_Date] [datetime] NULL,
	[E_By] [nvarchar](255) NULL,
	[E_ById] [int] NULL,
	[U_Date] [datetime] NULL,
	[U_By] [nvarchar](255) NULL,
	[U_ById] [int] NULL,
	[D_Date] [datetime] NULL,
	[D_By] [nvarchar](255) NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[IDState] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Master_Truck](
	[IDTruck] [int] IDENTITY(1,1) NOT NULL,
	[TruckNumber] [nvarchar](20) NULL,
	[PanCardHolder] [nvarchar](100) NULL,
	[PanCardNo] [nvarchar](20) NULL,
	[Remarks] [nvarchar](max) NULL,
	[E_Date] [datetime] NULL,
	[E_By] [nvarchar](100) NULL,
	[E_ById] [int] NULL,
	[U_Date] [datetime] NULL,
	[U_By] [nvarchar](100) NULL,
	[U_ById] [int] NULL,
	[D_Date] [datetime] NULL,
	[D_By] [nvarchar](100) NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[IDTruck] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Master_User](
	[IDUser] [int] IDENTITY(1,1) NOT NULL,
	[UserType] [nvarchar](30) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
	[FullName] [nvarchar](150) NOT NULL,
	[Email] [nvarchar](150) NULL,
	[ContactNo] [nvarchar](15) NULL,
	[E_Date] [datetime] NULL,
	[E_By] [nvarchar](100) NULL,
	[E_ById] [int] NULL,
	[U_Date] [datetime] NULL,
	[U_By] [nvarchar](100) NULL,
	[U_ById] [int] NULL,
	[D_Date] [datetime] NULL,
	[D_By] [nvarchar](100) NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[IDUser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Master_AccountType] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Master_BalanceType] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Master_BillToParty] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Master_City] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Master_Country] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Master_Driver] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Master_PartyAccount] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Master_State] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Master_Truck] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Master_User] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- DELETE
CREATE PROCEDURE [dbo].[usp_Master_City_Delete] (@IDCity INT, @DeletedBy NVARCHAR(100))
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE dbo.Master_City SET IsDeleted = 1, D_Date = GETDATE(), D_By = @DeletedBy WHERE IDCity = @IDCity AND IsDeleted = 0;
        SELECT Result = 1, Message = 'City deleted successfully', NewId = @IDCity, ErrorCode = '';
    END TRY
    BEGIN CATCH SELECT Result = -1, Message = ERROR_MESSAGE(), NewId = NULL, ErrorCode = 'SQL_ERROR'; END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SAVE
CREATE PROCEDURE [dbo].[usp_Master_City_Save]
(
    @IDCity   INT = 0,
    @IDState  INT,
    @City     NVARCHAR(50),
    @UserName NVARCHAR(100)
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM dbo.Master_City WHERE City = @City AND IDState = @IDState AND IsDeleted = 0 AND IDCity <> @IDCity)
        BEGIN SELECT Result = -1, Message = 'City already exists for this state', NewId = NULL, ErrorCode = 'DUPLICATE'; RETURN; END

        IF @IDCity = 0
        BEGIN
            INSERT INTO dbo.Master_City (IDState, City, E_Date, E_By, IsDeleted) VALUES (@IDState, @City, GETDATE(), @UserName, 0);
            SELECT Result = 1, Message = 'City created successfully', NewId = SCOPE_IDENTITY(), ErrorCode = '';
        END
        ELSE
        BEGIN
            UPDATE dbo.Master_City SET IDState = @IDState, City = @City, U_Date = GETDATE(), U_By = @UserName WHERE IDCity = @IDCity AND IsDeleted = 0;
            SELECT Result = 1, Message = 'City updated successfully', NewId = @IDCity, ErrorCode = '';
        END
    END TRY
    BEGIN CATCH SELECT Result = -1, Message = ERROR_MESSAGE(), NewId = NULL, ErrorCode = 'SQL_ERROR'; END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SELECT
CREATE PROCEDURE [dbo].[usp_Master_City_Select]
AS BEGIN SET NOCOUNT ON; SELECT c.*, s.State AS StateName FROM dbo.Master_City c LEFT JOIN dbo.Master_State s ON c.IDState = s.IDState WHERE c.IsDeleted = 0 ORDER BY c.City; END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SELECT BY ID
CREATE PROCEDURE [dbo].[usp_Master_City_SelectById] (@IDCity INT)
AS BEGIN SET NOCOUNT ON; SELECT * FROM dbo.Master_City WHERE IDCity = @IDCity AND IsDeleted = 0; END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create   PROCEDURE [dbo].[usp_Master_Country_Delete]
(
    @IDCountry INT,
    @DeletedBy NVARCHAR(100)
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        UPDATE dbo.Master_Country
        SET
            IsDeleted = 1,
            D_Date = GETDATE(),
            D_By = @DeletedBy
        WHERE IDCountry = @IDCountry
          AND IsDeleted = 0;

        IF @@ROWCOUNT = 0
        BEGIN
            SELECT
                Result = -1,
                Message = 'Country not found or already deleted',
                NewId = NULL,
                ErrorCode = 'NOT_FOUND';
            RETURN;
        END

        SELECT
            Result = 1,
            Message = 'Country deleted successfully',
            NewId = @IDCountry,
            ErrorCode = '';

    END TRY
    BEGIN CATCH
        SELECT
            Result = -1,
            Message = ERROR_MESSAGE(),
            NewId = NULL,
            ErrorCode = 'SQL_ERROR';
    END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_Master_Country_Save]
(
    @IDCountry  INT = 0,
    @Country    NVARCHAR(100),
    @UserName   NVARCHAR(100)
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        IF EXISTS (
            SELECT 1 
            FROM dbo.Master_Country 
            WHERE Country = @Country 
              AND IsDeleted = 0 
              AND IDCountry <> @IDCountry
        )
        BEGIN
            SELECT 
                Result = -1, 
                Message = 'Country name already exists', 
                NewId = NULL, 
                ErrorCode = 'DUPLICATE';
            RETURN;
        END

        IF @IDCountry = 0
        BEGIN
            INSERT INTO dbo.Master_Country
            (
                Country,
                E_Date,
                E_By,
                IsDeleted
            )
            VALUES
            (
                @Country,
                GETDATE(),
                @UserName,
                0
            );

            DECLARE @NewId INT = SCOPE_IDENTITY();

            SELECT 
                Result = 1, 
                Message = 'Country created successfully', 
                NewId = @NewId, 
                ErrorCode = '';
            RETURN;
        END

        UPDATE dbo.Master_Country
        SET 
            Country = @Country,
            U_Date  = GETDATE(),
            U_By    = @UserName
        WHERE IDCountry = @IDCountry 
          AND IsDeleted = 0;

        IF @@ROWCOUNT = 0
        BEGIN
            SELECT 
                Result = -1, 
                Message = 'Country not found or already deleted', 
                NewId = NULL, 
                ErrorCode = 'NOT_FOUND';
            RETURN;
        END

        SELECT 
            Result = 1, 
            Message = 'Country updated successfully', 
            NewId = @IDCountry, 
            ErrorCode = '';

    END TRY
    BEGIN CATCH
        SELECT 
            Result = -1, 
            Message = ERROR_MESSAGE(), 
            NewId = NULL, 
            ErrorCode = 'SQL_ERROR';
    END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create   PROCEDURE [dbo].[usp_Master_Country_Select]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IDCountry,
        Country,
        E_Date,
        E_By
    FROM dbo.Master_Country
    WHERE IsDeleted = 0
    ORDER BY Country;
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create   PROCEDURE [dbo].[usp_Master_Country_SelectById]
(
    @IDCountry INT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
       *
    FROM dbo.Master_Country
    WHERE IDCountry = @IDCountry
      AND IsDeleted = 0;
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- DELETE
CREATE PROCEDURE [dbo].[usp_Master_Driver_Delete] (@IDDriver INT, @DeletedBy NVARCHAR(100))
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE dbo.Master_Driver SET IsDeleted = 1, D_Date = GETDATE(), D_By = @DeletedBy WHERE IDDriver = @IDDriver AND IsDeleted = 0;
        SELECT Result = 1, Message = 'Driver deleted successfully', NewId = @IDDriver, ErrorCode = '';
    END TRY
    BEGIN CATCH SELECT Result = -1, Message = ERROR_MESSAGE(), NewId = NULL, ErrorCode = 'SQL_ERROR'; END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- SAVE
CREATE PROCEDURE [dbo].[usp_Master_Driver_Save]
(
    @IDDriver           INT = 0,
    @DriverName         NVARCHAR(100),
    @Address            NVARCHAR(255) = NULL,
    @ContactNo          NVARCHAR(20),
    @EmergencyContactNo NVARCHAR(20) = NULL,
    @DrivingLicenceNo   NVARCHAR(20) = NULL,
    @DLValidTill        DATE = NULL,
    @UserName           NVARCHAR(100)
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF @IDDriver = 0
        BEGIN
            INSERT INTO dbo.Master_Driver (DriverName, Address, ContactNo, EmergencyContactNo, DrivingLicenceNo, DLValidTill, E_Date, E_By, IsDeleted) 
            VALUES (@DriverName, @Address, @ContactNo, @EmergencyContactNo, @DrivingLicenceNo, @DLValidTill, GETDATE(), @UserName, 0);
            SELECT Result = 1, Message = 'Driver created successfully', NewId = SCOPE_IDENTITY(), ErrorCode = '';
        END
        ELSE
        BEGIN
            UPDATE dbo.Master_Driver SET DriverName = @DriverName, Address = @Address, ContactNo = @ContactNo, EmergencyContactNo = @EmergencyContactNo, DrivingLicenceNo = @DrivingLicenceNo, DLValidTill = @DLValidTill, U_Date = GETDATE(), U_By = @UserName 
            WHERE IDDriver = @IDDriver AND IsDeleted = 0;
            SELECT Result = 1, Message = 'Driver updated successfully', NewId = @IDDriver, ErrorCode = '';
        END
    END TRY
    BEGIN CATCH SELECT Result = -1, Message = ERROR_MESSAGE(), NewId = NULL, ErrorCode = 'SQL_ERROR'; END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SELECT & SELECT BY ID
CREATE PROCEDURE [dbo].[usp_Master_Driver_Select] AS BEGIN SET NOCOUNT ON; SELECT * FROM dbo.Master_Driver WHERE IsDeleted = 0 ORDER BY DriverName; END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_Master_Driver_SelectById] (@IDDriver INT) AS BEGIN SET NOCOUNT ON; SELECT * FROM dbo.Master_Driver WHERE IDDriver = @IDDriver AND IsDeleted = 0; END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- DELETE
CREATE PROCEDURE [dbo].[usp_Master_PartyAccount_Delete] (@IDPartyAccount INT, @DeletedBy NVARCHAR(100))
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE dbo.Master_PartyAccount SET IsDeleted = 1, D_Date = GETDATE(), D_By = @DeletedBy WHERE IDPartyAccount = @IDPartyAccount AND IsDeleted = 0;
        SELECT Result = 1, Message = 'Party Account deleted successfully', NewId = @IDPartyAccount, ErrorCode = '';
    END TRY
    BEGIN CATCH SELECT Result = -1, Message = ERROR_MESSAGE(), NewId = NULL, ErrorCode = 'SQL_ERROR'; END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SAVE
CREATE PROCEDURE [dbo].[usp_Master_PartyAccount_Save]
(
    @IDPartyAccount INT = 0,
    @AccountSrNo    INT = NULL,
    @PartyCode      NVARCHAR(50) = NULL,
    @IDAccountType  INT = NULL,
    @PartyName      NVARCHAR(100),
    @Address        NVARCHAR(255) = NULL,
    @IDState        INT = NULL,
    @IDCity         INT = NULL,
    @ContactNo1     NVARCHAR(15),
    @ContactNo2     NVARCHAR(15) = NULL,
    @Email          NVARCHAR(50) = NULL,
    @OpeningBalance DECIMAL(18,2) = 0,
    @IDBalanceType  INT = NULL,
    @GSTNo          NVARCHAR(100) = NULL,
    @PanNo          NVARCHAR(100) = NULL,
    @UserName       NVARCHAR(100)
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM dbo.Master_PartyAccount WHERE PartyName = @PartyName AND IsDeleted = 0 AND IDPartyAccount <> @IDPartyAccount)
        BEGIN SELECT Result = -1, Message = 'Party Name already exists', NewId = NULL, ErrorCode = 'DUPLICATE'; RETURN; END

        IF @IDPartyAccount = 0
        BEGIN
            INSERT INTO dbo.Master_PartyAccount (AccountSrNo, PartyCode, IDAccountType, PartyName, Address, IDState, IDCity, ContactNo1, ContactNo2, Email, OpeningBalance, IDBalanceType, GSTNo, PanNo, E_Date, E_By, IsDeleted)
            VALUES (@AccountSrNo, @PartyCode, @IDAccountType, @PartyName, @Address, @IDState, @IDCity, @ContactNo1, @ContactNo2, @Email, @OpeningBalance, @IDBalanceType, @GSTNo, @PanNo, GETDATE(), @UserName, 0);
            
            SELECT Result = 1, Message = 'Party Account created successfully', NewId = SCOPE_IDENTITY(), ErrorCode = '';
        END
        ELSE
        BEGIN
            UPDATE dbo.Master_PartyAccount SET AccountSrNo = @AccountSrNo, PartyCode = @PartyCode, IDAccountType = @IDAccountType, PartyName = @PartyName, Address = @Address, IDState = @IDState, IDCity = @IDCity, ContactNo1 = @ContactNo1, ContactNo2 = @ContactNo2, Email = @Email, OpeningBalance = @OpeningBalance, IDBalanceType = @IDBalanceType, GSTNo = @GSTNo, PanNo = @PanNo, U_Date = GETDATE(), U_By = @UserName
            WHERE IDPartyAccount = @IDPartyAccount AND IsDeleted = 0;
            
            SELECT Result = 1, Message = 'Party Account updated successfully', NewId = @IDPartyAccount, ErrorCode = '';
        END
    END TRY
    BEGIN CATCH SELECT Result = -1, Message = ERROR_MESSAGE(), NewId = NULL, ErrorCode = 'SQL_ERROR'; END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SELECT
CREATE PROCEDURE [dbo].[usp_Master_PartyAccount_Select] 
AS BEGIN 
    SET NOCOUNT ON; 
    SELECT p.*, c.City AS CityName 
    FROM dbo.Master_PartyAccount p 
    LEFT JOIN dbo.Master_City c ON p.IDCity = c.IDCity 
    WHERE p.IsDeleted = 0 
    ORDER BY p.PartyName; 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SELECT BY ID
CREATE PROCEDURE [dbo].[usp_Master_PartyAccount_SelectById] (@IDPartyAccount INT) AS BEGIN SET NOCOUNT ON; SELECT * FROM dbo.Master_PartyAccount WHERE IDPartyAccount = @IDPartyAccount AND IsDeleted = 0; END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- DELETE
CREATE PROCEDURE [dbo].[usp_Master_State_Delete] (@IDState INT, @DeletedBy NVARCHAR(255))
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE dbo.Master_State SET IsDeleted = 1, D_Date = GETDATE(), D_By = @DeletedBy WHERE IDState = @IDState AND IsDeleted = 0;
        SELECT Result = 1, Message = 'State deleted successfully', NewId = @IDState, ErrorCode = '';
    END TRY
    BEGIN CATCH SELECT Result = -1, Message = ERROR_MESSAGE(), NewId = NULL, ErrorCode = 'SQL_ERROR'; END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- SAVE
CREATE PROCEDURE [dbo].[usp_Master_State_Save]
(
    @IDState   INT = 0,
    @IDCountry INT,
    @State     NVARCHAR(255),
    @UserName  NVARCHAR(255)
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        -- Duplicate Check
        IF EXISTS (SELECT 1 FROM dbo.Master_State WHERE State = @State AND IDCountry = @IDCountry AND IsDeleted = 0 AND IDState <> @IDState)
        BEGIN
            SELECT Result = -1, Message = 'State name already exists for this country', NewId = NULL, ErrorCode = 'DUPLICATE';
            RETURN;
        END

        IF @IDState = 0
        BEGIN
            INSERT INTO dbo.Master_State (IDCountry, State, E_Date, E_By, IsDeleted)
            VALUES (@IDCountry, @State, GETDATE(), @UserName, 0);

            SELECT Result = 1, Message = 'State created successfully', NewId = SCOPE_IDENTITY(), ErrorCode = '';
        END
        ELSE
        BEGIN
            UPDATE dbo.Master_State SET IDCountry = @IDCountry, State = @State, U_Date = GETDATE(), U_By = @UserName
            WHERE IDState = @IDState AND IsDeleted = 0;

            SELECT Result = 1, Message = 'State updated successfully', NewId = @IDState, ErrorCode = '';
        END
    END TRY
    BEGIN CATCH
        SELECT Result = -1, Message = ERROR_MESSAGE(), NewId = NULL, ErrorCode = 'SQL_ERROR';
    END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SELECT
CREATE PROCEDURE [dbo].[usp_Master_State_Select]
AS BEGIN SET NOCOUNT ON; SELECT s.*, c.Country AS CountryName FROM dbo.Master_State s LEFT JOIN dbo.Master_Country c ON s.IDCountry = c.IDCountry WHERE s.IsDeleted = 0 ORDER BY s.State; END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SELECT BY ID
CREATE PROCEDURE [dbo].[usp_Master_State_SelectById] (@IDState INT)
AS BEGIN SET NOCOUNT ON; SELECT * FROM dbo.Master_State WHERE IDState = @IDState AND IsDeleted = 0; END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- DELETE
CREATE PROCEDURE [dbo].[usp_Master_Truck_Delete] (@IDTruck INT, @DeletedBy NVARCHAR(100))
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE dbo.Master_Truck SET IsDeleted = 1, D_Date = GETDATE(), D_By = @DeletedBy WHERE IDTruck = @IDTruck AND IsDeleted = 0;
        SELECT Result = 1, Message = 'Truck deleted successfully', NewId = @IDTruck, ErrorCode = '';
    END TRY
    BEGIN CATCH SELECT Result = -1, Message = ERROR_MESSAGE(), NewId = NULL, ErrorCode = 'SQL_ERROR'; END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- SAVE
CREATE PROCEDURE [dbo].[usp_Master_Truck_Save]
(
    @IDTruck       INT = 0,
    @TruckNumber   NVARCHAR(20),
    @PanCardHolder NVARCHAR(100) = NULL,
    @PanCardNo     NVARCHAR(20) = NULL,
    @Remarks       NVARCHAR(MAX) = NULL,
    @UserName      NVARCHAR(100)
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM dbo.Master_Truck WHERE TruckNumber = @TruckNumber AND IsDeleted = 0 AND IDTruck <> @IDTruck)
        BEGIN SELECT Result = -1, Message = 'Truck Number already exists', NewId = NULL, ErrorCode = 'DUPLICATE'; RETURN; END

        IF @IDTruck = 0
        BEGIN
            INSERT INTO dbo.Master_Truck (TruckNumber, PanCardHolder, PanCardNo, Remarks, E_Date, E_By, IsDeleted) 
            VALUES (@TruckNumber, @PanCardHolder, @PanCardNo, @Remarks, GETDATE(), @UserName, 0);
            SELECT Result = 1, Message = 'Truck created successfully', NewId = SCOPE_IDENTITY(), ErrorCode = '';
        END
        ELSE
        BEGIN
            UPDATE dbo.Master_Truck SET TruckNumber = @TruckNumber, PanCardHolder = @PanCardHolder, PanCardNo = @PanCardNo, Remarks = @Remarks, U_Date = GETDATE(), U_By = @UserName 
            WHERE IDTruck = @IDTruck AND IsDeleted = 0;
            SELECT Result = 1, Message = 'Truck updated successfully', NewId = @IDTruck, ErrorCode = '';
        END
    END TRY
    BEGIN CATCH SELECT Result = -1, Message = ERROR_MESSAGE(), NewId = NULL, ErrorCode = 'SQL_ERROR'; END CATCH
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SELECT
CREATE PROCEDURE [dbo].[usp_Master_Truck_Select] AS BEGIN SET NOCOUNT ON; SELECT * FROM dbo.Master_Truck WHERE IsDeleted = 0 ORDER BY TruckNumber; END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SELECT BY ID
CREATE PROCEDURE [dbo].[usp_Master_Truck_SelectById] (@IDTruck INT) AS BEGIN SET NOCOUNT ON; SELECT * FROM dbo.Master_Truck WHERE IDTruck = @IDTruck AND IsDeleted = 0; END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create   PROCEDURE [dbo].[usp_Master_User_Login]
(
    @UserName NVARCHAR(50),
    @Password NVARCHAR(100)
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        *
    FROM dbo.Master_User 
    WHERE UserName = @UserName
      AND Password = @Password
      AND IsDeleted = 0;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[usp_Master_User_Save]
(
    @IDUser    INT = 0,
    @UserType  NVARCHAR(30),
    @UserName  NVARCHAR(50),
    @Password  NVARCHAR(100),
    @FullName  NVARCHAR(150),
    @Email     NVARCHAR(150) = NULL,
    @ContactNo NVARCHAR(15)  = NULL,
    @ActionBy  NVARCHAR(100),
	@ActionById INT = 0
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        
        IF EXISTS (
            SELECT 1
            FROM dbo.Master_User
            WHERE UserName = @UserName
              AND IsDeleted = 0
              AND IDUser <> @IDUser
        )
        BEGIN
            SELECT
                Result = -1,
                Message = 'Username already exists',
                NewId = NULL,
                ErrorCode = 'DUPLICATE';
            RETURN;
        END

       
        IF @IDUser = 0
        BEGIN
            INSERT INTO dbo.Master_User
            (
                UserName,
                Password,
                FullName,
                Email,
                ContactNo,
                UserType,
                E_Date,
                E_By,
				E_ById,
                IsDeleted
            )
            VALUES
            (
                @UserName,
                @Password,
                @FullName,
                @Email,
                @ContactNo,
                @UserType,
                GETDATE(),
                @ActionBy,
				@ActionById,
                0
            );

            SELECT
                Result = 1,
                Message = 'User created successfully',
                NewId = SCOPE_IDENTITY(),
                ErrorCode = '';
            RETURN;
        END

        
        UPDATE dbo.Master_User
        SET
            UserName = @UserName,
            Password = @Password,
            FullName = @FullName,
            Email    = @Email,
            ContactNo = @ContactNo,
            UserType = @UserType,
            U_Date   = GETDATE(),
            U_By     = @ActionBy
        WHERE IDUser = @IDUser
          AND IsDeleted = 0;

        IF @@ROWCOUNT = 0
        BEGIN
            SELECT
                Result = -1,
                Message = 'User not found or deleted',
                NewId = NULL,
                ErrorCode = 'NOT_FOUND';
            RETURN;
        END

        SELECT
            Result = 1,
            Message = 'User updated successfully',
            NewId = @IDUser,
            ErrorCode = '';

    END TRY
    BEGIN CATCH
        SELECT
            Result = -1,
            Message = ERROR_MESSAGE(),
            NewId = NULL,
            ErrorCode = 'SQL_ERROR';
    END CATCH
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create   PROCEDURE [dbo].[usp_Master_User_Select]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IDUser,
        UserName,
        FullName,
        Email,
        ContactNo,
        UserType
    FROM dbo.Master_User
    WHERE IsDeleted = 0
    ORDER BY FullName;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create   PROCEDURE [dbo].[usp_Master_User_SelectById]
(
    @IDUser INT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
       *
    FROM dbo.Master_User
    WHERE IDUser = @IDUser
      AND IsDeleted = 0;
END
GO
USE [master]
GO
ALTER DATABASE [dbTMS] SET  READ_WRITE 
GO

    */