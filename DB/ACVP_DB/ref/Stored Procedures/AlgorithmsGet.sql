CREATE PROCEDURE [ref].[AlgorithmsGet]

AS

SET NOCOUNT ON

SELECT	 id AS AlgorithmId
		,[name] AS [Name]
		,mode AS Mode
		,revision AS Revision
		,display_name AS DisplayName
		,alias AS Alias
		,historical AS Historical
FROM ref.CRYPTO_ALGORITHM
