
CREATE FUNCTION [dbo].[DelimitedListToTable] (
	 @Input VARCHAR(MAX)
	,@Delimiter VARCHAR(10))
  
RETURNS @Values TABLE (
	 ValueID INT
	,Value VARCHAR(MAX))

AS

BEGIN 

	DECLARE	@SepPos INT
	DECLARE	@Count INT
	DECLARE	@DelimiterLength TINYINT

	--Get length of delimiter to avoid recalculation
	SET @DelimiterLength = LEN(@Delimiter)

	--Initialize counter used to create IDs in temp table
	SET @Count = 0

	-- prepare criteria string by forcing lowercase (for ASCII changes) and adding trailing delimiter
	SET @Input = LOWER(@Input) + @Delimiter

	WHILE PATINDEX('%' + @Delimiter + '%', @Input) <> 0 
		BEGIN
			--Find position of the delimiter
			SET @SepPos = PATINDEX('%' + @Delimiter + '%', @Input)

			--Add the value
  			INSERT INTO @Values
				(ValueID
				,Value)
			VALUES (@Count
				   ,LEFT(@Input, @SepPos - 1))

			--Increment the counter
			SET @Count = @Count + 1

			--Remove the value and comma from the input string
			SET @Input = RIGHT(@Input, LEN(@Input) - (@SepPos + @DelimiterLength - 1))
		END

	RETURN

END


