CREATE PROCEDURE [dbo].[DependencyAttributeDelete]

	@DependencyAttributeId bigint

AS

SET NOCOUNT ON

DELETE 
FROM dbo.DependencyAttributes
WHERE DependencyAttributeId = @DependencyAttributeId