CREATE PROCEDURE [acvp].[AcvpUserSeedGetByCertificate]
    @Subject NVARCHAR(2048)
	
AS

SELECT seed
FROM [acvp].[ACVP_USER] au
WHERE au.common_name = @Subject