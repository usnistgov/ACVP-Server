CREATE PROCEDURE [acvp].[VectorSetInsert]

	 @VectorSetID bigint
	,@TestSessionID bigint
	,@GeneratorVersion nvarchar(10)
	,@AlgorithmID bigint

AS

SET NOCOUNT ON

INSERT INTO acvp.VECTOR_SET (
	 id
	,test_session_id
	,generator_version
	,algorithm_id
	)
VALUES (
	 @VectorSetID
	,@TestSessionID
	,@GeneratorVersion
	,@AlgorithmID
	)