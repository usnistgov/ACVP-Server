CREATE TABLE [dbo].[ValidationOEAlgorithms] (
    [ValidationOEAlgorithmId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ValidationId]            BIGINT        NOT NULL,
    [OEId]                    BIGINT        NOT NULL,
    [AlgorithmId]             BIGINT        NOT NULL,
    [VectorSetId]             BIGINT        NULL,
    [Active]                  BIT           DEFAULT ((1)) NOT NULL,
    [CreatedOn]               DATETIME2 (7) DEFAULT (getdate()) NOT NULL,
    [InactiveOn]              DATETIME2 (7) NULL,
    CONSTRAINT [PK__ValidationOEAlgorithms] PRIMARY KEY CLUSTERED ([ValidationOEAlgorithmId] ASC)
);


