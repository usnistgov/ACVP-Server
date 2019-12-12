CREATE PROCEDURE [val].[WorkflowInsert]

	 @WorkflowItemType int
	,@Action int
	,@Status int
	,@LabName nvarchar(100)
	,@LabContactName nvarchar(100)
	,@LabContactEmail nvarchar(100)
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
)

SELECT SCOPE_IDENTITY() AS WorkflowID