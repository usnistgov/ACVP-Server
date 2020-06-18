CREATE PROCEDURE [dbo].[AlgorithmsGet]

AS

SET NOCOUNT ON

SELECT	 AlgorithmId
		,[Name]
		,Mode
		,Revision
		,DisplayName
		,Alias
		,Historical
FROM dbo.Algorithms
