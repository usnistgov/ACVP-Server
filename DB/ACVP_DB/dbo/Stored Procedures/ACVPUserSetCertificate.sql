CREATE PROCEDURE [dbo].[ACVPUserSetCertificate]

	 @ACVPUserId int
	,@CommonName varchar(max)
	,@Certificate varbinary(max)
	,@ExpiresOn datetime2(7)

AS

SET NOCOUNT ON

UPDATE dbo.ACVPUsers 
SET	 CommonName = @CommonName
	,[Certificate] = @Certificate
	,ExpiresOn = @ExpiresOn
WHERE ACVPUserId = @ACVPUserId