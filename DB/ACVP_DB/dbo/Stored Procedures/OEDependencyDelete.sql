CREATE PROCEDURE [dbo].[OEDependencyDelete]

	 @DependencyId bigint
	,@OEId bigint

AS

SET NOCOUNT ON

DELETE 
FROM dbo.OEDependencies
WHERE DependencyId = @DependencyId
  AND OEId = @OEId