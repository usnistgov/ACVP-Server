CREATE PROCEDURE [acvp].[TestSessionStatusUpdateForJava]
	
	 @TestSessionId bigint
	,@PassedDate datetime2(7)
	,@Disposition bit
	,@Publishable bit
	,@Published bit
AS

SET NOCOUNT ON

UPDATE acvp.TEST_SESSION
SET	 passed_date = ISNULL(@PassedDate, passed_date)
	,disposition = ISNULL(@Disposition, disposition)
	,publishable = ISNULL(@Publishable, publishable)
	,published = ISNULL(@Published, published)
WHERE id = @TestSessionId