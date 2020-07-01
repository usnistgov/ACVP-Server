CREATE PROCEDURE [dbo].[DependencyUpdate]

	 @DependencyId bigint
	,@Type nvarchar(1024)
	,@Name nvarchar(1024)
	,@Description nvarchar(2048)
	,@TypeUpdated bit
	,@NameUpdated bit
	,@DescriptionUpdated bit

AS

SET NOCOUNT ON

UPDATE dbo.Dependencies
SET  DependencyType = CASE @TypeUpdated WHEN 1 THEN @Type ELSE DependencyType END
	,[Name] = CASE @NameUpdated WHEN 1 THEN @Name ELSE [Name] END
	,[Description] = CASE @DescriptionUpdated WHEN 1 THEN @Description ELSE [Description] END
WHERE DependencyId = @DependencyId