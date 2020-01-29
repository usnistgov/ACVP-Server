
CREATE PROCEDURE [acvp].[VectorSetJsonPut]
    @VsId INT,
    @JsonFileType VARCHAR(20),
    @Content VARCHAR(max)
AS

DECLARE @JsonFileTypeId BIGINT
-- Get JsonFileType Id from String
SELECT @JsonFileTypeId = Id
FROM [common].[JsonFileType]
WHERE FileType = @JsonFileType

-- Put content in database
INSERT INTO [acvp].[VectorSetJson] (VsId, FileType, Content, CreatedOn)
VALUES (@VsId, @JsonFileTypeId, @Content, GETDATE())