CREATE PROCEDURE [dbo].[PreviousComputedWindowByUserGet]
    
	@Subject NVARCHAR(2048)
	
AS

SET NOCOUNT ON

SELECT TOP 1 A.LastUsedWindow
FROM dbo.ACVPUsers U
	INNER JOIN
	dbo.ACVPUserAuthentications A ON A.ACVPUserId = U.ACVPUserId
								  AND U.CommonName = @Subject
