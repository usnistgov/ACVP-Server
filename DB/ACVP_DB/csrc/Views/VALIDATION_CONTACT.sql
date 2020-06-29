
CREATE VIEW [csrc].[VALIDATION_CONTACT]
WITH SCHEMABINDING
AS

SELECT	 c.id
		,C.product_information_id
		,C.person_id
		,C.order_index
FROM val.VALIDATION_CONTACT C
	INNER JOIN
	dbo.Implementations I ON I.ImplementationId = C.product_information_id
							AND I.ITAR = 0


GO
CREATE UNIQUE CLUSTERED INDEX [PK_CSRC_VALIDATION_CONTACT]
    ON [csrc].[VALIDATION_CONTACT]([id] ASC);

