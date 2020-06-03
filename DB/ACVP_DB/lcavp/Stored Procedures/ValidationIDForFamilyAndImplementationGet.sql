
CREATE PROCEDURE [lcavp].[ValidationIDForFamilyAndImplementationGet]

	 @ImplementationId bigint
	,@Algorithm varchar(50) 
	
AS
	SET NOCOUNT ON

	SELECT V.ValidationId
	FROM dbo.Validations V
		INNER JOIN
		val.VALIDATION_SOURCE S ON S.id = V.ValidationSourceId
								AND V.ImplementationId = @ImplementationId
								AND S.prefix = @Algorithm

