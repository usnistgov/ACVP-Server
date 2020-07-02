CREATE PROCEDURE [dbo].[PrerequisiteInsert]

	 @ValidationOEAlgorithmId bigint
	,@ValidationId bigint
	,@Requirement nvarchar(2048)

AS

SET NOCOUNT ON

INSERT INTO dbo.Prerequisites (
	 ValidationOEAlgorithmId
	,ValidationId
	,Requirement
)
VALUES (
	 @ValidationOEAlgorithmId
	,@ValidationId
	,@Requirement
)

SELECT CAST(SCOPE_IDENTITY() AS bigint) AS PrerequisiteId

