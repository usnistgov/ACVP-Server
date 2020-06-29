CREATE TABLE [dbo].[VectorSets] (
    [VectorSetId]                   BIGINT         NOT NULL,
    [TestSessionId]      BIGINT         NOT NULL,
    [GeneratorVersion]    NVARCHAR (10)  NOT NULL,
    [AlgorithmId]         BIGINT         NOT NULL,
    [VectorSetStatusId]               INT            DEFAULT ((0)) NOT NULL,
    [ErrorMessage] NVARCHAR (MAX) NULL,
    [Archived] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_VectorSets] PRIMARY KEY CLUSTERED ([VectorSetId] ASC),
    CONSTRAINT [FK_VectorSets_AlgorithmId] FOREIGN KEY ([AlgorithmId]) REFERENCES [dbo].[Algorithms] ([AlgorithmId]),
    CONSTRAINT [FK_VectorSets_TestSessionId] FOREIGN KEY ([TestSessionId]) REFERENCES [dbo].[TestSessions] ([TestSessionId])
);

