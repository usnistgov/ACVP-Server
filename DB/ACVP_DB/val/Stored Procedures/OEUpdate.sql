CREATE PROCEDURE [val].[OEUpdate]

	 @OEID bigint
	,@Name nvarchar(2048)

AS

SET NOCOUNT ON

UPDATE val.VALIDATION_OE 
SET [name] = @Name
WHERE id = @OEID