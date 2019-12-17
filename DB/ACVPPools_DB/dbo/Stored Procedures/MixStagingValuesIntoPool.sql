-- =============================================
-- Author:		Russ Hammett
-- Create date: 2019-02-06
-- Description:	Moves the values from the specified staging table to the "prod" table
-- =============================================
CREATE PROCEDURE [dbo].[MixStagingValuesIntoPool]
	@poolName VARCHAR(512)
AS
BEGIN
	
	SET NOCOUNT ON;

	DECLARE @stagingTable TABLE 
	(
		dateCreated DATETIME2(7),
		dateLastUsed DATETIME2(7),
		timesUsed BIGINT,
		value VARCHAR(MAX)
	)

    DECLARE @poolId BIGINT
	EXEC [dbo].[GetPoolIdFromName] @poolName, @poolId OUTPUT

	DELETE
	FROM	[dbo].[PoolValues]
	OUTPUT	DELETED.dateCreated, DELETED.dateLastUsed, DELETED.timesUsed, DELETED.value INTO @stagingTable
	WHERE	poolId = @poolId
		AND isStagingValue = 1

	INSERT	INTO [dbo].[PoolValues] (poolId, isStagingValue, dateCreated, dateLastUsed, timesUsed, value)
	SELECT	@poolId, 0, dateCreated, dateLastUsed, timesUsed, value
	FROM	@stagingTable
	ORDER	BY NEWID()

END

