CREATE PROCEDURE [dbo].[ValidationOEAlgorithmInsert]

	 @ValidationId bigint
	,@OEId bigint
	,@AlgorithmId bigint
	,@VectorSetId bigint

AS

SET NOCOUNT ON

INSERT INTO dbo.ValidationOEAlgorithms (ValidationId, OEId, AlgorithmId, VectorSetId)
VALUES (@ValidationId, @OEId, @AlgorithmId, @VectorSetId)

SELECT CAST(SCOPE_IDENTITY() AS bigint) AS ValidationOEAlgorithmId

