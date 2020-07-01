CREATE PROCEDURE [dbo].[DependencyAttributeDeleteAll]

	@DependencyId bigint

AS

SET NOCOUNT ON

DELETE 
FROM dbo.DependencyAttributes
WHERE DependencyId = @DependencyId