CREATE PROCEDURE [acvp].[AcvpUserInsert]

	 @PersonId int
	,@CommonName varchar(max)
	,@Certificate varbinary(max)
	,@Seed nvarchar(64)
	,@ExpiresOn datetime

AS

SET NOCOUNT ON

INSERT INTO acvp.ACVP_USER(person_id, common_name, certificate, seed, expiresOn)
VALUES (@PersonId, @CommonName, @Certificate, @Seed, @ExpiresOn)

SELECT CAST(SCOPE_IDENTITY() AS bigint) AS UserID