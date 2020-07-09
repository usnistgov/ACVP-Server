CREATE TABLE [dbo].[AlgorithmProperties] (
    [AlgorithmPropertyId]       BIGINT         IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [AlgorithmId]               BIGINT         NOT NULL,
    [PropertyName]              NVARCHAR (128) NOT NULL,
    [ParentAlgorithmPropertyId] BIGINT         NULL,
    [AlgorithmPropertyTypeId]   TINYINT        NOT NULL,
    [DefaultValue]              NVARCHAR (128) NULL,
    [Historical]                BIT            NOT NULL,
    [DisplayName]               NVARCHAR (128) NOT NULL,
    [InCertificate]             BIT            NOT NULL,
    [OrderIndex]                INT            NULL,
    [IsRequired]                BIT            NULL,
    [UnitsLabel]                NVARCHAR (128) NULL,
    CONSTRAINT [PK_AlgorithmProperties] PRIMARY KEY CLUSTERED ([AlgorithmPropertyId] ASC)
);

