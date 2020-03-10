CREATE PROCEDURE [val].[OrganizationGetAddresses]
	@OrganizationID bigint
AS
	SELECT * FROM val.ADDRESS AS ADDRESS
	WHERE ADDRESS.organization_id = @OrganizationID