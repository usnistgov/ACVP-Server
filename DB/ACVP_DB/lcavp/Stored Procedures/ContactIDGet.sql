CREATE PROCEDURE [lcavp].[ContactIDGet]

	 @Algorithm varchar(50)
	,@ValidationNumber int
	,@OrderIndex int

AS
	SET NOCOUNT ON

	SELECT VC.id AS ContactID
	FROM val.VALIDATION_SOURCE S
		INNER JOIN
		dbo.Validations V ON V.ValidationSourceId = S.id
								AND S.prefix = @Algorithm
								AND V.ValidationNumber = @ValidationNumber
		INNER JOIN
		val.VALIDATION_CONTACT VC ON VC.product_information_id = V.ImplementationId
								 AND VC.order_index = @OrderIndex


