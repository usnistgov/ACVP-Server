

CREATE VIEW [csrc].[Dependencies]
WITH SCHEMABINDING
AS

SELECT DependencyId, DependencyType, [Name], [Description]
FROM dbo.Dependencies



GO
CREATE UNIQUE CLUSTERED INDEX [PK_CSRC_Dependencies]
    ON [csrc].[Dependencies]([DependencyId] ASC);

