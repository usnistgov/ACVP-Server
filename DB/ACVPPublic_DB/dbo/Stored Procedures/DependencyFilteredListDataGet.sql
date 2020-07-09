CREATE PROCEDURE [dbo].[DependencyFilteredListDataGet]

	@IDs varchar(MAX)

AS

SET NOCOUNT ON

-- Get the dependency level data
SELECT	 D.DependencyId
		,D.DependencyType
		,D.[Name]
		,D.[Description]
FROM dbo.Dependencies D
	INNER JOIN
	dbo.DelimitedListToTable(@IDs, ',') X ON X.[Value] = D.DependencyId

-- Get the attributes for all of these
SELECT	 A.DependencyId
		,A.[Name]
		,A.[Value] 
FROM dbo.DependencyAttributes A
	INNER JOIN
	dbo.DelimitedListToTable(@IDs, ',') X ON X.[Value] = A.DependencyId
