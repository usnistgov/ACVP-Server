CREATE PROCEDURE [dbo].[PersonInsert]

	 @Name nvarchar(1024)
	,@OrganizationId bigint

AS

SET NOCOUNT ON

INSERT INTO dbo.People (
	 FullName
	,OrganizationId
)
VALUES (
	 @Name
	,@OrganizationId
)

SELECT CAST(SCOPE_IDENTITY() AS bigint) AS PersonId

