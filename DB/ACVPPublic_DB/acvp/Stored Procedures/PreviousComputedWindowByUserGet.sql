CREATE PROCEDURE [acvp].[PreviousComputedWindowByUserGet]
    @Subject NVARCHAR(2048)
	
AS

SET NOCOUNT ON

SELECT TOP 1 A.LastUsedWindow
from acvp.ACVP_USER U
	INNER JOIN
	acvp.AcvpUserAuthentications A ON A.AcvpUserID = U.id
								  AND U.common_name = @Subject