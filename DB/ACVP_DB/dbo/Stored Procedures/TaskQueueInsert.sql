CREATE PROCEDURE [dbo].[TaskQueueInsert]

	 @TaskTypeId tinyint
	,@VectorSetId bigint
	,@IsSample bit
	,@ShowExpected bit

AS

SET NOCOUNT ON

INSERT INTO dbo.TaskQueue(TaskTypeId, VectorSetId, IsSample, ShowExpected)
VALUES (@TaskTypeId, @VectorSetId, @IsSample, @ShowExpected)