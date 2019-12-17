
CREATE VIEW [csrc].[VALIDATION_NOTE]
WITH SCHEMABINDING
AS


SELECT	 N.id
		,N.record_id
		,N.algorithm_id
		,N.internal
		,N.note
		,N.updated_on 
FROM val.VALIDATION_NOTE N
	INNER JOIN
	val.VALIDATION_RECORD R ON R.id = N.record_id
	INNER JOIN
	val.PRODUCT_INFORMATION P ON P.id = R.product_information_id
							AND P.itar = 0

