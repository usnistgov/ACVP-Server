CREATE PROCEDURE [common].[TaskQueueInsert]

	 @TaskType nvarchar(128)
	,@Payload varbinary(MAX)

AS

SET NOCOUNT ON

INSERT INTO common.TASK_QUEUE (task_type, task_payload, created_on)
VALUES (@TaskType, @Payload, CURRENT_TIMESTAMP)