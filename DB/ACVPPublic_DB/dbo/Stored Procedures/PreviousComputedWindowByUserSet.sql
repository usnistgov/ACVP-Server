CREATE PROCEDURE [dbo].[PreviousComputedWindowByUserSet]
    
	 @Subject NVARCHAR(2048)
    ,@LastUsedWindow BIGINT
	
AS

SET NOCOUNT ON

DECLARE @UserId BIGINT

SELECT TOP(1) @UserId = ACVPUserId
FROM dbo.ACVPUsers
WHERE CommonName = @Subject

MERGE dbo.ACVPUserAuthentications AS target
USING (SELECT @UserId) as source (ACVPUserId)
ON target.ACVPUserId = source.ACVPUserId
WHEN matched THEN
	UPDATE SET LastUsedWindow = @LastUsedWindow
WHEN not matched THEN
	INSERT (ACVPUserId, LastUsedWindow)
	VALUES (@UserId, @LastUsedWindow)
;
