CREATE PROCEDURE [val].[OrganizationExists]
	
	@organizationId BIGINT,
	@exists BIT OUTPUT

AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT	@exists = CASE WHEN EXISTS (
		SELECT	1
		FROM	ORGANIZATION o
		WHERE	id = @organizationId
	) THEN 1 ELSE 0 END
    
END