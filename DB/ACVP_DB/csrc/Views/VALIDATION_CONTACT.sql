
CREATE VIEW [csrc].[VALIDATION_CONTACT]
WITH SCHEMABINDING
AS

SELECT	 c.id
		,C.product_information_id
		,C.person_id
		,C.order_index
FROM val.VALIDATION_CONTACT C
	INNER JOIN
	val.PRODUCT_INFORMATION P ON P.id = C.product_information_id
							AND P.itar = 0

