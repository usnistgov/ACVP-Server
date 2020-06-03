CREATE PROCEDURE [dbo].[ValidationOEAlgorithmInactivate]

	 @ValidationOEAlgorithmId bigint

AS

SET NOCOUNT ON

UPDATE dbo.ValidationOEAlgorithms
SET	 Active = 0
	,InactiveOn = CURRENT_TIMESTAMP
WHERE ValidationOEAlgorithmId = @ValidationOEAlgorithmId