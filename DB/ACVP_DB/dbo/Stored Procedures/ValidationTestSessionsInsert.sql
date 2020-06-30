CREATE PROCEDURE [dbo].[ValidationTestSessionsInsert]
	
	 @ValidationId bigint
	,@TestSessionId bigint

AS

SET NOCOUNT ON

INSERT INTO dbo.ValidationTestSessions (ValidationId, TestSessionId, ValidationDate)
VALUES (@ValidationId, @TestSessionId, CURRENT_TIMESTAMP)

