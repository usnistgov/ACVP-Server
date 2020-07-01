CREATE PROCEDURE [dbo].[DependencyAttributeInsert]

	 @DependencyId bigint
	,@Name nvarchar(1024)
	,@Value nvarchar(1024)

AS

SET NOCOUNT ON

INSERT INTO dbo.DependencyAttributes(DependencyId, [Name], [Value])
VALUES (@DependencyId, @Name, @Value)

SELECT CAST(SCOPE_IDENTITY() AS bigint) AS DependencyAttributeId