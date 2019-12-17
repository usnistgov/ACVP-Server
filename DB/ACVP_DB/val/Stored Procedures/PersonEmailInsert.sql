CREATE PROCEDURE [val].[PersonEmailInsert]

	 @PersonID bigint
	,@EmailAddress varchar(512)
	,@OrderIndex int

AS

SET NOCOUNT ON

INSERT INTO val.PERSON_EMAIL (
	 person_id
	,order_index
	,email_address
)
VALUES (
	 @PersonID
	,@OrderIndex
	,@EmailAddress
)

