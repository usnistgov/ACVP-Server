CREATE PROCEDURE [lcavp].[ValidationIDGet]

	 @Algorithm varchar(50)
	,@ValidationNumber int
	
AS
	SET NOCOUNT ON

	SELECT V.ValidationId
	FROM dbo.ValidationSources S
		INNER JOIN
		dbo.Validations V ON V.ValidationSourceId = S.ValidationSourceId
								AND S.Prefix = @Algorithm
								AND V.ValidationNumber = @ValidationNumber


