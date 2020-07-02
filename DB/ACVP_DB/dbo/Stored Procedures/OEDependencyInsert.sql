CREATE PROCEDURE [dbo].[OEDependencyInsert]

	 @OEId bigint
	,@DependencyId bigint

AS

SET NOCOUNT ON

INSERT INTO dbo.OEDependencies (OEId, DependencyId)
VALUES (@OEId, @DependencyId)