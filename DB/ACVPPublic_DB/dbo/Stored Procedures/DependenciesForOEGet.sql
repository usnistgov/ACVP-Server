CREATE PROCEDURE [dbo].[DependenciesForOEGet]

    @OEId BIGINT
	
AS

SET NOCOUNT ON

SELECT DependencyId
FROM dbo.OEDependencies
WHERE OEId = @OEId