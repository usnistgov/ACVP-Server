CREATE PROCEDURE [dbo].[CapabilityInsert]

	 @ValidationOEAlgorithmId bigint
	,@AlgorithmPropertyId bigint
	,@Historical bit
	,@ParentCapabilityId bigint
	,@OrderIndex int
	,@StringValue nvarchar(512)
	,@NumberValue bigint
	,@BooleanValue bit

AS

SET NOCOUNT ON

INSERT INTO dbo.Capabilities(
		 ValidationOEAlgorithmId
		,AlgorithmPropertyId
		,Historical
		,ParentCapabilityId
		,OrderIndex
		,StringValue
		,NumberValue
		,BooleanValue
		)
VALUES (
		 @ValidationOEAlgorithmId
		,@AlgorithmPropertyId
		,@Historical
		,@ParentCapabilityId
		,@OrderIndex
		,@StringValue
		,@NumberValue
		,@BooleanValue
)

SELECT CAST(SCOPE_IDENTITY() AS bigint) AS CapabilityId