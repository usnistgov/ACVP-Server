-- =============================================
-- Author:		Russ Hammett
-- Create date: 2019-02-06
-- Description:	Gets a value from the specified pool
-- =============================================
CREATE PROCEDURE [dbo].[GetPoolValue]
	@poolName VARCHAR(512)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @poolId BIGINT
	EXEC [dbo].[GetPoolIdFromName] @poolName, @poolId OUTPUT

    DELETE TOP(1) [dbo].[PoolValues] WITH (READPAST)
	OUTPUT deleted.*  
	WHERE poolId = @poolId
		and isStagingValue = 0

END

