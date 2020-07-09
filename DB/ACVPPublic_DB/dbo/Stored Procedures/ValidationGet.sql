CREATE PROCEDURE [dbo].[ValidationGet]
	
	@ValidationId bigint

AS

SET NOCOUNT ON;

SELECT	 S.Prefix
		,V.ValidationNumber
		,V.ImplementationId
FROM dbo.Validations V
	INNER JOIN
	dbo.ValidationSources S ON S.ValidationSourceId = V.ValidationSourceId
							AND V.ValidationId = @ValidationId
