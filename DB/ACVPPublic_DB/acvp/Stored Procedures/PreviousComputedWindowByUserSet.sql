CREATE PROCEDURE [acvp].[PreviousComputedWindowByUserSet]
    @Subject NVARCHAR(2048),
    @LastUsedWindow BIGINT
	
AS

DECLARE @UserId BIGINT

SELECT TOP(1) @UserId = id
FROM [acvp].[ACVP_USER] au
WHERE au.common_name = @Subject

MERGE [acvp].[AcvpUserAuthentications] AS target
USING (SELECT @UserId) as source (AcvpUserID)
ON target.AcvpUserID = source.AcvpUserID
WHEN matched THEN
	UPDATE SET LastUsedWindow = @LastUsedWindow
WHEN not matched THEN
	INSERT (AcvpUserID, LastUsedWindow)
	VALUES (@UserId, @LastUsedWindow)
;