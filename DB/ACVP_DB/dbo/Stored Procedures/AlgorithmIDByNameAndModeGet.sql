CREATE PROCEDURE [dbo].[AlgorithmIDByNameAndModeGet]
	
	 @Name nvarchar(128)
	,@Mode nvarchar(128)

AS

SET NOCOUNT ON

SELECT AlgorithmId
FROM dbo.Algorithms
WHERE [Name] = @Name
  AND Mode = @Mode
