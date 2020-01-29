CREATE PROCEDURE [val].[ValidationTestSessionsInsert]
	
	 @ValidationId bigint
	,@TestSessionId bigint

AS

SET NOCOUNT ON

INSERT INTO val.ValidationTestSessions (ValidationId, TestSessionId, ValidationDate)
VALUES (@ValidationId, @TestSessionId, CURRENT_TIMESTAMP)

