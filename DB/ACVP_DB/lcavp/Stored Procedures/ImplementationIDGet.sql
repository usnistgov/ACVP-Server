CREATE PROCEDURE [lcavp].[ImplementationIDGet]

	 @Algorithm varchar(50)
	,@ValidationNumber int

AS
	SET NOCOUNT ON

	SELECT V.ImplementationId
	FROM val.VALIDATION_SOURCE S
		INNER JOIN
		dbo.Validations V ON V.ValidationSourceId = S.id
								AND V.ValidationNumber = @ValidationNumber


