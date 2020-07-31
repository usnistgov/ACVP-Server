
CREATE VIEW [csrc].[DependencyAttributes]
WITH SCHEMABINDING
AS

SELECT DA.DependencyAttributeId, DA.DependencyId, DA.[Name], DA.[Value]
FROM dbo.DependencyAttributes DA
WHERE NOT EXISTS (SELECT NULL
                  FROM dbo.OEDependencies OED
                        INNER JOIN
                        dbo.OEs OE ON OE.OEId = OED.OEId
                                  AND DA.DependencyId = OED.DependencyId
                                  AND OE.ITAR = 1)
  OR (EXISTS (SELECT NULL
              FROM dbo.OEDependencies OED
                    INNER JOIN
                   dbo.OEs OE ON OE.OEId = OED.OEId
                             AND DA.DependencyId = OED.DependencyId
                             AND OE.ITAR = 1)
      AND EXISTS (SELECT NULL
                    FROM dbo.OEDependencies OED
                        INNER JOIN
                        dbo.OEs OE ON OE.OEId = OED.OEId
                                     AND DA.DependencyId = OED.DependencyId
                                     AND OE.ITAR = 0)
    )


GO
CREATE UNIQUE CLUSTERED INDEX [PK_CSRC_DependencyAttributes]
    ON [csrc].[DependencyAttributes]([DependencyAttributeId] ASC);

