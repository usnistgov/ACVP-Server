CREATE PROCEDURE [dbo].[VectorSetsGetByTestSessionId]

	@TestSessionId bigint

AS

SET NOCOUNT ON

SELECT	 VS.VectorSetId
		,VS.GeneratorVersion
		,VS.AlgorithmId
		,VS.VectorSetStatusId
		,A.DisplayName
FROM dbo.VectorSets VS
	INNER JOIN
	dbo.Algorithms A ON A.AlgorithmId = VS.AlgorithmId
					AND VS.TestSessionId = @TestSessionId