
CREATE PROCEDURE [lcavp].[ValidationIDForFamilyAndImplementationGet]

	 @ImplementationId bigint
	,@Algorithm varchar(50) 
	
AS
	SET NOCOUNT ON

	SELECT V.ValidationId
	FROM dbo.Validations V
		INNER JOIN
		dbo.ValidationSources S ON S.ValidationSourceId = V.ValidationSourceId
								AND V.ImplementationId = @ImplementationId
								AND S.prefix = @Algorithm

