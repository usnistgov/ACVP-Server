CREATE PROCEDURE [acvp].[AcvpUserSetCertificate]

	 @AcvpUserId int
	,@CommonName varchar(max)
	,@Certificate varbinary(max)
	,@ExpiresOn datetime2(7)

AS

SET NOCOUNT ON

UPDATE acvp.ACVP_USER 
SET common_name = @CommonName
  , [certificate] = @Certificate
  , expiresOn = @ExpiresOn
WHERE acvp.ACVP_USER.id = @AcvpUserId