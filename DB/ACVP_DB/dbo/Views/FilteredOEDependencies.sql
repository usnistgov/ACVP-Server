CREATE VIEW [dbo].[FilteredOEDependencies]
WITH SCHEMABINDING
AS

SELECT OED.OEId, OED.DependencyId
FROM dbo.OEDependencies OED
    INNER JOIN
    dbo.OEs OE ON OE.OEId = OED.OEId
              AND OE.ITAR = 0


GO
CREATE UNIQUE CLUSTERED INDEX [PK_FilteredOEDependencies]
    ON [dbo].[FilteredOEDependencies]([OEId] ASC, [DependencyId] ASC);

