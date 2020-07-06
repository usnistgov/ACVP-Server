CREATE TABLE [dbo].[Prerequisites] (
    [PrerequisiteId]                    BIGINT          IDENTITY (1, 1) NOT NULL,
    [ValidationOEAlgorithmId] BIGINT          NOT NULL,
    [ValidationId]             BIGINT          NOT NULL,
    [Requirement]           NVARCHAR (2048) NOT NULL,
    CONSTRAINT [PK_Prerequisites] PRIMARY KEY CLUSTERED ([PrerequisiteId] ASC),
    CONSTRAINT [FK_Prerequisites_Validations] FOREIGN KEY ([ValidationId]) REFERENCES [dbo].[Validations] ([ValidationId]),
    CONSTRAINT [FK_Prerequisites_ValidationOEAlgorithms] FOREIGN KEY ([ValidationOEAlgorithmId]) REFERENCES [dbo].[ValidationOEAlgorithms] ([ValidationOEAlgorithmId])
);

