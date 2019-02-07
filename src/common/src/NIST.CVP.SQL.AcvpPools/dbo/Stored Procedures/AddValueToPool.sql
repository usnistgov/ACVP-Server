-- =============================================
-- Author:		Russ Hammett
-- Create date: 2019-02-06
-- Description:	Adds a value to the specified pool
-- =============================================
CREATE PROCEDURE [dbo].[AddValueToPool]
	@poolName VARCHAR(512),
	@value VARCHAR(MAX),
	@isStagingValue BIT = 0,
	@dateCreated DATETIME2(7) = NULL,
	@dateLastUsed DATETIME2(7) = NULL,
	@timesValueUsed BIGINT = 0
AS
BEGIN
	
	SET NOCOUNT ON;

    DECLARE @poolId BIGINT
	EXEC [dbo].[GetPoolIdFromName] @poolName, @poolId OUTPUT

	IF @dateCreated IS NULL
	BEGIN
		SET @dateCreated = getdate()
	END

	INSERT INTO PoolValues (poolId, isStagingValue, dateCreated, dateLastUsed, timesUsed, value) 
	VALUES (@poolId, @isStagingValue, @dateCreated, @dateLastUsed, @timesValueUsed, @value)

END
