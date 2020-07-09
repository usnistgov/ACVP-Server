CREATE TABLE [dbo].[VectorSets] (
    [VectorSetId]       BIGINT         NOT NULL,
    [TestSessionId]     BIGINT         NOT NULL,
    [GeneratorVersion]  NVARCHAR (10)  NOT NULL,
    [AlgorithmId]       BIGINT         NOT NULL,
    [VectorSetStatusId] INT            NOT NULL,
    [ErrorMessage]      NVARCHAR (MAX) NULL,
    [Archived]          BIT            NOT NULL,
    CONSTRAINT [PK_VectorSets] PRIMARY KEY CLUSTERED ([VectorSetId] ASC)
);

