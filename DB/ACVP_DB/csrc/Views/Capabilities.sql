

CREATE VIEW [csrc].[Capabilities]
WITH SCHEMABINDING
AS

SELECT	 C.CapabilityId
		,C.ValidationOEAlgorithmId
		,C.AlgorithmPropertyId
		,C.Historical
		,C.ParentCapabilityId
		,C.OrderIndex
		,C.StringValue
		,C.NumberValue
		,C.BooleanValue
FROM dbo.Capabilities C
	INNER JOIN
	dbo.ValidationOEAlgorithms VOA ON VOA.ValidationOEAlgorithmId = C.ValidationOEAlgorithmId
	INNER JOIN
	dbo.Validations V ON V.ValidationId = VOA.ValidationId
	INNER JOIN
	dbo.Implementations I ON I.ImplementationId = V.ImplementationId
						 AND I.ITAR = 0


GO
CREATE UNIQUE CLUSTERED INDEX [PK_CSRC_Capabilities]
    ON [csrc].[Capabilities]([CapabilityId] ASC);

