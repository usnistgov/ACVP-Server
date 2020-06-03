CREATE PROCEDURE [lcavp].[ContactPersonIDGet]

	 @Algorithm varchar(50)
	,@ValidationNumber int
	,@OrderIndex int

AS
	SET NOCOUNT ON

	SELECT VC.person_id AS PersonID
	FROM val.VALIDATION_SOURCE S
		INNER JOIN
		dbo.Validations V ON V.ValidationSourceId = S.id
								AND S.prefix = @Algorithm
								AND V.ValidationNumber = @ValidationNumber
		INNER JOIN
		val.VALIDATION_CONTACT VC ON VC.product_information_id = V.ImplementationId
	ORDER BY VC.order_index
	OFFSET @OrderIndex - 1 ROWS
	FETCH NEXT 1 ROW ONLY
