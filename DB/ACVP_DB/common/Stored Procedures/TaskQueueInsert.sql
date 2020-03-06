CREATE PROCEDURE [common].[TaskQueueInsert]

	 @TaskType nvarchar(128)
	,@VectorSetId bigint
	,@IsSample bit

AS

SET NOCOUNT ON

INSERT INTO common.TaskQueue(TaskType, VsId, IsSample)
VALUES (@TaskType, @VectorSetId, @IsSample)