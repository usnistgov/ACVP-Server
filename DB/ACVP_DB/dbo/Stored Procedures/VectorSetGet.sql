CREATE PROCEDURE [dbo].[VectorSetGet]
	
	@VectorSetId bigint

AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT	 VS.VectorSetId
			,VS.TestSessionId
			,VS.GeneratorVersion
			,VS.AlgorithmId
			,VS.VectorSetStatusId
			,A.DisplayName
	FROM dbo.VectorSets VS
		INNER JOIN
		dbo.Algorithms A ON A.AlgorithmId = VS.AlgorithmId
						AND	VS.VectorSetId = @VectorSetId

END