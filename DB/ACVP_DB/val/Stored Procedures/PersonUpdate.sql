CREATE PROCEDURE [val].[PersonUpdate]

	 @PersonID bigint
	,@Name nvarchar(1024)
	,@OrganizationID bigint
	,@NameUpdated bit
	,@OrganizationIDUpdated bit

AS

SET NOCOUNT ON

UPDATE val.PERSON
SET	 full_name = CASE @NameUpdated WHEN 1 THEN @Name ELSE full_name END
	,org_id = CASE @OrganizationIDUpdated WHEN 1 THEN @OrganizationID ELSE org_id END
WHERE id = @PersonID

