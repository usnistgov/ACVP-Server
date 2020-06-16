CREATE PROCEDURE [val].[PersonsForImplementationGet]
    @ImplementationID BIGINT
AS

SET NOCOUNT ON

SELECT person_id AS PersonID
FROM [val].[VALIDATION_CONTACT]
WHERE product_information_id = @ImplementationID
ORDER BY order_index