CREATE PROCEDURE [val].[OEDependenciesGet]
	@OEID int = 0
AS
	SELECT DEP.id,
	DEP.name,
	DEP.dependency_type,
	DEP.description
	FROM val.VALIDATION_OE_DEPENDENCY_LINK AS DEP_LINK
	JOIN val.VALIDATION_OE_DEPENDENCY as DEP ON DEP.id = DEP_LINK.dependency_id
	WHERE DEP.id = @OEID