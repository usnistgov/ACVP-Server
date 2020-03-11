CREATE PROCEDURE [val].[OrganizationEmailsGet]
	@OrganizationID bigint
AS
	SELECT * FROM val.ORGANIZATION_EMAIL AS ORGEMAIL
	WHERE ORGEMAIL.organization_id = @OrganizationID
	ORDER BY ORGEMAIL.order_index