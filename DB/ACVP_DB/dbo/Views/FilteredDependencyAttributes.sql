
CREATE VIEW [dbo].[FilteredDependencyAttributes]
WITH SCHEMABINDING
AS

SELECT DA.DependencyAttributeId, DA.DependencyId, DA.[Name], DA.[Value]
FROM dbo.DependencyAttributes DA
    INNER JOIN
    dbo.Dependencies D ON D.DependencyId = DA.DependencyId
                      AND D.EffectiveITAR = 0


GO
CREATE UNIQUE CLUSTERED INDEX [PK_FilteredDependencyAttributes]
    ON [dbo].[FilteredDependencyAttributes]([DependencyAttributeId] ASC);

