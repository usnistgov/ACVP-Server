CREATE PROCEDURE [acvp].[VectorSetStatusGet]
    @VsID BIGINT
AS

SELECT [status] AS [Status]
--FROM [acvp].[VectorSet]
FROM [acvp].[VECTOR_SET]
WHERE id = @VsID