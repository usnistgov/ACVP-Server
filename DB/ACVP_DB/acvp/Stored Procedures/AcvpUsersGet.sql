CREATE PROCEDURE [acvp].[AcvpUsersGet]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	au.id AS acvpUserId
			, p.id AS personId
			, p.full_name AS fullName
			, o.[name] AS companyName
			, o.id AS companyId
	FROM	acvp.ACVP_USER au
	INNER	JOIN val.PERSON p ON au.person_id = p.id
	INNER	JOIN val.ORGANIZATION o ON p.org_id = o.id

END