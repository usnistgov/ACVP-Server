
CREATE VIEW [csrc].[Algorithms]
WITH SCHEMABINDING
AS

SELECT AlgorithmId, [Name], Mode, Revision, Alias, DisplayName, Historical, PrimitiveId
FROM dbo.Algorithms



GO
CREATE UNIQUE CLUSTERED INDEX [PK_CSRC_Algorithms]
    ON [csrc].[Algorithms]([AlgorithmId] ASC);

