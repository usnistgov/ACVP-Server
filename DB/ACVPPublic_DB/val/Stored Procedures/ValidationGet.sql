CREATE PROCEDURE [val].[ValidationGet]
	
	@Id BIGINT

AS
BEGIN
	
	SET NOCOUNT ON;

    SELECT	vr.id AS Id, vs.prefix AS SourcePrefix, vr.validation_id AS ValidationId
	FROM	val.validation_record vr
	INNER	JOIN val.validation_source vs ON vr.source_id = vs.id
	WHERE	vr.id = @id

END