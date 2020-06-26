CREATE PROCEDURE [dbo].[CapabilitiesGet]
	 
	@ValidationOEAlgorithmId bigint

AS

SET NOCOUNT ON

SELECT	 C.CapabilityId
		,C.AlgorithmPropertyId
		,C.ParentCapabilityId
		,C.Historical AS HistoricalCapability
		,ISNULL(C.OrderIndex, 0) AS CapabilityOrderIndex
		,C.BooleanValue
		,ISNULL(M.DisplayValue, C.StringValue) AS StringDisplayValue
		,C.NumberValue
		,AP.DisplayName
		,AP.AlgorithmPropertyTypeId
		,AP.Historical AS HistoricalProperty
		,AP.IsRequired
		,AP.UnitsLabel
		,ISNULL(AP.OrderIndex, 0) AS PropertyOrderIndex
		,A.Historical AS HistoricalAlgorithm
FROM dbo.Capabilities C
	INNER JOIN
	dbo.AlgorithmProperties AP ON AP.AlgorithmPropertyId = C.AlgorithmPropertyId
							  AND C.ValidationOEAlgorithmId = @ValidationOEAlgorithmId
	INNER JOIN
	dbo.Algorithms A ON A.AlgorithmId = AP.AlgorithmId
	LEFT OUTER JOIN
	dbo.AlgorithmPropertyStringValueDisplayMapping M ON M.AlgorithmPropertyId = C.AlgorithmPropertyId
													AND M.StringValue = C.StringValue
