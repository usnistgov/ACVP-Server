CREATE PROCEDURE [val].[PersonsForImplementationGet]
    @ImplementationID BIGINT
AS

SELECT person_id AS PersonID
FROM [val].[VALIDATION_CONTACT]
WHERE id = @ImplementationID
ORDER BY order_index