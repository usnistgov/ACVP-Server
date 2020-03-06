CREATE PROCEDURE [ref].[AlgorithmIDByNameAndModeGet]
	
	 @Name nvarchar(128)
	,@Mode nvarchar(128)

AS

SET NOCOUNT ON

SELECT id AS AlgorithmId
FROM ref.CRYPTO_ALGORITHM
WHERE [name] = @Name
  AND mode = @Mode
