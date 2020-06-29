CREATE VIEW [csrc].[AlgorithmPropertyStringValueDisplayMapping]
WITH SCHEMABINDING
AS

SELECT AlgorithmPropertyStringValueDisplayMappingId, AlgorithmPropertyId, StringValue, DisplayValue
FROM dbo.AlgorithmPropertyStringValueDisplayMapping


GO
CREATE UNIQUE CLUSTERED INDEX [PK_CSRC_AlgorithmPropertyStringValueDisplayMapping]
    ON [csrc].[AlgorithmPropertyStringValueDisplayMapping]([AlgorithmPropertyStringValueDisplayMappingId] ASC);

