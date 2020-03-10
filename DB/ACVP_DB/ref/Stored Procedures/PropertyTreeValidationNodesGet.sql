CREATE PROCEDURE [ref].[PropertyTreeValidationNodesGet]

	@AlgorithmId bigint

AS

SET NOCOUNT ON;


WITH Foo_CTE AS (
select  P.id as PropertyId, C.id AS ProtocolID, X.id AS LinkID, C.capability_field_name, C.capability_info, P.capability_type, P.name, P.default_value
,CAST('\' + P.[name] AS varchar(MAX)) AS Lineage, P.order_index, P.in_certificate
from ref.CRYPTO_ALGORITHM_PROPERTY_PROTOCOL_ACV_VERSION_LINK X
	INNER JOIN
	ref.CRYPTO_ALGORITHM_PROPERTY_PROTOCOL C ON C.id = X.protocol_id
										AND X.acv_version_id = 5
										AND C.capability_field_name is not null
										AND X.capability_parent_id IS NULL
	INNER JOIN
	ref.CRYPTO_ALGORITHM_PROPERTY P ON P.id = C.property_id
								AND P.algorithm_id = @AlgorithmId

UNION ALL

select P.id as PropertyId, C.id AS ProtocolID, X.id AS LinkID, C.capability_field_name, C.capability_info, P.capability_type, P.name, P.default_value
,cast(foo.Lineage + '\' + P.[name] AS varchar(MAX)) AS Lineage, P.order_index, P.in_certificate
from ref.CRYPTO_ALGORITHM_PROPERTY_PROTOCOL_ACV_VERSION_LINK X
	INNER JOIN
	ref.CRYPTO_ALGORITHM_PROPERTY_PROTOCOL C ON C.id = X.protocol_id
										AND X.acv_version_id = 5
										AND C.capability_field_name is not null
	INNER JOIN
	ref.CRYPTO_ALGORITHM_PROPERTY P ON P.id = C.property_id
								AND P.algorithm_id = @AlgorithmId
	INNER JOIN Foo_CTE foo ON foo.ProtocolID = X.capability_parent_id)

SELECT PropertyID, [name] AS PropertyName, CAST(LEN(Lineage) - LEN(REPLACE(Lineage, '\', '')) - 1 AS int) AS [Level], capability_type AS PropertyType, order_index AS OrderIndex
FROM Foo_CTE
ORDER BY Lineage
