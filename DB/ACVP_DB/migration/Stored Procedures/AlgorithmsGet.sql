CREATE PROCEDURE [migration].[AlgorithmsGet]

AS
	SET NOCOUNT ON

	SELECT	 id
			,name
			,mode
			,revision
	FROM ref.CRYPTO_ALGORITHM


