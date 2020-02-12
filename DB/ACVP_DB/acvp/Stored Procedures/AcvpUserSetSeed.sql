CREATE PROCEDURE [acvp].[AcvpUserSetSeed]
	
	@acvpUserId BIGINT,
	@seed NVARCHAR(64)

AS
BEGIN
	SET NOCOUNT ON;

	UPDATE	acvp.ACVP_USER
	SET		seed = @seed
	WHERE	id = @acvpUserId

END