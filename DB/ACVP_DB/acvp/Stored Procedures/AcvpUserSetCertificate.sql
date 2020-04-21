CREATE PROCEDURE [acvp].[AcvpUserSetCertificate]

	 @AcvpUserId int
	,@CommonName varchar(max)
	,@Certificate varbinary(max)

AS

SET NOCOUNT ON

UPDATE acvp.ACVP_USER 
SET common_name = @CommonName
  , certificate = @Certificate
WHERE acvp.ACVP_USER.id = @AcvpUserId