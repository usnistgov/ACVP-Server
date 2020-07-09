CREATE PROCEDURE [dbo].[OEFilteredListDataGet]

	 @IDs varchar(MAX)

AS

SET NOCOUNT ON

-- Get the OE level data
SELECT	 OE.OEId
		,OE.[Name]
FROM dbo.OEs OE
	INNER JOIN
	dbo.DelimitedListToTable(@IDs, ',') X ON X.Value = OE.OEId

-- Get the dependency ids for all of these
SELECT	 D.OEId
		,D.DependencyId
FROM dbo.OEDependencies D
	INNER JOIN
	dbo.DelimitedListToTable(@IDs, ',') X ON X.Value = D.OEId
