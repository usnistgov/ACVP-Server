CREATE PROCEDURE [dbo].[ImplementationContactInsert]

	 @ImplementationId bigint
	,@PersonId bigint
	,@OrderIndex int

AS

SET NOCOUNT ON

INSERT INTO dbo.ImplementationContacts(
	 ImplementationId
	,PersonId
	,OrderIndex
)
VALUES (
	 @ImplementationId
	,@PersonId
	,@OrderIndex
)