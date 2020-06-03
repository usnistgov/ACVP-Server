CREATE TABLE [dbo].[Capabilities]
(
	[CapabilityId] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [ValidationOEAlgorithmId] BIGINT NOT NULL, 
    [AlgorithmPropertyId] BIGINT NOT NULL, 
    [Historical] BIT NOT NULL DEFAULT 0, 
    [ParentCapabilityId] BIGINT NULL, 
    [OrderIndex] INT NULL, 
    [StringValue] NVARCHAR(512) NULL, 
    [NumberValue] BIGINT NULL, 
    [BooleanValue] BIT NULL
)
