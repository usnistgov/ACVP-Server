
CREATE PROCEDURE [dbo].[ValidationGetById]
	
	@ValidationId bigint

AS
BEGIN
	SET NOCOUNT ON;

    SELECT	 V.ValidationId
			,V.ImplementationId
			,CONCAT(vs.prefix, V.ValidationNumber) AS ValidationLabel
			,I.ImplementationName
			,V.CreatedOn
			,V.LastUpdated
			,I.VendorId
	FROM dbo.Validations V
		INNER JOIN
		dbo.ValidationSources vs on V.ValidationSourceId = vs.ValidationSourceId
								AND V.ValidationId = @ValidationId
		INNER JOIN
		dbo.Implementations I ON I.ImplementationId = V.ImplementationId
END