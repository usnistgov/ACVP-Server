CREATE TABLE [dbo].[ExternalVectorSets] (
    [VectorSetId]              BIGINT         IDENTITY (1, 1) NOT NULL,
    [TestSessionId] BIGINT         NOT NULL,
    [Token]           NVARCHAR (128) NULL,
    CONSTRAINT [PK_ExternalVectorSets] PRIMARY KEY CLUSTERED ([VectorSetId] ASC),
    CONSTRAINT [FK_ExternalVectorSets_ExternalTestSessions] FOREIGN KEY ([TestSessionId]) REFERENCES [dbo].[ExternalTestSessions] ([TestSessionId])
);

