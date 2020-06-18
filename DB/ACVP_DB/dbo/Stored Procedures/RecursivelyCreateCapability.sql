CREATE PROCEDURE [dbo].[RecursivelyCreateCapability]

	 @OriginalCapabilityID bigint
	,@NewVOAID bigint
	,@NewParentCapabilityID bigint

AS

SET NOCOUNT ON

-- Clone this capability

DECLARE @NewCapabilityID bigint

INSERT INTO dbo.Capabilities (ValidationOEAlgorithmId, AlgorithmPropertyId, Historical, ParentCapabilityId, OrderIndex, StringValue, NumberValue, BooleanValue)
SELECT top 1 @NewVOAID, algorithm_property, historical, @NewParentCapabilityID, order_index, string_value, number_value, boolean_value
FROM val.VALIDATION_CAPABILITY
WHERE id = @OriginalCapabilityID

SET @NewCapabilityID = SCOPE_IDENTITY()

-- Get any children of the original capability
DECLARE @ChildCapabilities TABLE (Iterator int identity, CapabilityID bigint)

INSERT INTO @ChildCapabilities (CapabilityID)
SELECT id
FROM val.VALIDATION_CAPABILITY
WHERE parent = @OriginalCapabilityID

DECLARE @ChildIterator int
SET @ChildIterator = 1
DECLARE @ChildCount int
DECLARE @ChildCapabilityID bigint

SELECT @ChildCount = count(*) from @ChildCapabilities

IF (@ChildCount <> 0)
BEGIN
	-- Iterate over the children, calling this recursive procedure
	WHILE (@ChildIterator <= @ChildCount)
		BEGIN
			-- Get the original child capability id
			SELECT top 1 @ChildCapabilityID = CapabilityID FROM @ChildCapabilities WHERE Iterator = @ChildIterator

			-- Make the recursive call
			exec dbo.RecursivelyCreateCapability @ChildCapabilityID, @NewVOAID, @NewCapabilityID

			SET @ChildIterator = @ChildIterator + 1
		END
END