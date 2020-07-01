CREATE PROCEDURE [dbo].[OEDependenciesGet]

	@OEId bigint

AS

SET NOCOUNT ON

SELECT DependencyId
FROM dbo.OEDependencies
WHERE OEId = @OEId