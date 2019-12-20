-- =============================================
-- Author:		Russ Hammett
-- Create date: 2019-02-11
-- Description:	Gets the pool log type ID for the specified name, if it doesn't exist, creates it.
-- =============================================
CREATE PROCEDURE [dbo].[GetPoolLogTypeIdFromName]
	@poolLogTypeName VARCHAR(256),
	@poolLogTypeId BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT @poolLogTypeId = id FROM PoolInformation (NOLOCK) WHERE poolName = @poolLogTypeName
	IF @poolLogTypeId IS NULL
	BEGIN
		SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
		BEGIN TRANSACTION
			SELECT @poolLogTypeId = id FROM PoolLogTypes (NOLOCK) WHERE logTypeDescription = @poolLogTypeName
			IF @poolLogTypeId IS NULL
			BEGIN
			   INSERT INTO PoolLogTypes (logTypeDescription) VALUES (@poolLogTypeName)
			   SELECT @poolLogTypeId = SCOPE_IDENTITY()
			END
		COMMIT TRANSACTION
	END

END


