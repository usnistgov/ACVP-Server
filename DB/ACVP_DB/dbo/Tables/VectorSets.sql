CREATE TABLE [dbo].[VectorSets] (
    [VectorSetId]         BIGINT         NOT NULL,
    [TestSessionId]       BIGINT         NOT NULL,
    [GeneratorVersion]    NVARCHAR (10)  NOT NULL,
    [AlgorithmId]         BIGINT         NOT NULL,
    [VectorSetStatusId]   INT CONSTRAINT [DF_VectorSets_VectorSetStatusId] DEFAULT ((0)) NOT NULL,
    [ErrorMessage] NVARCHAR (MAX) NULL,
    [Archived] BIT NOT NULL CONSTRAINT [DF_VectorSets_Archived] DEFAULT 0, 
    CONSTRAINT [PK_VectorSets] PRIMARY KEY CLUSTERED ([VectorSetId] ASC),
    CONSTRAINT [FK_VectorSets_Algorithms] FOREIGN KEY ([AlgorithmId]) REFERENCES [dbo].[Algorithms] ([AlgorithmId]),
    CONSTRAINT [FK_VectorSets_TestSessions] FOREIGN KEY ([TestSessionId]) REFERENCES [dbo].[TestSessions] ([TestSessionId]),
    CONSTRAINT [FK_VectorSets_VectorSetStatuses] FOREIGN KEY ([VectorSetStatusId]) REFERENCES [dbo].[VectorSetStatuses] ([VectorSetStatusId])
);

