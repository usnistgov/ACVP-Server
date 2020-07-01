CREATE PROCEDURE [lcavp].[ImplementationIDGet]

	 @Algorithm varchar(50)
	,@ValidationNumber int

AS
	SET NOCOUNT ON

	SELECT V.ImplementationId
	FROM dbo.ValidationSources S
		INNER JOIN
		dbo.Validations V ON V.ValidationSourceId = S.ValidationSourceId
						 AND V.ValidationNumber = @ValidationNumber


