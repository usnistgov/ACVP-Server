

CREATE VIEW [csrc].[Dependencies]
WITH SCHEMABINDING
AS

SELECT D.DependencyId, D.DependencyType, D.[Name], D.[Description]
FROM dbo.Dependencies D
WHERE NOT EXISTS (SELECT NULL
                  FROM dbo.OEDependencies OED
                        INNER JOIN
                        dbo.OEs OE ON OE.OEId = OED.OEId
                                  AND D.DependencyId = OED.DependencyId
                                  AND OE.ITAR = 1)
  OR (EXISTS (SELECT NULL
              FROM dbo.OEDependencies OED
                    INNER JOIN
                   dbo.OEs OE ON OE.OEId = OED.OEId
                             AND D.DependencyId = OED.DependencyId
                             AND OE.ITAR = 1)
      AND EXISTS (SELECT NULL
                    FROM dbo.OEDependencies OED
                        INNER JOIN
                        dbo.OEs OE ON OE.OEId = OED.OEId
                                     AND D.DependencyId = OED.DependencyId
                                     AND OE.ITAR = 0)
    )



GO
CREATE UNIQUE CLUSTERED INDEX [PK_CSRC_Dependencies]
    ON [csrc].[Dependencies]([DependencyId] ASC);

