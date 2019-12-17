CREATE PROCEDURE [acvp].[TestSessionInsert]

	 @TestSessionID bigint
	,@ACVVersionID int
	,@Generator nvarchar(32)
	,@IsSample bit
	,@Publishable bit
	,@UserID bigint

AS

SET NOCOUNT ON

INSERT INTO acvp.TEST_SESSION (id, acv_version_id, generator, [sample], publishable, [user_id], created_on)
VALUES (@TestSessionID, @ACVVersionID, @Generator, @IsSample, @Publishable, @UserID, CURRENT_TIMESTAMP)
