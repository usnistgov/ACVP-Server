CREATE PROCEDURE [dbo].[DependencyInsert]

	 @Type nvarchar(1024)
	,@Name nvarchar(1024)
	,@Description nvarchar(2048)

AS

SET NOCOUNT ON

INSERT INTO dbo.Dependencies(DependencyType, [Name], [Description])
VALUES (@Type, @Name, @Description)

SELECT CAST(SCOPE_IDENTITY() AS bigint) AS DependencyId