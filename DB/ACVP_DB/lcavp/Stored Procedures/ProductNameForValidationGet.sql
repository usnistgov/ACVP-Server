CREATE PROCEDURE [lcavp].[ProductNameForValidationGet]

	 @Algorithm varchar(50)
	,@ValidationNumber int
	
AS
	SET NOCOUNT ON

	SELECT P.module_name AS ProductName
	FROM val.VALIDATION_SOURCE S
		INNER JOIN
		dbo.Validations V ON V.ValidationSourceId = S.id
								AND S.prefix = @Algorithm
								AND V.ValidationNumber = @ValidationNumber
		INNER JOIN
		val.PRODUCT_INFORMATION P ON P.id = V.ImplementationId
