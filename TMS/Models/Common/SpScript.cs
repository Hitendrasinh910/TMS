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