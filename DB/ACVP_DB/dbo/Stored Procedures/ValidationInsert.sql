CREATE PROCEDURE [dbo].[ValidationInsert]

	 @ImplementationId bigint
	,@ValidationSourceId int
	,@ValidationNumber bigint

AS

SET NOCOUNT ON

INSERT INTO dbo.Validations (
		 ImplementationId
		,ValidationSourceId
		,ValidationNumber
		,CreatedOn
		,LastUpdated
		)
VALUES (
		 @ImplementationId
		,@ValidationSourceId
		,@ValidationNumber
		,CURRENT_TIMESTAMP
		,CURRENT_TIMESTAMP
)

SELECT CAST(SCOPE_IDENTITY() AS bigint) AS ValidationId