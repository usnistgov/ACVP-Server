CREATE TABLE [dbo].[ValidationOEAlgorithms] (
    [ValidationOEAlgorithmId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ValidationId]            BIGINT        NOT NULL,
    [OEId]                    BIGINT        NOT NULL,
    [AlgorithmId]             BIGINT        NOT NULL,
    [VectorSetId]             BIGINT        NULL,
    [Active]                  BIT           CONSTRAINT [DF_ValidationOEAlgorithms_Active] DEFAULT ((1)) NOT NULL,
    [CreatedOn]               DATETIME2 (7) CONSTRAINT [DF_ValidationOEAlgorithms_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [InactiveOn]              DATETIME2 (7) NULL,
    CONSTRAINT [PK_ValidationOEAlgorithms] PRIMARY KEY CLUSTERED ([ValidationOEAlgorithmId] ASC),
    CONSTRAINT [FK_ValidationOEAlgorithms_Algorithms] FOREIGN KEY ([AlgorithmId]) REFERENCES [dbo].[Algorithms] ([AlgorithmId]),
    CONSTRAINT [FK_ValidationOEAlgorithms_OEs] FOREIGN KEY ([OEId]) REFERENCES [dbo].[OEs] ([OEId]),
    CONSTRAINT [FK_ValidationOEAlgorithms_VectorSets] FOREIGN KEY ([VectorSetId]) REFERENCES [dbo].[VectorSets] ([VectorSetId]),
    CONSTRAINT [FK_ValidationOEAlgorithms_Validations] FOREIGN KEY ([ValidationId]) REFERENCES [dbo].[Validations] ([ValidationId])
);


