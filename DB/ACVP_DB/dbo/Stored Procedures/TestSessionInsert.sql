CREATE PROCEDURE [dbo].[TestSessionInsert]

	 @TestSessionId bigint
	,@ACVVersionId int
	,@Generator nvarchar(32)
	,@IsSample bit
	,@UserId bigint

AS

SET NOCOUNT ON

INSERT INTO dbo.TestSessions (TestSessionId, ACVVersionId, Generator, IsSample, TestSessionStatusId, AcvpUserId, CreatedOn, LastTouched)
VALUES (@TestSessionId, @ACVVersionId, @Generator, @IsSample, 2, @UserId, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)
