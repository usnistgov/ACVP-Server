CREATE PROCEDURE [ref].[AlgorithmsGet]

AS

SET NOCOUNT ON
-- filter out historical
-- do not need display name, or alias, or historical
SELECT 	 id AS AlgorithmId
		,[name] AS [Name]
		,mode AS Mode
		,revision AS Revision
FROM ref.CRYPTO_ALGORITHM
WHERE historical = 0