﻿CREATE PROCEDURE [val].[PersonInsert]

	 @Name nvarchar(1024)
	,@OrganizationID bigint

AS

SET NOCOUNT ON

INSERT INTO val.PERSON (
	 full_name
	,org_id
)
VALUES (
	 @Name
	,@OrganizationID
)

SELECT SCOPE_IDENTITY() AS PersonID
