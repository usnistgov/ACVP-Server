CREATE TABLE [dbo].[Prerequisites] (
    [PrerequisiteId]                    BIGINT          IDENTITY (1, 1) NOT NULL,
    [ValidationOEAlgorithmID] BIGINT          NOT NULL,
    [ValidationId]             BIGINT          NOT NULL,
    [Requirement]           NVARCHAR (2048) NOT NULL,
    CONSTRAINT [PK_PREREQUISITE] PRIMARY KEY CLUSTERED ([PrerequisiteId] ASC),
    CONSTRAINT [FK_PREREQUISITE_RECORD_ID] FOREIGN KEY ([ValidationId]) REFERENCES [dbo].[Validations] ([ValidationId]),
    CONSTRAINT [FK_PREREQUISITE_SCENARIO_ALGORITHM_ID] FOREIGN KEY ([ValidationOEAlgorithmId]) REFERENCES [dbo].[ValidationOEAlgorithms] ([ValidationOEAlgorithmId])
);

