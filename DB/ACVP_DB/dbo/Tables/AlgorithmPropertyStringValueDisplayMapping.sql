CREATE TABLE [dbo].[AlgorithmPropertyStringValueDisplayMapping] (
    [AlgorithmPropertyStringValueDisplayMappingId] BIGINT         IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [AlgorithmPropertyId]                          BIGINT         NOT NULL,
    [StringValue]                                  NVARCHAR (512) NOT NULL,
    [DisplayValue]                                 NVARCHAR (512) NOT NULL,
    CONSTRAINT [PK_AlgorithmPropertyStringValueDisplayMapping] PRIMARY KEY CLUSTERED ([AlgorithmPropertyStringValueDisplayMappingId] ASC), 
    CONSTRAINT [FK_AlgorithmPropertyStringValueDisplayMapping_AlgorithmProperties] FOREIGN KEY ([AlgorithmPropertyId]) REFERENCES [AlgorithmProperties]([AlgorithmPropertyId])
);

