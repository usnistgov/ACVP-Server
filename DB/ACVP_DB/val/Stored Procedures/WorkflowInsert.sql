CREATE PROCEDURE [val].[WorkflowInsert]

	 @APIActionID int
	,@Status int
	,@LabName nvarchar(100)
	,@LabContactName nvarchar(100)
	,@LabContactEmail nvarchar(100)
	,@RequestingUserId bigint
	,@Json nvarchar(MAX)

AS

SET NOCOUNT ON

INSERT INTO val.WORKFLOW (
	 created_on
	,[status]
	,json_blob
	,lab_name
	,lab_contact
	,lab_email
	,APIActionID
	,RequestingUserId
	,LastUpdatedDate
)
VALUES (
	 CURRENT_TIMESTAMP
	,@Status
	,@Json
	,@LabName
	,@LabContactName
	,@LabContactEmail
	,@APIActionID
	,@RequestingUserId
	,CURRENT_TIMESTAMP
)

SELECT CAST(SCOPE_IDENTITY() AS bigint) AS WorkflowID