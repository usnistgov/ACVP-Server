CREATE PROCEDURE [lcavp].[ModuleIDGet]

	 @Algorithm varchar(50)
	,@ValidationNumber int

AS
	SET NOCOUNT ON

	SELECT VR.product_information_id AS ModuleID
	FROM val.VALIDATION_SOURCE S
		INNER JOIN
		val.VALIDATION_RECORD VR ON VR.source_id = S.id
								AND S.prefix = @Algorithm
								AND VR.validation_id = @ValidationNumber


