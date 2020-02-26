CREATE PROCEDURE [acvp].[PreviousComputedWindowByUserGet]
    @CertificateRawData VARBINARY(MAX)
	
AS

DECLARE @UserId BIGINT

SELECT TOP(1) @UserId = id
FROM [acvp].[ACVP_USER] au
WHERE au.certificate = @CertificateRawData

SELECT TOP(1) (LastUsedWindow)
FROM [acvp].[AcvpUserAuthentications] auAuths
WHERE auAuths.AcvpUserID = @UserId