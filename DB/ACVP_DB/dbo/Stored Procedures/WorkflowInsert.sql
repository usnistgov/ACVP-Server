CREATE PROCEDURE [dbo].[WorkflowInsert]

	 @APIActionID int
	,@WorkflowStatusId int
	,@LabName nvarchar(100)
	,@LabContactName nvarchar(100)
	,@LabContactEmail nvarchar(100)
	,@RequestingUserId bigint
	,@Json nvarchar(MAX)

AS

SET NOCOUNT ON

INSERT INTO dbo.WorkflowItems(
	 CreatedOn
	,WorkflowStatusId
	,JsonBlob
	,LabName
	,LabContact
	,LabEmail
	,APIActionID
	,RequestingUserId
	,LastUpdatedDate
)
VALUES (
	 CURRENT_TIMESTAMP
	,@WorkflowStatusId
	,@Json
	,@LabName
	,@LabContactName
	,@LabContactEmail
	,@APIActionID
	,@RequestingUserId
	,CURRENT_TIMESTAMP
)

SELECT CAST(SCOPE_IDENTITY() AS bigint) AS WorkflowItemId