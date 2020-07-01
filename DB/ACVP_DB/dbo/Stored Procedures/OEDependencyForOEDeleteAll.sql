CREATE PROCEDURE [dbo].[OEDependencyForOEDeleteAll]

	@OEId bigint

AS

SET NOCOUNT ON

DELETE 
FROM dbo.OEDependencies
WHERE OEId = @OEId