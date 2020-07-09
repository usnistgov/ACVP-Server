CREATE TABLE [dbo].[ValidationOEAlgorithms] (
    [ValidationOEAlgorithmId] BIGINT        NOT NULL,
    [ValidationId]            BIGINT        NOT NULL,
    [OEId]                    BIGINT        NOT NULL,
    [AlgorithmId]             BIGINT        NOT NULL,
    [VectorSetId]             BIGINT        NULL,
    [Active]                  BIT           NOT NULL,
    [CreatedOn]               DATETIME2 (7) NOT NULL,
    [InactiveOn]              DATETIME2 (7) NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [PK_CSRC_ValidationOEAlgorithms]
    ON [dbo].[ValidationOEAlgorithms]([ValidationOEAlgorithmId] ASC);

