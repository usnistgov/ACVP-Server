CREATE PROCEDURE [dbo].[OEUpdate]

	 @OEId bigint
	,@Name nvarchar(2048)

AS

SET NOCOUNT ON

UPDATE dbo.OEs
SET [Name] = @Name
WHERE OEId = @OEId