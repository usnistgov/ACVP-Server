﻿CREATE VIEW [csrc].[VALIDATION_SCENARIO]
WITH SCHEMABINDING
AS

SELECT	 S.id
		,S.record_id
FROM val.VALIDATION_SCENARIO S
	INNER JOIN
	val.VALIDATION_RECORD R ON R.id = S.record_id
	INNER JOIN
	val.PRODUCT_INFORMATION P ON P.id = R.product_information_id
							AND P.itar = 0
