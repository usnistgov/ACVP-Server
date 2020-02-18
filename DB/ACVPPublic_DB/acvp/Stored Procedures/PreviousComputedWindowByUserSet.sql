CREATE PROCEDURE [acvp].[PreviousComputedWindowByUserSet]
    @CertificateRawData VARBINARY(MAX),
    @LastUsedWindow BIGINT
	
AS

DECLARE @UserId BIGINT

SELECT TOP(1) @UserId = id
FROM [acvp].[ACVP_USER] au
WHERE au.certificate = @CertificateRawData

UPDATE [acvp].[AcvpUserAuthentications]
SET LastUsedWindow = @LastUsedWindow
WHERE AcvpUserID = @UserId

IF @@ROWCOUNT = 0
    INSERT INTO [acvp].[AcvpUserAuthentications] (AcvpUserId, LastUsedWindow)
    VALUES (@UserId, @LastUsedWindow)