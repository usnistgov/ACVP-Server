CREATE PROCEDURE [val].[DependencyAttributeGet]
    @DependencyID BIGINT
	
AS

SELECT [name] as [Name]
    ,[value] AS [Value]
FROM [val].[VALIDATION_OE_DEPENDENCY_ATTRIBUTE]
WHERE validation_oe_dependency_id = @DependencyID