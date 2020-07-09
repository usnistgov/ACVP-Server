CREATE PROCEDURE [dbo].[OrganizationExists]
	
	@OrganizationId BIGINT,
	@Exists BIT OUTPUT

AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT	@Exists = CASE WHEN EXISTS (
		SELECT	1
		FROM	dbo.Organizations
		WHERE	OrganizationId = @OrganizationId
	) THEN 1 ELSE 0 END
    
END