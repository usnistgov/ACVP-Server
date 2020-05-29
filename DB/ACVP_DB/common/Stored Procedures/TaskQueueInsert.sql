CREATE PROCEDURE [common].[TaskQueueInsert]

	 @TaskType nvarchar(128)
	,@VectorSetId bigint
	,@IsSample bit
	,@showExpected bit

AS

SET NOCOUNT ON

INSERT INTO common.TaskQueue(TaskType, VsId, IsSample, ShowExpected)
VALUES (@TaskType, @VectorSetId, @IsSample, @showExpected)