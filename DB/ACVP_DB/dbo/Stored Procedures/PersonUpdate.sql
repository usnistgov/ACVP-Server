CREATE PROCEDURE [dbo].[PersonUpdate]

	 @PersonId bigint
	,@Name nvarchar(1024)
	,@OrganizationId bigint
	,@NameUpdated bit
	,@OrganizationIdUpdated bit

AS

SET NOCOUNT ON

UPDATE dbo.People
SET	 FullName = CASE @NameUpdated WHEN 1 THEN @Name ELSE FullName END
	,OrganizationId = CASE @OrganizationIdUpdated WHEN 1 THEN @OrganizationId ELSE OrganizationId END
WHERE PersonId = @PersonId

