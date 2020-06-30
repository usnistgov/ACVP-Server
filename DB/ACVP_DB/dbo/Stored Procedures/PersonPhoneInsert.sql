CREATE PROCEDURE [dbo].[PersonPhoneInsert]

	 @PersonId bigint
	,@Type nvarchar(32)
	,@Number nvarchar(64)
	,@OrderIndex int

AS

SET NOCOUNT ON

INSERT INTO dbo.PersonPhoneNumbers(
	 PersonId
	,OrderIndex
	,PhoneNumber
	,PhoneNumberType
)
VALUES (
	 @PersonId
	,@OrderIndex
	,@Number
	,@Type
)

