CREATE PROCEDURE [lcavp].[VendorIDGet]

	 @Algorithm varchar(50)
	,@ValidationNumber int

AS
	SET NOCOUNT ON

	SELECT P.vendor_id AS VendorID
	FROM val.VALIDATION_SOURCE S
		INNER JOIN
		val.VALIDATION_RECORD VR ON VR.source_id = S.id
								AND S.prefix = @Algorithm
								AND VR.validation_id = @ValidationNumber
		INNER JOIN
		val.PRODUCT_INFORMATION P ON P.id = VR.product_information_id


