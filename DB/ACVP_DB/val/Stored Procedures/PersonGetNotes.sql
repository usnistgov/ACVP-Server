CREATE PROCEDURE [val].[PersonGetNotes]
	@PersonID bigint
AS
	SELECT NOTE.id,
			NOTE.note_date,
			NOTE.note_subject,
			NOTE.note
	FROM [val].[PERSON_NOTE] AS NOTE
	WHERE NOTE.person_id = @PersonID