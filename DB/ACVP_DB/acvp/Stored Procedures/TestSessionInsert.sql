CREATE PROCEDURE [acvp].[TestSessionInsert]

	 @TestSessionID bigint
	,@ACVVersionID int
	,@Generator nvarchar(32)
	,@IsSample bit
	,@UserID bigint

AS

SET NOCOUNT ON

INSERT INTO acvp.TEST_SESSION (id, acv_version_id, generator, [sample], TestSessionStatusId, [user_id], created_on, LastTouched)
VALUES (@TestSessionID, @ACVVersionID, @Generator, @IsSample, 2, @UserID, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)
