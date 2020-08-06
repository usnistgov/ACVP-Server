CREATE VIEW [dbo].[FilteredValidations]
WITH SCHEMABINDING
AS

SELECT	 V.ValidationId
		,V.ImplementationId
		,V.ValidationSourceId
		,V.ValidationNumber
		,V.CreatedOn
		,V.LastUpdated
FROM dbo.Validations V
	INNER JOIN
	dbo.Implementations I ON I.ImplementationId = V.ImplementationId
						 AND I.ITAR = 0


GO
CREATE UNIQUE CLUSTERED INDEX [PK_FilteredValidations]
    ON [dbo].[FilteredValidations]([ValidationId] ASC);

