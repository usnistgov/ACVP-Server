
CREATE PROCEDURE [dbo].[VectorSetJsonPut]

     @VectorSetId BIGINT
    ,@VectorSetJsonFileTypeId BIGINT
    ,@Content VARCHAR(max)

AS

SET NOCOUNT ON



INSERT INTO [dbo].[VectorSetJson] (VectorSetId, VectorSetJsonFileTypeId, Content, CreatedOn, ShouldReplicate)
VALUES (@VectorSetId, @VectorSetJsonFileTypeId, @Content, CURRENT_TIMESTAMP, CASE
																				WHEN @VectorSetJsonFileTypeId IN (2,6,7) THEN 1		-- Prompt, Validation, or Error
																				WHEN @VectorSetJsonFileTypeId = 4					-- Expected Answers if the TS is a sample
																				 AND EXISTS (SELECT NULL
																							 FROM dbo.VectorSets VS
																									INNER JOIN
																									dbo.TestSessions TS ON TS.TestSessionId = VS.TestSessionId
																														AND VS.VectorSetId = @VectorSetId
																														AND TS.IsSample = 1) THEN 1
																				ELSE 0
                                                                             END)