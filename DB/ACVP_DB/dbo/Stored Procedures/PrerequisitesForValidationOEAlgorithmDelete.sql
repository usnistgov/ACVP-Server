CREATE PROCEDURE [dbo].[PrerequisitesForValidationOEAlgorithmDelete]

	@ValidationOEAlgorithmId bigint

AS
	
SET NOCOUNT ON

DELETE
FROM dbo.Prerequisites
WHERE ValidationOEAlgorithmID = @ValidationOEAlgorithmId
