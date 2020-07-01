CREATE PROCEDURE [dbo].[OrganizationEmailInsert]

	 @OrganizationId bigint
	,@EmailAddress varchar(512)
	,@OrderIndex int

AS

SET NOCOUNT ON

INSERT INTO dbo.OrganizationEmails (
	 OrganizationId
	,OrderIndex
	,EmailAddress
)
VALUES (
	 @OrganizationId
	,@OrderIndex
	,@EmailAddress
)

