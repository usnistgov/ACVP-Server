CREATE PROCEDURE [dbo].[DependencyAttributesGet]

	@DependencyId bigint

AS

SET NOCOUNT ON

SELECT	 DependencyAttributeId
		,[Name]
		,[Value]
FROM dbo.DependencyAttributes
WHERE DependencyId = @DependencyId