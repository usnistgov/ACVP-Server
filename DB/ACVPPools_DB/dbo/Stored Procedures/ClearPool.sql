-- =============================================
-- Author:		Russ Hammett
-- Create date: 2019-02-06
-- Description:	Clears a pool by name
-- =============================================
CREATE PROCEDURE [dbo].[ClearPool]
	@poolName VARCHAR(512)
AS
BEGIN
	
	SET NOCOUNT ON;

    DECLARE @poolId BIGINT
	EXEC [dbo].[GetPoolIdFromName] @poolName, @poolId OUTPUT

	DELETE
	FROM	[dbo].[PoolValues]
	WHERE	poolId = @poolId

END

