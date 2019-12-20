-- =============================================
-- Author:		Russ Hammett
-- Create date: 2019-02-06
-- Description:	Gets the count for a specific pool name
-- =============================================
CREATE PROCEDURE [dbo].[GetPoolCount]
	@poolName varchar(512),
	@isStagingValue BIT = 0
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @poolId BIGINT
	EXEC [dbo].[GetPoolIdFromName] @poolName, @poolId OUTPUT

	SELECT	COUNT_BIG(1) as poolCount
	FROM	[dbo].[PoolValues]
	WHERE	poolId = @poolId
		AND	isStagingValue = @isStagingValue

END

