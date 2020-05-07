CREATE VIEW [csrc].[VALIDATION_RECORD]
WITH SCHEMABINDING
AS

SELECT	 R.id
		,R.product_information_id
		,R.source_id
		,R.validation_id
		,R.created_on
		,R.updated_on
FROM val.VALIDATION_RECORD R
	INNER JOIN
	val.PRODUCT_INFORMATION P ON P.id = R.product_information_id
							AND P.itar = 0


GO
CREATE UNIQUE CLUSTERED INDEX [PK_CSRC_VALIDATION_RECORD]
    ON [csrc].[VALIDATION_RECORD]([id] ASC);

