CREATE PROCEDURE [val].[OEsGet]

	@PageSize bigint,
	@PageNumber bigint

AS

SET NOCOUNT ON

SELECT OE.id,
		OE.name
	   --TotalCount
FROM [val].[VALIDATION_OE] AS OE
--CROSS APPLY (SELECT COUNT(*) TotalCount
--FROM [val].[VALIDATION_OE] ) [Count]
ORDER BY OE.id
OFFSET @PageSize * (@PageNumber - 1) ROWS
FETCH NEXT @PageSize ROWS ONLY;