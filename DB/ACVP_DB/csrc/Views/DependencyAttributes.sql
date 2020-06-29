
CREATE VIEW [csrc].[DependencyAttributes]
WITH SCHEMABINDING
AS

SELECT DependencyAttributeId, DependencyId, [Name], [Value]
FROM dbo.DependencyAttributes


GO
CREATE UNIQUE CLUSTERED INDEX [PK_CSRC_DependencyAttributes]
    ON [csrc].[DependencyAttributes]([DependencyAttributeId] ASC);

