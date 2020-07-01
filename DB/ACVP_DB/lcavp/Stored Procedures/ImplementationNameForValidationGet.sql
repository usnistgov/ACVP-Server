CREATE PROCEDURE [lcavp].[ImplementationNameForValidationGet]

	 @Algorithm varchar(50)
	,@ValidationNumber int
	
AS
	SET NOCOUNT ON

	SELECT I.ImplementationName
	FROM dbo.ValidationSources S
		INNER JOIN
		dbo.Validations V ON V.ValidationSourceId = S.ValidationSourceId
								AND S.Prefix = @Algorithm
								AND V.ValidationNumber = @ValidationNumber
		INNER JOIN
		dbo.Implementations I ON I.ImplementationId = V.ImplementationId
