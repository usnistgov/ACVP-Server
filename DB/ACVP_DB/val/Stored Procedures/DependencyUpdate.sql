CREATE PROCEDURE [val].[DependencyUpdate]

	 @DependencyID bigint
	,@Type nvarchar(1024)
	,@Name nvarchar(1024)
	,@Description nvarchar(2048)
	,@TypeUpdated bit
	,@NameUpdated bit
	,@DescriptionUpdated bit

AS

SET NOCOUNT ON

UPDATE val.VALIDATION_OE_DEPENDENCY
SET  dependency_type = CASE @TypeUpdated WHEN 1 THEN @Type ELSE dependency_type END
	,[name] = CASE @NameUpdated WHEN 1 THEN @Name ELSE [name] END
	,[description] = CASE @DescriptionUpdated WHEN 1 THEN @Description ELSE [description] END
WHERE id = @DependencyID