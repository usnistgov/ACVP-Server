

CREATE VIEW [csrc].[ImplementationTypes]
WITH SCHEMABINDING
AS

SELECT ImplementationTypeId, ImplementationTypeName
FROM dbo.ImplementationTypes


GO
CREATE UNIQUE CLUSTERED INDEX [PK_CSRC_ImplementationTypes]
    ON [csrc].[ImplementationTypes]([ImplementationTypeId] ASC);

