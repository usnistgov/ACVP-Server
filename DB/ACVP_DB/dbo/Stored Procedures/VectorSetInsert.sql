CREATE PROCEDURE [dbo].[VectorSetInsert]

	 @VectorSetId bigint
	,@TestSessionId bigint
	,@GeneratorVersion nvarchar(10)
	,@AlgorithmId bigint

AS

SET NOCOUNT ON

INSERT INTO dbo.VectorSets (
	 VectorSetId
	,TestSessionId
	,GeneratorVersion
	,AlgorithmId
	)
VALUES (
	 @VectorSetId
	,@TestSessionId
	,@GeneratorVersion
	,@AlgorithmId
	)