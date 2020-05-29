CREATE VIEW [acvp].[FilteredVectorSetJson]
WITH SCHEMABINDING
AS 

SELECT	 J.VsId
		,J.FileType
		,J.Content
		,J.CreatedOn
FROM acvp.VectorSetJson J
	INNER JOIN
	acvp.VECTOR_SET VS ON VS.id = J.VsId
	INNER JOIN
	acvp.TEST_SESSION TS ON TS.id = VS.test_session_id
						AND (J.FileType IN (2, 6, 7)	-- Prompt, Validation, or Error
							OR (J.FileType = 4 AND TS.[sample] = 1))	-- Expected Answers if the TS is a sample

GO

CREATE UNIQUE CLUSTERED INDEX [PK_ACVP_FILTEREDVECTORSETJSON] ON [acvp].[FilteredVectorSetJson]
(
	 [VsId] ASC
	,[FileType] ASC
)