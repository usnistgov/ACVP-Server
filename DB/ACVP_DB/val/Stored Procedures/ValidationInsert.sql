CREATE PROCEDURE [val].[ValidationInsert]

	 @ImplementationId bigint
	,@SourceId int
	,@ValidationNumber bigint

AS

SET NOCOUNT ON

INSERT INTO val.VALIDATION_RECORD (
		 product_information_id
		,source_id
		,validation_id
		,created_on
		,updated_on
		)
VALUES (
		 @ImplementationId
		,@SourceId
		,@ValidationNumber
		,CURRENT_TIMESTAMP
		,CURRENT_TIMESTAMP
)

SELECT CAST(SCOPE_IDENTITY() AS bigint) AS ValidationId