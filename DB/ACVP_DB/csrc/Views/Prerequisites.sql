
CREATE VIEW [csrc].[Prerequisites]
WITH SCHEMABINDING
AS

SELECT	 Q.PrerequisiteId
		,Q.ValidationOEAlgorithmId
		,Q.ValidationId
		,Q.Requirement
FROM dbo.Prerequisites Q
	INNER JOIN
	dbo.Validations V ON V.ValidationId = Q.ValidationId
	INNER JOIN
	dbo.Implementations I ON I.ImplementationId = V.ImplementationId
						 AND I.ITAR = 0


GO
CREATE UNIQUE CLUSTERED INDEX [PK_CSRC_Prerequisites]
    ON [csrc].[Prerequisites]([PrerequisiteId] ASC);

