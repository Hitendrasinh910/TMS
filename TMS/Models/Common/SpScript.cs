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