-- =============================================
-- Author:		Russ Hammett
-- Create date: 2020-02-03
-- Description:	Get all pool counts for initial app bootstrapping.
-- =============================================
CREATE PROCEDURE [dbo].[GetPoolCounts]
	
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT	poolId, poolName, poolCount
	FROM	(
		SELECT	poolId, COUNT_BIG(1) as poolCount
		FROM	[dbo].[PoolValues] pv
		WHERE	pv.isStagingValue = 0
		GROUP	BY pv.poolId
	) poolCounts
	INNER	JOIN [dbo].[PoolInformation] poolInfo on poolCounts.poolId = poolInfo.id

END