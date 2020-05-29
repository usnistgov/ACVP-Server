CREATE PROCEDURE [val].[DependencyGet]
    @DependencyID BIGINT
	
AS

SELECT id AS ID
    ,dependency_type AS DependencyType
    ,[name] AS [Name]
    ,[description] AS [Description]
FROM [val].[VALIDATION_OE_DEPENDENCY]
WHERE id = @DependencyID