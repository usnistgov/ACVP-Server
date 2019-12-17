
CREATE FUNCTION [dbo].[MonthPeriods]
(
	 @StartDate datetime2(7)
	,@EndDate datetime2(7)
)
RETURNS @returntable TABLE
(
	 StartOfPeriod datetime2(7)
	,EndOfPeriod datetime2(7)
)
AS
BEGIN

	DECLARE @MonthCount int 
	SET @MonthCount = DATEDIFF(MONTH, @StartDate, @EndDate) + 1

	INSERT @returntable
	SELECT	 CASE M.monthNumber
				WHEN 0 THEN @StartDate
				ELSE DATEADD(MONTH, M.monthNumber, DATEADD(MONTH, DATEDIFF(MONTH, 0, @StartDate), 0))
				END AS StartOfPeriod
			,CASE M.monthNumber
				WHEN @MonthCount - 1 THEN @EndDate
				ELSE DATEADD(MONTH, M.monthNumber, DATEADD(MONTH, DATEDIFF(MONTH, 0, @StartDate) + 1, 0))
				END AS EndOfPeriod
	FROM (	SELECT TOP (@MonthCount) ROW_NUMBER() OVER (ORDER BY [object_id]) - 1 AS monthNumber
			FROM sys.all_objects
			ORDER BY [object_id]) M

	RETURN
END

