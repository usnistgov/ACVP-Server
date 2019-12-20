CREATE PROCEDURE [lcavp].[ContactIDGet]

	 @Algorithm varchar(50)
	,@ValidationNumber int
	,@OrderIndex int

AS
	SET NOCOUNT ON

	SELECT VC.id AS ContactID
	FROM val.VALIDATION_SOURCE S
		INNER JOIN
		val.VALIDATION_RECORD VR ON VR.source_id = S.id
								AND S.prefix = @Algorithm
								AND VR.validation_id = @ValidationNumber
		INNER JOIN
		val.VALIDATION_CONTACT VC ON VC.product_information_id = VR.product_information_id
								 AND VC.order_index = @OrderIndex


