
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
VALUES (@VsId, @JsonFileTypeId, @Content, CURRENT_TIMESTAMP)

-- In some cases also need to insert into the VECTOR_SET_DATA table for replication, at least until Public is rewritten
if (@JsonFileType = 'prompt'
 OR @JsonFileType = 'validation'
 OR (@JsonFileType = 'expectedAnswers' AND EXISTS(SELECT NULL
                                                  FROM  acvp.VECTOR_SET VS
                                                        INNER JOIN
                                                        acvp.TEST_SESSION TS ON TS.id = VS.test_session_id
                                                                            AND VS.id = @VsId
                                                                            AND TS.[sample] = 1)))
    INSERT INTO acvp.VECTOR_SET_DATA
    (
         vector_set_id
        ,created_on
        ,data_type
        ,vector_set_data
    )
    VALUES
    (
         @VsId
        ,CURRENT_TIMESTAMP
        ,CASE @JsonFileType
            WHEN 'prompt' THEN 0
            WHEN 'validation' THEN 1
            ELSE 2
            END
        ,CAST(@Content AS varbinary(MAX))
    )