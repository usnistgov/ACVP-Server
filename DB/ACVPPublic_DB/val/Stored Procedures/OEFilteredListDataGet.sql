
CREATE PROCEDURE [val].[OEFilteredListDataGet]

	 @IDs varchar(MAX)

AS

SET NOCOUNT ON

-- Get the OE level data
SELECT	 OE.id AS Id
		,OE.[name] AS [Name]
FROM val.VALIDATION_OE OE
	INNER JOIN
	dbo.DelimitedListToTable(@IDs, ',') X ON X.Value = OE.id

-- Get the dependency ids for all of these
SELECT	 L.validation_oe_id AS OEId
		,L.dependency_id AS DependencyId
FROM val.VALIDATION_OE_DEPENDENCY_LINK L
	INNER JOIN
	dbo.DelimitedListToTable(@IDs, ',') X ON X.Value = L.validation_oe_id