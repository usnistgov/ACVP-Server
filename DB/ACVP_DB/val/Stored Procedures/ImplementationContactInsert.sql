CREATE PROCEDURE [val].[ImplementationContactInsert]

	 @ImplementationID bigint
	,@PersonID bigint
	,@OrderIndex int

AS

SET NOCOUNT ON

INSERT INTO val.VALIDATION_CONTACT (
	 product_information_id
	,person_id
	,order_index
)
VALUES (
	 @ImplementationID
	,@PersonID
	,@OrderIndex
)