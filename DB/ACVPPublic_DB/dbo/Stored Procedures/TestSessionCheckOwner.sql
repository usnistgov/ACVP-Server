CREATE PROCEDURE [dbo].[TestSessionCheckOwner]

     @TestSessionId BIGINT
    ,@ACVPUserId BIGINT
    ,@Result BIT OUTPUT

AS

SET NOCOUNT ON

SET @Result = CASE WHEN EXISTS (SELECT 1
                                FROM dbo.TestSessions
                                WHERE TestSessionId = @TestSessionId
                                  AND ACVPUserId = @ACVPUserId) THEN 1
                   ELSE 0
              END