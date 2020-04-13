CREATE PROCEDURE [acvp].[AcvpUserInsert]

	 @PersonId int
	,@CommonName varchar
	,@Certificate varbinary(max)
	,@Seed nvarchar(64)

AS

SET NOCOUNT ON

INSERT INTO acvp.ACVP_USER(person_id, common_name, certificate, seed)
VALUES (@PersonId, @CommonName, @Certificate, @Seed)

SELECT CAST(SCOPE_IDENTITY() AS bigint) AS UserID