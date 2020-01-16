CREATE PROCEDURE [val].[AddressDelete]

	@AddressID bigint

AS

SET NOCOUNT ON

DELETE 
FROM val.[ADDRESS]
WHERE id = @AddressID