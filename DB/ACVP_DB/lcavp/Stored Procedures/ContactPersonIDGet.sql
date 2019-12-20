CREATE PROCEDURE [lcavp].[ContactPersonIDGet]

	 @Algorithm varchar(50)
	,@ValidationNumber int
	,@OrderIndex int

AS
	SET NOCOUNT ON

	SELECT VC.person_id AS PersonID
	FROM val.VALIDATION_SOURCE S
		INNER JOIN
		val.VALIDATION_RECORD VR ON VR.source_id = S.id
								AND S.prefix = @Algorithm
								AND VR.validation_id = @ValidationNumber
		INNER JOIN
		val.VALIDATION_CONTACT VC ON VC.product_information_id = VR.product_information_id
								 --AND VC.order_index = @OrderIndex
	ORDER BY VC.order_index
	OFFSET @OrderIndex - 1 ROWS
	FETCH NEXT 1 ROW ONLY
