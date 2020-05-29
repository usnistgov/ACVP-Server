CREATE PROCEDURE [acvp].[PreviousComputedWindowByUserGet]
    @Subject NVARCHAR(2048)
	
AS

DECLARE @UserId BIGINT

SELECT TOP(1) @UserId = id
FROM [acvp].[ACVP_USER] au
WHERE au.common_name = @Subject

SELECT TOP(1) (LastUsedWindow)
FROM [acvp].[AcvpUserAuthentications] auAuths
WHERE auAuths.AcvpUserID = @UserId