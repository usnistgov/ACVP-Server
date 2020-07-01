CREATE PROCEDURE [dbo].[DependencyDelete]

	@DependencyId bigint

AS

SET NOCOUNT ON

DELETE 
FROM dbo.Dependencies
WHERE DependencyId = @DependencyId