CREATE PROCEDURE [acvp].[AcvpUserGetByCertificate]
    @CertificateRawData VARBINARY(MAX)
	
AS

SELECT id AS UserID
FROM [acvp].[ACVP_USER] au
WHERE au.certificate = @CertificateRawData