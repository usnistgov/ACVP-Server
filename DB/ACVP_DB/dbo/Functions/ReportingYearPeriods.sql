
CREATE FUNCTION [dbo].[ReportingYearPeriods]
(
	 @StartDate datetime2(7)
	,@EndDate datetime2(7)
	,@ReportingYearEndMonth int
)
RETURNS @returntable TABLE
(
	 StartOfPeriod datetime2(7)
	,EndOfPeriod datetime2(7)
	,ReportingYear int
)
AS
BEGIN

	DECLARE @StartYear int
	SET @StartYear = DATEDIFF(YEAR, 0, @StartDate)

	DECLARE @StartOffset int
	SET @StartOffset = CASE
						WHEN DATEPART(month, @StartDate) <= @ReportingYearEndMonth THEN 1
						ELSE 0
					  END

	DECLARE @EndOffset int
	SET @EndOffset = CASE
						WHEN DATEPART(month, @EndDate) > @ReportingYearEndMonth THEN 1
						ELSE 0
					  END

	DECLARE @YearCount int 
		SET @YearCount = DATEDIFF(YEAR, @StartDate, @EndDate) + @StartOffset + @EndOffset

	INSERT @returntable
	SELECT	 CASE Y.YearNumber
				WHEN 0 THEN @StartDate
				ELSE DATEADD(YEAR, Y.YearNumber + @StartYear - @StartOffset, DATEADD(MONTH, @ReportingYearEndMonth, 0))
				END AS StartOfPeriod
			,CASE Y.YearNumber
				WHEN @YearCount - 1 THEN @EndDate
				ELSE DATEADD(YEAR, Y.YearNumber + @StartYear - @StartOffset + 1, DATEADD(MONTH, @ReportingYearEndMonth, 0))
				END AS EndOfPeriod
			,DATEPART(YEAR, DATEADD(YEAR, Y.YearNumber + 1 - @StartOffset, @StartDate)) AS ReportingYear
	FROM (	SELECT	TOP (@YearCount) ROW_NUMBER() OVER (ORDER BY [object_id]) - 1 AS YearNumber
			FROM sys.all_objects
			ORDER BY [object_id]) Y

	RETURN
END

