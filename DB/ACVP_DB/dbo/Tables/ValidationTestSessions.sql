CREATE TABLE [dbo].[ValidationTestSessions]
(
	[ValidationTestSessionId] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [ValidationId] BIGINT NOT NULL, 
    [TestSessionId] BIGINT NOT NULL, 
    [ValidationDate] DATETIME2 NOT NULL
)
