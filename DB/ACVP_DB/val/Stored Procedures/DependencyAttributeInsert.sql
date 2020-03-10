CREATE PROCEDURE [val].[DependencyAttributeInsert]

	 @DependencyID bigint
	,@Name nvarchar(1024)
	,@Value nvarchar(1024)

AS

SET NOCOUNT ON

INSERT INTO val.VALIDATION_OE_DEPENDENCY_ATTRIBUTE (validation_oe_dependency_id, [name], [value])
VALUES (@DependencyID, @Name, @Value)

SELECT CAST(SCOPE_IDENTITY() AS bigint) AS DependencyAttributeID