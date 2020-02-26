CREATE PROCEDURE [acvp].[AcvpUserGetByCertificate]
    @CertificateRawData VARBINARY(MAX)
	
AS

SELECT TOP(1) (seed)
FROM [acvp].[ACVP_USER] au
WHERE au.certificate = @CertificateRawData