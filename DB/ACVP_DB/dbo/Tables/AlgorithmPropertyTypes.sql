CREATE TABLE [dbo].[AlgorithmPropertyTypes] (
    [AlgorithmPropertyTypeId] TINYINT       NOT NULL,
    [TypeName]                VARCHAR (128) NOT NULL,
    CONSTRAINT [PK_AlgorithmPropertyTypes] PRIMARY KEY CLUSTERED ([AlgorithmPropertyTypeId] ASC)
);

