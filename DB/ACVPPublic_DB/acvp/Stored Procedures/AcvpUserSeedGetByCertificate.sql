CREATE PROCEDURE [acvp].[AcvpUserSeedGetByCertificate]
    @CertificateRawData VARBINARY(MAX)
	
AS

SELECT (seed)
FROM [acvp].[ACVP_USER] au
WHERE au.certificate = @CertificateRawData