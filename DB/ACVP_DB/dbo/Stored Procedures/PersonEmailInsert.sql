CREATE PROCEDURE [dbo].[PersonEmailInsert]

	 @PersonId bigint
	,@EmailAddress varchar(512)
	,@OrderIndex int

AS

SET NOCOUNT ON

INSERT INTO dbo.PersonEmails(
	 PersonId
	,OrderIndex
	,EmailAddress
)
VALUES (
	 @PersonId
	,@OrderIndex
	,@EmailAddress
)

