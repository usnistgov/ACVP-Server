﻿
CREATE VIEW [csrc].[VALIDATION_PREREQUISITE]
WITH SCHEMABINDING
AS

SELECT	 Q.id
		,Q.scenario_algorithm_id
		,Q.record_id
		,Q.requirement
FROM val.VALIDATION_PREREQUISITE Q
	INNER JOIN
	val.VALIDATION_RECORD R ON R.id = Q.record_id
	INNER JOIN
	val.PRODUCT_INFORMATION P ON P.id = R.product_information_id
							AND P.itar = 0
