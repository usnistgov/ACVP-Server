CREATE PROCEDURE [acvp].[PreviousComputedWindowByUserSet]
    @Subject NVARCHAR(2048),
    @LastUsedWindow BIGINT
	
AS

DECLARE @UserId BIGINT

SELECT TOP(1) @UserId = id
FROM [acvp].[ACVP_USER] au
WHERE au.common_name = @Subject

UPDATE [acvp].[AcvpUserAuthentications]
SET LastUsedWindow = @LastUsedWindow
WHERE AcvpUserID = @UserId

IF @@ROWCOUNT = 0
    INSERT INTO [acvp].[AcvpUserAuthentications] (AcvpUserId, LastUsedWindow)
    VALUES (@UserId, @LastUsedWindow)