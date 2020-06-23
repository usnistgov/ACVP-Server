CREATE PROCEDURE [val].[ImplementationContactsGet]
	@ImplementationId BIGINT
AS

	SELECT p.id,
		   p.full_name
	FROM val.VALIDATION_CONTACT AS c
		JOIN val.PERSON AS p
		ON c.person_id = p.id
	WHERE c.product_information_id = @ImplementationId