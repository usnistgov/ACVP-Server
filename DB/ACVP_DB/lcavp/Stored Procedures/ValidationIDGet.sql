CREATE PROCEDURE [lcavp].[ValidationIDGet]

	 @Algorithm varchar(50)
	,@ValidationNumber int
	
AS
	SET NOCOUNT ON

	SELECT V.ValidationId
	FROM val.VALIDATION_SOURCE S
		INNER JOIN
		dbo.Validations V ON V.ValidationSourceId = S.id
								AND S.prefix = @Algorithm
								AND V.ValidationNumber = @ValidationNumber


