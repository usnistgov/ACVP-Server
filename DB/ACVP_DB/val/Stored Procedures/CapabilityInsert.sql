CREATE PROCEDURE [val].[CapabilityInsert]
	 
	 @ScenarioAlgorithmId bigint
	,@PropertyId bigint
	,@RootCapabilityId bigint
	,@ParentCapabilityId bigint
	,@Level int
	,@Type int
	,@OrderIndex int
	,@Historical bit
	,@StringValue nvarchar(512)
	,@NumberValue bigint
	,@BooleanValue bit

AS

SET NOCOUNT ON

INSERT INTO val.VALIDATION_CAPABILITY (
		 scenario_algorithm_id
		,algorithm_property
		,[root]
		,parent
		,[level]
		,[type]
		,order_index
		,historical
		,string_value
		,number_value
		,boolean_value)
VALUES (
		 @ScenarioAlgorithmId
		,@PropertyId
		,@RootCapabilityId
		,@ParentCapabilityId
		,@Level
		,@Type
		,@OrderIndex
		,@Historical
		,@StringValue
		,@NumberValue
		,@BooleanValue
)

SELECT CAST(SCOPE_IDENTITY() AS bigint) AS CapabilityId

