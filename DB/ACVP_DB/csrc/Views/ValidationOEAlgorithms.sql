CREATE VIEW [csrc].[ValidationOEAlgorithms]
WITH SCHEMABINDING
AS

SELECT	 VOA.ValidationOEAlgorithmId
		,VOA.ValidationId
		,VOA.OEId
		,VOA.AlgorithmId
		,VOA.VectorSetId
		,VOA.Active
		,VOA.CreatedOn
		,VOA.InactiveOn
FROM dbo.ValidationOEAlgorithms VOA
	INNER JOIN
	dbo.Validations V ON V.ValidationId = VOA.ValidationId
	INNER JOIN
	dbo.Implementations I ON I.ImplementationId = V.ImplementationId
						 AND I.ITAR = 0


GO
CREATE UNIQUE CLUSTERED INDEX [PK_CSRC_ValidationOEAlgorithms]
    ON [csrc].[ValidationOEAlgorithms]([ValidationOEAlgorithmId] ASC);

