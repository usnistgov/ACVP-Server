
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
	DECLARE @EncryptedChunks TABLE (id int identity, Content varbinary(8000))
	
	SET @MaxChunkLength = 3900					-- Break the text into chunks less than 8000 characters, plus need some room for extra data the encrypt adds, plus nvarchar uses by 2 bytes per character
	SET @TotalLength = DATALENGTH(@Value)
	SET @CurrentPosition = 1

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

		-- Add 2 records for the length and the value - slightly faster than concatenating and doing 1 insert when you have very large inputs
		INSERT INTO @EncryptedChunks (Content) VALUES 
		(CAST(@EncryptedChunkLength AS binary(2))),			--Convert that length to binary (2 bytes is enough) and prepend it to the encrypted chunk
		(@EncryptedChunk)
	END
	
	-- Concatenate all back into a single value to return
	RETURN CONVERT(varbinary(max), (SELECT CONVERT(VARCHAR(MAX), Content,2) AS [text()] FROM @EncryptedChunks order by id for xml path('')), 2)
END

