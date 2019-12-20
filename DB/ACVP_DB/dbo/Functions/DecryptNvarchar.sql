
CREATE FUNCTION [dbo].[DecryptNvarchar](
	@EncryptedValue varbinary(MAX)
)
RETURNS varchar(MAX)

AS

BEGIN
	DECLARE @TotalEncryptedLength int
	DECLARE @CurrentPosition  int
	DECLARE @EncryptedChunk varbinary(8000)
	DECLARE @EncryptedChunkLength  int
	DECLARE @DecryptedChunk nvarchar(3900)
	DECLARE @DecryptedValue nvarchar(MAX)
	
	SET @CurrentPosition = 1
	SET @TotalEncryptedLength = DATALENGTH(@EncryptedValue)
	SET @DecryptedValue = null
	
	WHILE @CurrentPosition <= @TotalEncryptedLength
	BEGIN
		-- Read the next encrypted chunk length - this is the first 2 bytes of the chunk, because we know that's how we encrypted it
		SELECT @EncryptedChunkLength = SUBSTRING(@EncryptedValue, @CurrentPosition, 2)

		-- Move the "cursor" to skip the chunk length
		SET @CurrentPosition = @CurrentPosition + 2
		
		-- Get the next encrypted chunk content
		SELECT @EncryptedChunk = SUBSTRING(@EncryptedValue, @CurrentPosition, @EncryptedChunkLength)

		-- Move the "cursor" to skip the chunk content
		SET @CurrentPosition = @CurrentPosition + @EncryptedChunkLength
		
		-- Check that the claimed length of the chunk and what we have match, or exit
		IF DATALENGTH(@EncryptedChunk) <> @EncryptedChunkLength
			RETURN null
	
		-- Decrypt the current encrypted chunk
		SET @DecryptedChunk = CAST(DECRYPTBYKEY(@EncryptedChunk) as nvarchar(3900))
	
		-- Exit if decryption failed
		IF @DecryptedChunk is null
			RETURN null
	
		-- Concatenate this chunk onto the output. First chunk requires special handling
		IF @DecryptedValue IS NULL
			SET @DecryptedValue = @DecryptedChunk
		ELSE
		   SET @DecryptedValue = @DecryptedValue + @DecryptedChunk
	END
	
	RETURN @DecryptedValue
END

