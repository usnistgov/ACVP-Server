CREATE PROCEDURE [val].[OEFilteredListGet]

	 @Filter nvarchar(MAX)
	,@Limit bigint
	,@Offset bigint
	,@ORdelimiter varchar(10)
	,@ANDdelimiter varchar(10)

AS

SET NOCOUNT ON

DECLARE @DefaultRowLimit bigint = 20

DECLARE @MatchingIDs TABLE (id int)

-- Branch on whether or not there is a filter
IF (@Filter IS NULL)
	BEGIN
		-- No filter simply means all are potentially returned
		INSERT INTO @MatchingIDs (id)
		SELECT id
		FROM val.VALIDATION_OE
	END
ELSE
	BEGIN
		-- Do nasty things to apply the filters
		DECLARE @ANDMatchingIDs TABLE (id int)

		-- Split the query into the OR lines
		DECLARE @ORs TABLE (id int, clause nvarchar(1000))

		INSERT into @ORs (id, clause)
		SELECT ValueID, value FROM dbo.DelimitedListToTable(@Filter, @ORdelimiter)


		DECLARE @OrClause varchar(1000)

		DECLARE @ANDSegments TABLE (id int, segmentText nvarchar(1000))

		DECLARE @PropertyID int
		DECLARE @PropertyName varchar(15)
		DECLARE @Operator varchar(10)
		DECLARE @Value varchar(100)
		DECLARE @ANDSegmentCount int

		DECLARE @OrID int = 0
		DECLARE @OrCount int
		SELECT @OrCount = count(*) FROM @ORs

		while (@OrID < @OrCount)
		BEGIN
			-- Reset all variables and tables
			SET @PropertyID = 0
			SET @ANDSegmentCount = 0
			DELETE FROM @ANDMatchingIDs
			DELETE FROM @ANDSegments

			-- Get the next OR clause
			SELECT TOP 1 @OrClause = clause FROM @ORs WHERE id = @OrID

			-- Split it into all of the individual pieces of the AND - 3 pieces for each AND
			INSERT into @ANDSegments (id, segmentText) SELECT ValueID, [Value] FROM dbo.DelimitedListToTable(@OrClause, @ANDdelimiter)

			-- Get the count of AND pieces so we can iterate properly
			SELECT @ANDSegmentCount = count(*) FROM @ANDSegments

			while (@PropertyID < @ANDSegmentCount)
			BEGIN
				-- Get the 3 pieces for this AND clause. Property starts at index 0, 3, 6, etc
				SELECT @PropertyName = segmentText FROM @ANDSegments WHERE id = @PropertyID
				SELECT @Operator = segmentText FROM @ANDSegments WHERE id = @PropertyID + 1
				SELECT @Value = segmentText FROM @ANDSegments WHERE id = @PropertyID + 2
		
				-- Do the query - special process for first AND clause
				if (@PropertyID = 0)
					BEGIN
						-- Simply insert all the matching IDs into the table
						INSERT INTO @ANDMatchingIDs (id)
						SELECT id
						FROM val.VALIDATION_OE
						WHERE 1 = CASE
									WHEN @PropertyName = 'name' AND @Operator = 'eq' AND name = @Value THEN 1
									WHEN @PropertyName = 'name' AND @Operator = 'contains' AND name LIKE '%' + @Value + '%' THEN 1
									WHEN @PropertyName = 'name' AND @Operator = 'start' AND name LIKE @Value + '%' THEN 1
									WHEN @PropertyName = 'name' AND @Operator = 'end' AND name LIKE '%' + @Value THEN 1
									ELSE 0
								  END
					END
				ELSE
					BEGIN
						-- Remove from the IDs that have matched so far all those that don't match this particular AND clause
						DELETE FROM @ANDMatchingIDs
						WHERE id NOT IN (
							SELECT D.id
							FROM val.VALIDATION_OE D
								INNER JOIN
								@ANDMatchingIDs X ON X.id = D.id
							WHERE 1 = CASE
										WHEN @PropertyName = 'name' AND @Operator = 'eq' AND name = @Value THEN 1
										WHEN @PropertyName = 'name' AND @Operator = 'contains' AND name LIKE '%' + @Value + '%' THEN 1
										WHEN @PropertyName = 'name' AND @Operator = 'start' AND name LIKE @Value + '%' THEN 1
										WHEN @PropertyName = 'name' AND @Operator = 'end' AND name LIKE '%' + @Value THEN 1
										ELSE 0
									  END
						) 
					END

				-- Iterate to the next property, which is 3 more than where we are
				SET @PropertyID = @PropertyID + 3
			END

			-- Have completed all the ANDs on this OR line, so add them to the overall set of matching IDs. This may include duplicate IDs
			INSERT into @MatchingIDs (id) SELECT id FROM @ANDMatchingIDs WHERE id NOT IN (SELECT id FROM @MatchingIDs)

			SET @OrID = @OrID + 1
		END
	END

-- Get the total count
SELECT COUNT_BIG(1) AS TotalRecords
FROM @MatchingIDs

-- Determine which IDs are on this "page" (not exactly paged, but same idea)
DECLARE @PageIDs TABLE (id bigint)
INSERT INTO @PageIDs (id)
SELECT id
FROM @MatchingIDs
ORDER BY id
OFFSET ISNULL(@Offset, 0) ROWS
FETCH NEXT ISNULL(@Limit, @DefaultRowLimit) ROWS ONLY

-- Get the OE level data
SELECT	 OE.id AS Id
		,OE.[name] AS [Name]
FROM val.VALIDATION_OE OE
	INNER JOIN
	@PageIDs X ON X.id = OE.id

-- Get the dependency ids for all of these
SELECT	 L.validation_oe_id AS OEId
		,L.dependency_id AS DependencyId
FROM val.VALIDATION_OE_DEPENDENCY_LINK L
	INNER JOIN
	@PageIDs X ON X.id = L.validation_oe_id
