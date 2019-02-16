-- =============================================
-- Author:		Russ Hammett
-- Create date: 2019-02-06
-- Description:	Gets the pool ID for the specified name, if it doesn't exist, creates it.
-- =============================================
CREATE PROCEDURE [dbo].[GetPoolIdFromName]
	@poolName VARCHAR(512),
	@poolId BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT @poolId = id FROM PoolInformation (NOLOCK) WHERE poolName = @poolName
	IF @poolId IS NULL
	BEGIN
		SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
		BEGIN TRANSACTION
			SELECT @poolId = id FROM PoolInformation (NOLOCK) WHERE poolName = @poolName
			IF @poolId IS NULL
			BEGIN
			   INSERT INTO PoolInformation (poolName) VALUES (@poolName)
			   SELECT @poolId = SCOPE_IDENTITY()
			END
		COMMIT TRANSACTION
	END

END
