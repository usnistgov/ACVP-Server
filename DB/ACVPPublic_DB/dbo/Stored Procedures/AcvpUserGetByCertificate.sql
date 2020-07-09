CREATE PROCEDURE [dbo].[AcvpUserGetByCertificate]
    
	@Subject NVARCHAR(2048)
	
AS

SET NOCOUNT ON

SELECT TOP 1 ACVPUserId
FROM dbo.ACVPUsers
WHERE CommonName = @Subject