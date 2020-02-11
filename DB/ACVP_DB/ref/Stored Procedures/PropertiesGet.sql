CREATE PROCEDURE [ref].[PropertiesGet]

AS

SET NOCOUNT ON

SELECT	 id AS PropertyId
		,algorithm_id AS AlgorithmId
		,[name] AS [Name]
		,order_index AS OrderIndex
FROM ref.CRYPTO_ALGORITHM_PROPERTY
