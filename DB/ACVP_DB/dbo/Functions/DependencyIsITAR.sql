
CREATE FUNCTION dbo.DependencyIsITAR (@DependencyId bigint)
RETURNS bit
WITH SCHEMABINDING

AS

BEGIN

RETURN CASE
		WHEN NOT EXISTS (SELECT NULL
                  FROM dbo.OEDependencies OED
                        INNER JOIN
                        dbo.OEs OE ON OE.OEId = OED.OEId
                                  AND OED.DependencyId = @DependencyId
                                  AND OE.ITAR = 1) THEN 0
		WHEN EXISTS (SELECT NULL
              FROM dbo.OEDependencies OED
                    INNER JOIN
                   dbo.OEs OE ON OE.OEId = OED.OEId
                             AND OED.DependencyId = @DependencyId
                             AND OE.ITAR = 1)
				AND EXISTS (SELECT NULL
                    FROM dbo.OEDependencies OED
                        INNER JOIN
                        dbo.OEs OE ON OE.OEId = OED.OEId
                                     AND OED.DependencyId = @DependencyId
                                     AND OE.ITAR = 0) THEN 0
		ELSE 1
		END
END