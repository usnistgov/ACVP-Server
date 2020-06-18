CREATE PROCEDURE [dbo].[PropertiesGet]

AS

SET NOCOUNT ON

SELECT	 AlgorithmPropertyId
		,AlgorithmId
		,PropertyName
		,OrderIndex
FROM dbo.AlgorithmProperties
