CREATE PROCEDURE [dbo].[PropertyTreeValidationNodesGet]

	@AlgorithmId bigint

AS

SET NOCOUNT ON;

WITH Foo_CTE AS (
select  P.AlgorithmPropertyId as PropertyID, P.AlgorithmPropertyTypeId, P.PropertyName, P.DefaultValue, P.DisplayName
,CAST('\' + CAST(P.OrderIndex AS varchar(10)) AS varchar(4000)) AS Lineage, P.OrderIndex, P.InCertificate
from dbo.AlgorithmProperties P
WHERE P.AlgorithmId = @AlgorithmId
AND P.ParentAlgorithmPropertyId is null

UNION ALL

select P.AlgorithmPropertyId as PropertyID, P.AlgorithmPropertyTypeId, P.PropertyName, P.DefaultValue, P.DisplayName
,CAST(foo.Lineage + '\' + CAST(P.OrderIndex AS varchar(10)) AS varchar(4000)) AS Lineage, P.OrderIndex, P.InCertificate
from dbo.AlgorithmProperties P
	INNER JOIN 
	Foo_CTE foo ON foo.PropertyID = P.ParentAlgorithmPropertyId)

SELECT PropertyID, PropertyName, CAST(LEN(Lineage) - LEN(REPLACE(Lineage, '\', '')) - 1 AS int) AS [Level], AlgorithmPropertyTypeId, OrderIndex
FROM Foo_CTE
ORDER BY Lineage