﻿
CREATE PROCEDURE [acvp].[VectorSetJsonGet]
    @VsId INT,
    @JsonFileType VARCHAR(20)
AS

DECLARE @JsonFileTypeId BIGINT

-- Get JsonFileTypeId from String
SELECT @JsonFileTypeId = Id
FROM [common].[JsonFileType]
WHERE FileType = @JsonFileType

-- Get content from database
SELECT VsId, FileType, Content, CreatedOn
FROM [acvp].[VectorSetJson]
WHERE VsId = @VsId AND FileType = @JsonFileTypeId