

CREATE VIEW [csrc].[VALIDATION_CAPABILITY]
WITH SCHEMABINDING
AS

SELECT	 C.id
		,C.scenario_algorithm_id
		,C.algorithm_property
		,C.historical
		,C.[root]
		,C.[level]
		,C.parent
		,C.[type]
		,C.order_index
		,C.string_value
		,C.number_value
		,C.boolean_value
FROM val.VALIDATION_CAPABILITY C
	INNER JOIN
	val.VALIDATION_SCENARIO_ALGORITHM A ON A.id = C.scenario_algorithm_id
	INNER JOIN
	val.VALIDATION_SCENARIO S ON S.id = A.scenario_id
	INNER JOIN
	val.VALIDATION_RECORD R ON R.id = S.record_id
	INNER JOIN
	val.PRODUCT_INFORMATION P ON P.id = R.product_information_id
							AND P.itar = 0

