CREATE PROCEDURE [dbo].[AcvpUserSeedGetByCertificate]

    @Subject NVARCHAR(2048)
	
AS

SET NOCOUNT ON

SELECT Seed
FROM dbo.ACVPUsers
WHERE CommonName = @Subject