CREATE TABLE [dbo].[ValidationTestSessions]
(
	[ValidationTestSessionId] BIGINT NOT NULL IDENTITY, 
    [ValidationId] BIGINT NOT NULL, 
    [TestSessionId] BIGINT NOT NULL, 
    [ValidationDate] DATETIME2 NOT NULL,
    CONSTRAINT [PK_ValidationTestSessions] PRIMARY KEY CLUSTERED ([ValidationTestSessionId] ASC),
    CONSTRAINT [FK_ValidationTestSessions_Validations] FOREIGN KEY ([ValidationId]) REFERENCES [dbo].[Validations] ([ValidationId]),
    CONSTRAINT [FK_ValidationTestSessions_TestSessions] FOREIGN KEY ([TestSessionId]) REFERENCES [dbo].[TestSessions] ([TestSessionId])
)
