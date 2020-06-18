CREATE TABLE [dbo].[AlgorithmProperties] (
    [AlgorithmPropertyId]       BIGINT         IDENTITY (1, 1) NOT NULL,
    [AlgorithmId]               BIGINT         NOT NULL,
    [PropertyName]              NVARCHAR (128) NOT NULL,
    [ParentAlgorithmPropertyId] BIGINT         NULL,
    [AlgorithmPropertyTypeId]   TINYINT        NOT NULL,
    [DefaultValue]              NVARCHAR (128) NULL,
    [Historical]                BIT            CONSTRAINT [DF_AlgorithmProperties_Historical] DEFAULT ((0)) NOT NULL,
    [DisplayName]               NVARCHAR (128) NOT NULL,
    [InCertificate]             BIT            CONSTRAINT [DF_AlgorithmProperties_InCertificate] DEFAULT ((1)) NOT NULL,
    [OrderIndex]                INT            NULL,
    [IsRequired]                BIT            NULL,
    [UnitsLabel]                NVARCHAR (128) NULL,
    CONSTRAINT [PK_AlgorithmProperties] PRIMARY KEY CLUSTERED ([AlgorithmPropertyId] ASC),
    CONSTRAINT [FK_AlgorithmProperties_AlgorithmPropertyTypes] FOREIGN KEY ([AlgorithmPropertyTypeId]) REFERENCES [dbo].[AlgorithmPropertyTypes] ([AlgorithmPropertyTypeId])
);

