CREATE PROCEDURE [val].[WorkflowInsert]

	 @APIActionID int
	,@WorkflowItemType int
	,@Action int
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
	,[type]
	,[status]
	,json_blob
	,lab_name
	,lab_contact
	,lab_email
	,[action]
	,APIActionID
	,RequestingUserId
	,LastUpdatedDate
)
VALUES (
	 CURRENT_TIMESTAMP
	,@WorkflowItemType
	,@Status
	,@Json
	,@LabName
	,@LabContactName
	,@LabContactEmail
	,@Action
	,@APIActionID
	,@RequestingUserId
	,CURRENT_TIMESTAMP
)

SELECT SCOPE_IDENTITY() AS WorkflowID