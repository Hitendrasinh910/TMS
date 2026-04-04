/*
CREATE PROCEDURE[dbo].[usp_Master_PartyAccount_SelectSrNo]
AS
BEGIN
    SET NOCOUNT ON;

SELECT ISNULL(MAX(AccountSrNo), 0) +1
    FROM Master_PartyAccount; --Ensure this matches your actual table name
END
*/

/*
Create PROCEDURE[dbo].[usp_Master_BillToParty_SelectSrNo]
AS
BEGIN
    SET NOCOUNT ON;

SELECT ISNULL(MAX(SrNo), 0) +1
    FROM[dbo].[Master_BillToParty]; --Ensure this matches your actual table name
END
*/

/*
Create PROCEDURE[dbo].[usp_Transaction_Bill_SelectBillNo]
AS
BEGIN
    SET NOCOUNT ON;

	SELECT ISNULL(MAX(IDBill), 0) +1
    FROM[dbo].[Transaction_Bill] --Ensure this matches your actual table name
END
*/

/*
Create PROCEDURE[dbo].[usp_Transaction_LR_SelectLrNo]
AS
BEGIN
    SET NOCOUNT ON;

	SELECT ISNULL(MAX(IDLR), 0) +1
    FROM[dbo].[Transaction_LR] --Ensure this matches your actual table name
END
*/

/*
Create PROCEDURE[dbo].[usp_Transaction_PaymentReceive_SelectReceiptNo]
AS
BEGIN
    SET NOCOUNT ON;

	SELECT ISNULL(MAX(IDPayment), 0) +1
    FROM[dbo].[Transaction_PaymentReceive] --Ensure this matches your actual table name
END
*/

/*
CREATE TABLE [dbo].[Transaction_PaymentType](
	[IDPaymentType] [int] IDENTITY(1,1) NOT NULL,
	[PaymentType] [nvarchar](255) NULL,
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
	[IDPaymentType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Transaction_PaymentType] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO


// 
CREATE TABLE [dbo].[Transaction_PaymentMode](
	[IDPaymentMode] [int] IDENTITY(1,1) NOT NULL,
	[PaymentMode] [nvarchar](255) NULL,
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
	[IDPaymentMode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Transaction_PaymentMode] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO

// SP
Create   PROCEDURE [dbo].[usp_Transaction_PaymentMode_Select]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IDPaymentMode,
        PaymentMode,
        E_Date,
        E_By
    FROM dbo.Transaction_PaymentMode
    WHERE IsDeleted = 0
    ORDER BY PaymentMode;
END

// 
Create   PROCEDURE [dbo].[usp_Transaction_PaymentType_Select]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IDPaymentType,
        PaymentType,
        E_Date,
        E_By
    FROM dbo.Transaction_PaymentType
    WHERE IsDeleted = 0
    ORDER BY PaymentType;
END

*/

/*
CREATE TABLE[dbo].[Extra_UserRights] (
    [IDUserRights][int] IDENTITY(1, 1) NOT NULL,

    [IDUser] [int] NULL,
	[IDForms][int] NULL,
	[AllowToView][bit] NULL,
	[AllowToAdd][bit] NULL,
	[AllowToUpdate][bit] NULL,
	[AllowToDelete][bit] NULL,
	[E_Date][datetime] NULL,
	[E_By][nvarchar](255) NULL,
    [E_ById] [int] NULL,
	[U_Date][datetime] NULL,
	[U_ById][int] NULL,
 CONSTRAINT[PK_Extra_UserRights] PRIMARY KEY CLUSTERED 
(

    [IDUserRights] ASC
)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON[PRIMARY]
) ON[PRIMARY]
GO

CREATE TABLE [dbo].[Extra_UserForms](
	[IDUserForms] [int] IDENTITY(1,1) NOT NULL,
	[FormName] [varchar](150) NULL,
 CONSTRAINT [PK_Extra_UserForms] PRIMARY KEY CLUSTERED 
(
	[IDUserForms] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

-- 1. Create the TVP for saving multiple rights at once
CREATE TYPE [dbo].[udt_UserRights] AS TABLE(
    [IDForms] INT,
    [AllowToView] BIT,
    [AllowToAdd] BIT,
    [AllowToUpdate] BIT,
    [AllowToDelete] BIT
)
GO

-- 2. SP to GET Rights (Returns ALL forms, and checks the boxes if the user has rights)
CREATE PROCEDURE [dbo].[usp_Extra_UserRights_GetByUser]
(
    @IDUser INT
)
AS
BEGIN
    SET NOCOUNT ON;

    -- LEFT JOIN ensures we see all forms, even if the user has no rights assigned yet
    SELECT 
        f.IDUserForms AS IDForms,
        f.FormName,
        ISNULL(r.AllowToView, 0) AS AllowToView,
        ISNULL(r.AllowToAdd, 0) AS AllowToAdd,
        ISNULL(r.AllowToUpdate, 0) AS AllowToUpdate,
        ISNULL(r.AllowToDelete, 0) AS AllowToDelete
    FROM dbo.Extra_UserForms f
    LEFT JOIN dbo.Extra_UserRights r 
        ON f.IDUserForms = r.IDForms AND r.IDUser = @IDUser;
END
GO

-- 3. SP to SAVE Rights
CREATE PROCEDURE [dbo].[usp_Extra_UserRights_Save]
(
    @IDUser INT,
    @UserName NVARCHAR(255),
    @Rights dbo.udt_UserRights READONLY
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        
        -- 1. Wipe existing rights for this user
        DELETE FROM dbo.Extra_UserRights WHERE IDUser = @IDUser;

        -- 2. Insert new rights from the TVP
        INSERT INTO dbo.Extra_UserRights (
            IDUser, IDForms, AllowToView, AllowToAdd, AllowToUpdate, AllowToDelete, E_Date, E_By
        )
        SELECT 
            @IDUser, IDForms, AllowToView, AllowToAdd, AllowToUpdate, AllowToDelete, GETDATE(), @UserName
        FROM @Rights;

        COMMIT TRANSACTION;
        SELECT Result = 1, Message = 'User rights updated successfully', NewId = @IDUser, ErrorCode = '';

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SELECT Result = -1, Message = ERROR_MESSAGE(), NewId = NULL, ErrorCode = 'SQL_ERROR';
    END CATCH
END
GO
*/