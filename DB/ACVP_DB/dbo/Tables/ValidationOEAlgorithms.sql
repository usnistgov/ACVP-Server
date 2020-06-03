CREATE TABLE [dbo].[ValidationOEAlgorithms]
(
	[ValidationOEAlgorithmId] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [ValidationId] BIGINT NOT NULL, 
    [OEId] BIGINT NOT NULL, 
    [AlgorithmId] BIGINT NOT NULL, 
    [VectorSetId] BIGINT NULL,
    [Active] BIT NOT NULL DEFAULT 1,
    [CreatedOn] DATETIME2 NOT NULL DEFAULT CURRENT_TIMESTAMP, 
    [InactiveOn] DATETIME2 NULL
)
