CREATE TABLE [dbo].[Capabilities] (
    [CapabilityId]            BIGINT         IDENTITY (1, 1) NOT NULL,
    [ValidationOEAlgorithmId] BIGINT         NOT NULL,
    [AlgorithmPropertyId]     BIGINT         NOT NULL,
    [Historical]              BIT            CONSTRAINT [DF_Capabilities_Historical] DEFAULT ((0)) NOT NULL,
    [ParentCapabilityId]      BIGINT         NULL,
    [OrderIndex]              INT            NULL,
    [StringValue]             NVARCHAR (512) NULL,
    [NumberValue]             BIGINT         NULL,
    [BooleanValue]            BIT            NULL,
    CONSTRAINT [PK_Capabilities] PRIMARY KEY CLUSTERED ([CapabilityId] ASC), 
    CONSTRAINT [FK_Capabilities_ValidationOEAlgorithms] FOREIGN KEY ([ValidationOEAlgorithmId]) REFERENCES [dbo].[ValidationOEAlgorithms]([ValidationOEAlgorithmId]), 
    CONSTRAINT [FK_Capabilities_AlgorithmProperties] FOREIGN KEY ([AlgorithmPropertyId]) REFERENCES [dbo].[AlgorithmProperties]([AlgorithmPropertyId])
);


