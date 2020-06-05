CREATE PROCEDURE [val].[ImplementationFilteredListGet]

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
		FROM val.PRODUCT_INFORMATION
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
						SELECT p.id
						FROM val.PRODUCT_INFORMATION P
						INNER JOIN ref.MODULE_TYPE m ON P.module_type = m.id
						WHERE 1 = CASE
									WHEN @PropertyName = 'name' AND @Operator = 'eq' AND module_name = @Value THEN 1
									WHEN @PropertyName = 'name' AND @Operator = 'contains' AND module_name LIKE '%' + @Value + '%' THEN 1
									WHEN @PropertyName = 'name' AND @Operator = 'start' AND module_name LIKE @Value + '%' THEN 1
									WHEN @PropertyName = 'name' AND @Operator = 'end' AND module_name LIKE '%' + @Value THEN 1
									WHEN @PropertyName = 'version' AND @Operator = 'eq' AND module_version = @Value THEN 1
									WHEN @PropertyName = 'version' AND @Operator = 'contains' AND module_version LIKE '%' + @Value + '%' THEN 1
									WHEN @PropertyName = 'version' AND @Operator = 'start' AND module_version LIKE @Value + '%' THEN 1
									WHEN @PropertyName = 'version' AND @Operator = 'end' AND module_version LIKE '%' + @Value THEN 1
									WHEN @PropertyName = 'website' AND @Operator = 'eq' AND product_url = @Value THEN 1
									WHEN @PropertyName = 'website' AND @Operator = 'contains' AND product_url LIKE '%' + @Value + '%' THEN 1
									WHEN @PropertyName = 'website' AND @Operator = 'start' AND product_url LIKE @Value + '%' THEN 1
									WHEN @PropertyName = 'website' AND @Operator = 'end' AND product_url LIKE '%' + @Value THEN 1
									WHEN @PropertyName = 'description' AND @Operator = 'eq' AND [implementation_description] = @Value THEN 1
									WHEN @PropertyName = 'description' AND @Operator = 'contains' AND [implementation_description] LIKE '%' + @Value + '%' THEN 1
									WHEN @PropertyName = 'description' AND @Operator = 'start' AND [implementation_description] LIKE @Value + '%' THEN 1
									WHEN @PropertyName = 'description' AND @Operator = 'end' AND [implementation_description] LIKE '%' + @Value THEN 1
									WHEN @PropertyName = 'type' AND @Operator = 'eq' AND m.[name] = @Value THEN 1
									WHEN @PropertyName = 'type' AND @Operator = 'ne' AND m.[name] <> @Value THEN 1
									WHEN @PropertyName = 'vendorId' AND @Operator = 'eq' AND vendor_id = CAST(@Value as bigint) THEN 1
									WHEN @PropertyName = 'vendorId' AND @Operator = 'ne' AND vendor_id <> CAST(@Value as bigint) THEN 1
									WHEN @PropertyName = 'vendorId' AND @Operator = 'lt' AND vendor_id < CAST(@Value as bigint) THEN 1
									WHEN @PropertyName = 'vendorId' AND @Operator = 'le' AND vendor_id <= CAST(@Value as bigint) THEN 1
									WHEN @PropertyName = 'vendorId' AND @Operator = 'gt' AND vendor_id > CAST(@Value as bigint) THEN 1
									WHEN @PropertyName = 'vendorId' AND @Operator = 'ge' AND vendor_id >= CAST(@Value as bigint) THEN 1
									ELSE 0
								  END
					END
				ELSE
					BEGIN
						-- Remove from the IDs that have matched so far all those that don't match this particular AND clause
						DELETE FROM @ANDMatchingIDs
						WHERE id NOT IN (
							SELECT P.id
							FROM val.PRODUCT_INFORMATION P
								INNER JOIN
								@ANDMatchingIDs X ON X.id = P.id
								INNER JOIN ref.MODULE_TYPE m ON P.module_type = m.id
							WHERE 1 = CASE
										WHEN @PropertyName = 'name' AND @Operator = 'eq' AND module_name = @Value THEN 1
										WHEN @PropertyName = 'name' AND @Operator = 'contains' AND module_name LIKE '%' + @Value + '%' THEN 1
										WHEN @PropertyName = 'name' AND @Operator = 'start' AND module_name LIKE @Value + '%' THEN 1
										WHEN @PropertyName = 'name' AND @Operator = 'end' AND module_name LIKE '%' + @Value THEN 1
										WHEN @PropertyName = 'version' AND @Operator = 'eq' AND module_version = @Value THEN 1
										WHEN @PropertyName = 'version' AND @Operator = 'contains' AND module_version LIKE '%' + @Value + '%' THEN 1
										WHEN @PropertyName = 'version' AND @Operator = 'start' AND module_version LIKE @Value + '%' THEN 1
										WHEN @PropertyName = 'version' AND @Operator = 'end' AND module_version LIKE '%' + @Value THEN 1
										WHEN @PropertyName = 'website' AND @Operator = 'eq' AND product_url = @Value THEN 1
										WHEN @PropertyName = 'website' AND @Operator = 'contains' AND product_url LIKE '%' + @Value + '%' THEN 1
										WHEN @PropertyName = 'website' AND @Operator = 'start' AND product_url LIKE @Value + '%' THEN 1
										WHEN @PropertyName = 'website' AND @Operator = 'end' AND product_url LIKE '%' + @Value THEN 1
										WHEN @PropertyName = 'description' AND @Operator = 'eq' AND [implementation_description] = @Value THEN 1
										WHEN @PropertyName = 'description' AND @Operator = 'contains' AND [implementation_description] LIKE '%' + @Value + '%' THEN 1
										WHEN @PropertyName = 'description' AND @Operator = 'start' AND [implementation_description] LIKE @Value + '%' THEN 1
										WHEN @PropertyName = 'description' AND @Operator = 'end' AND [implementation_description] LIKE '%' + @Value THEN 1
										WHEN @PropertyName = 'type' AND @Operator = 'eq' AND m.[name] = @Value THEN 1
										WHEN @PropertyName = 'type' AND @Operator = 'ne' AND m.[name] <> @Value THEN 1
										WHEN @PropertyName = 'vendorId' AND @Operator = 'eq' AND vendor_id = CAST(@Value as bigint) THEN 1
										WHEN @PropertyName = 'vendorId' AND @Operator = 'ne' AND vendor_id <> CAST(@Value as bigint) THEN 1
										WHEN @PropertyName = 'vendorId' AND @Operator = 'lt' AND vendor_id < CAST(@Value as bigint) THEN 1
										WHEN @PropertyName = 'vendorId' AND @Operator = 'le' AND vendor_id <= CAST(@Value as bigint) THEN 1
										WHEN @PropertyName = 'vendorId' AND @Operator = 'gt' AND vendor_id > CAST(@Value as bigint) THEN 1
										WHEN @PropertyName = 'vendorId' AND @Operator = 'ge' AND vendor_id >= CAST(@Value as bigint) THEN 1
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

-- Get the implementation level data
SELECT	 P.id AS Id
		,P.module_name AS [Name]
		,P.module_version AS [Version]
		,P.module_type AS [Type]
		,P.product_url AS Website
		,P.implementation_description AS [Description]
		,P.vendor_id AS OrganizationId
		,P.address_id AS AddressId
FROM val.PRODUCT_INFORMATION P
	INNER JOIN
	@PageIDs X ON X.id = P.id

-- Get the contacts for all of these
SELECT	 VC.product_information_id AS ImplementationId
		,VC.person_id AS PersonId
		,VC.order_index AS OrderIndex
FROM val.VALIDATION_CONTACT VC
	INNER JOIN
	@PageIDs X ON X.id = VC.product_information_id
