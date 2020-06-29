CREATE VIEW [csrc].[OEDependencies]
WITH SCHEMABINDING
AS

SELECT OEId, DependencyId
FROM dbo.OEDependencies


GO
CREATE UNIQUE CLUSTERED INDEX [PK_CSRC_OEDependencies]
    ON [csrc].[OEDependencies]([OEId] ASC, [DependencyId] ASC);

