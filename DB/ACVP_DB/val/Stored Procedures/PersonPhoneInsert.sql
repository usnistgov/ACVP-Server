CREATE PROCEDURE [val].[PersonPhoneInsert]

	 @PersonID bigint
	,@Type nvarchar(32)
	,@Number nvarchar(64)
	,@OrderIndex int

AS

SET NOCOUNT ON

INSERT INTO val.PERSON_PHONE (
	 person_id
	,order_index
	,phone_number
	,phone_number_type
)
VALUES (
	 @PersonID
	,@OrderIndex
	,@Number
	,@Type
)

