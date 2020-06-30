CREATE PROCEDURE [dbo].[AddressDelete]

	@AddressId bigint

AS

SET NOCOUNT ON

DELETE 
FROM dbo.Addresses
WHERE AddressId = @AddressId