-- =============================================
-- Author:		Russ Hammett
-- Create date: 2019-02-11
-- Description:	Adds a log entry to the pool logs table
-- =============================================
CREATE PROCEDURE [dbo].[AddPoolLogEntry]
	@logTypeName VARCHAR(256),
	@poolName VARCHAR(512),
	@dateStart DATETIME2(7),
	@dateEnd DATETIME2(7) = NULL,
	@msg VARCHAR(MAX) = NULL
AS
BEGIN
	
	SET NOCOUNT ON;

    DECLARE @logTypeId BIGINT
	EXEC [dbo].[GetPoolLogTypeIdFromName] @logTypeName, @logTypeId OUTPUT

	DECLARE @poolId BIGINT
	EXEC [dbo].[GetPoolIdFromName] @poolName, @poolId OUTPUT

	INSERT INTO [dbo].[PoolLogs] (logType, poolId, dateStart, dateEnd, msg)
	VALUES (@logTypeId, @poolId, @dateStart, @dateEnd, @msg)

END


