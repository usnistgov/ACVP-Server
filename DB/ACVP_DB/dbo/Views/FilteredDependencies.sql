CREATE VIEW [dbo].[FilteredDependencies]
WITH SCHEMABINDING
AS

SELECT DependencyId, DependencyType, [Name], [Description]
FROM dbo.Dependencies
WHERE EffectiveITAR = 0


GO
CREATE UNIQUE CLUSTERED INDEX [PK_FilteredDependencies]
    ON [dbo].[FilteredDependencies]([DependencyId] ASC);

