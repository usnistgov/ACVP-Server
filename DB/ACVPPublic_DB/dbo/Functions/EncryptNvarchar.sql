
CREATE FUNCTION [dbo].[EncryptNvarchar] (
	 @KeyGuid uniqueidentifier
	,@Value nvarchar(MAX) 
)
RETURNS varbinary(MAX)

AS

BEGIN
	DECLARE @MaxChunkLength int
	DECLARE @TotalLength  int
	DECLARE @CurrentPosition  int
	DECLARE @ChunkText nvarchar(3900)
	DECLARE @EncryptedChunk varbinary(8000)
	DECLARE @EncryptedChunkLength int
	DECLARE @EncryptedValue varbinary(MAX)		-- The final result
	
	SET @MaxChunkLength = 3900					-- Break the text into chunks less than 8000 characters, plus need some room for extra data the encrypt adds, plus nvarchar uses by 2 bytes per character
	SET @TotalLength = DATALENGTH(@value)
	SET @CurrentPosition = 1
	SET @EncryptedValue = null

	WHILE @CurrentPosition <= @TotalLength
	BEGIN
		-- Get a new chunk of text
		SELECT @ChunkText = SUBSTRING(@Value, @CurrentPosition, @MaxChunkLength)

		--Move the "cursor" to the first character of the next chunk (doesn't matter if not one there)
		SET @CurrentPosition = @CurrentPosition + @MaxChunkLength
	
		-- Encrypt the chunk
		SET @EncryptedChunk = ENCRYPTBYKEY(@KeyGuid, @ChunkText)
		
		-- Exit if encryption failed
		IF (@EncryptedChunk IS NULL)
			RETURN null
	
		-- Get the length of the encrypted chunk
		SET @EncryptedChunkLength = DATALENGTH(@EncryptedChunk)

		-- Convert that length to binary (2 bytes is enough) and prepend it to the encrypted chunk
		SET @EncryptedChunk = CAST(@EncryptedChunkLength AS binary(2)) + @EncryptedChunk	-- convert(binary(2), @EncryptedChunkLength) + @EncryptedChunk
	
		-- Concatenate this chunk onto the output. First chunk requires special handling
		IF (@EncryptedValue IS NULL)
			SET @EncryptedValue = @EncryptedChunk
		ELSE
			SET @EncryptedValue = @EncryptedValue + @EncryptedChunk
	END
	
	RETURN @EncryptedValue
END

