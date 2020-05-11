CREATE PROCEDURE [acvp].[AcvpUserGetByCertificate]
    @Subject NVARCHAR(2048)
	
AS

SELECT id AS UserID
FROM [acvp].[ACVP_USER] au
WHERE au.common_name = @Subject