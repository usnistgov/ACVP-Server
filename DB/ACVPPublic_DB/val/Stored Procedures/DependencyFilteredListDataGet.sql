
CREATE PROCEDURE [val].[DependencyFilteredListDataGet]
	
	 @IDs varchar(MAX)

AS

SET NOCOUNT ON

-- Get the dependency level data
SELECT	 D.id AS Id
		,D.dependency_type AS DependencyType
		,D.[name] AS [Name]
		,D.[description] AS [Description]
FROM val.VALIDATION_OE_DEPENDENCY D
	INNER JOIN
	dbo.DelimitedListToTable(@IDs, ',') X ON X.[Value] = D.id

-- Get the attributes for all of these
SELECT	 A.validation_oe_dependency_id as DependencyId
		,A.[name] AS [Name]
		,A.[value] AS [Value] 
FROM val.VALIDATION_OE_DEPENDENCY_ATTRIBUTE A
	INNER JOIN
	dbo.DelimitedListToTable(@IDs, ',') X ON X.[Value] = A.validation_oe_dependency_id