CREATE PROCEDURE [dbo].[OEDependencyForDependencyDeleteAll]

	@DependencyId bigint

AS

SET NOCOUNT ON

DELETE 
FROM dbo.OEDependencies
WHERE DependencyId = @DependencyId