

CREATE VIEW [csrc].[AlgorithmProperties]
WITH SCHEMABINDING
AS

SELECT AlgorithmPropertyId, AlgorithmId, PropertyName, ParentAlgorithmPropertyId, AlgorithmPropertyTypeId, DefaultValue, Historical, DisplayName, InCertificate, OrderIndex, IsRequired, UnitsLabel
FROM dbo.AlgorithmProperties


GO
CREATE UNIQUE CLUSTERED INDEX [PK_CSRC_AlgorithmProperties]
    ON [csrc].[AlgorithmProperties]([AlgorithmPropertyId] ASC);

