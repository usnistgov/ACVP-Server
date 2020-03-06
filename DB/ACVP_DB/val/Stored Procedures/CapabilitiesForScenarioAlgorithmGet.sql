CREATE PROCEDURE [val].[CapabilitiesForScenarioAlgorithmGet]

	@ScenarioAlgorithmId bigint

AS
	
SET NOCOUNT ON

SELECT	 C.id AS CapabilityId
		,C.algorithm_property AS PropertyId
		,C.[root] AS RootCapabilityId
		,C.[level] AS [Level]
		,C.parent AS ParentCapabilityId
		,ISNULL(C.order_index, 0) AS OrderIndex
		,C.string_value AS StringValue
		,C.number_value AS NumberValue
		,C.boolean_value AS BooleanValue
		,P.capability_type AS CapabilityType
FROM val.VALIDATION_CAPABILITY C
	INNER JOIN
	ref.CRYPTO_ALGORITHM_PROPERTY P ON P.id = C.algorithm_property
									AND C.scenario_algorithm_id = @ScenarioAlgorithmId
