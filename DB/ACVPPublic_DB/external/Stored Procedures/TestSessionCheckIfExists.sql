﻿CREATE PROCEDURE [external].[TestSessionCheckIfExists]
	@TestSessionID BIGINT
	,@Exists BIT OUTPUT
AS
BEGIN
	
	SET NOCOUNT ON;

    -- This proc is needed in cases where a test session has been created, 
	-- but has not yet been replicated back to the public site.
	-- We can just validate that the test session being requested actually exists in the external id table
	SET @Exists = CASE WHEN EXISTS (
		SELECT	1
		FROM	[external].[test_session]
		WHERE	id = @TestSessionId
	) THEN 1 ELSE 0 END

END