CREATE PROCEDURE [val].[DependencyInsert]

	 @Type nvarchar(1024)
	,@Name nvarchar(1024)
	,@Description nvarchar(2048)

AS

SET NOCOUNT ON

INSERT INTO val.VALIDATION_OE_DEPENDENCY (dependency_type, [name], [description])
VALUES (@Type, @Name, @Description)

SELECT SCOPE_IDENTITY() AS DependencyID