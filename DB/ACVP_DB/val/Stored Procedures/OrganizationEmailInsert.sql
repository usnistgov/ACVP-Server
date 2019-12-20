CREATE PROCEDURE [val].[OrganizationEmailInsert]

	 @OrganizationID bigint
	,@EmailAddress varchar(512)
	,@OrderIndex int

AS

SET NOCOUNT ON

INSERT INTO val.ORGANIZATION_EMAIL(
	 organization_id
	,order_index
	,email_address
)
VALUES (
	 @OrganizationID
	,@OrderIndex
	,@EmailAddress
)

