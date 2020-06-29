
CREATE VIEW [csrc].[ValidationSources]
WITH SCHEMABINDING
AS

SELECT ValidationSourceId, [Name], Prefix
FROM dbo.ValidationSources

GO
CREATE UNIQUE CLUSTERED INDEX [PK_CSRC_ValidationSources]
    ON [csrc].[ValidationSources]([ValidationSourceId] ASC);

