CREATE PROCEDURE [dbo].[ACVPUserInsert]

	 @PersonId int
	,@CommonName varchar(max)
	,@Certificate varbinary(max)
	,@Seed nvarchar(64)
	,@ExpiresOn datetime2(7)

AS

SET NOCOUNT ON

INSERT INTO dbo.ACVPUsers (PersonId, CommonName, [Certificate], Seed, ExpiresOn)
VALUES (@PersonId, @CommonName, @Certificate, @Seed, @ExpiresOn)

SELECT CAST(SCOPE_IDENTITY() AS bigint) AS ACVPUserId