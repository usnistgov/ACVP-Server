CREATE VIEW [dbo].[FilteredVectorSetJson]
WITH SCHEMABINDING
AS 

SELECT	 J.VectorSetId
		,J.VectorSetJsonFileTypeId
		,J.Content
		,J.CreatedOn
FROM dbo.VectorSetJson J
	INNER JOIN
	dbo.VectorSets VS ON VS.VectorSetId = J.VectorSetId
	INNER JOIN
	dbo.TestSessions TS ON TS.TestSessionId = VS.TestSessionId
						AND (J.VectorSetJsonFileTypeId IN (2, 6, 7)	-- Prompt, Validation, or Error
							OR (J.VectorSetJsonFileTypeId = 4 AND TS.IsSample = 1))	-- Expected Answers if the TS is a sample

GO

CREATE UNIQUE CLUSTERED INDEX [PK_ACVP_FILTEREDVECTORSETJSON] ON [dbo].[FilteredVectorSetJson]
(
	 [VectorSetId] ASC
	,[VectorSetJsonFileTypeId] ASC
)