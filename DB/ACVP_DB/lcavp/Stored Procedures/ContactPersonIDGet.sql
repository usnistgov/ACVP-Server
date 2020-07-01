CREATE PROCEDURE [lcavp].[ContactPersonIDGet]

	 @Algorithm varchar(50)
	,@ValidationNumber int
	,@OrderIndex int

AS
	SET NOCOUNT ON

	SELECT C.PersonId
	FROM dbo.ValidationSources S
		INNER JOIN
		dbo.Validations V ON V.ValidationSourceId = S.ValidationSourceId
								AND S.Prefix = @Algorithm
								AND V.ValidationNumber = @ValidationNumber
		INNER JOIN
		dbo.ImplementationContacts C ON C.ImplementationId = V.ImplementationId
	ORDER BY C.OrderIndex
	OFFSET @OrderIndex - 1 ROWS
	FETCH NEXT 1 ROW ONLY
