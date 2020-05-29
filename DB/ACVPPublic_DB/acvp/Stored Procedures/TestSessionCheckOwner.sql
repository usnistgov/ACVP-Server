CREATE PROCEDURE [acvp].[TestSessionCheckOwner]
    @TestSessionID BIGINT,
    @UserID BIGINT,
    @Result BIT OUTPUT
AS
SET @Result = CASE WHEN EXISTS (
    SELECT 1
--    FROM [acvp].[TestSession]
--    WHERE id = @TestSessionID AND user_id = @UserID
    FROM [acvp].[TEST_SESSION]
    WHERE id = @TestSessionID AND user_id = @UserID
)
THEN 1
ELSE 0 END