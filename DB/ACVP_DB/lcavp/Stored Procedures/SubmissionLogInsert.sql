
CREATE PROCEDURE [lcavp].[SubmissionLogInsert]
	 @SubmissionID nvarchar(100)
	,@LabName nvarchar(50)
	,@LabPOC nvarchar(50)
	,@LabPOCEmail nvarchar(50)
	,@SubmissionType char(1)
	,@ReceivedDate datetime2(7)
	,@Status tinyint
	,@SenderEmailAddress nvarchar(50)
	,@ZipFileName nvarchar(255)
	,@ExtractedFileLocation nvarchar(255)
	,@ErrorJson nvarchar(MAX)
AS

SET NOCOUNT ON

INSERT INTO lcavp.SubmissionLog (SubmissionID, LabName, LabPOC, LabPOCEmail, SubmissionType, ReceivedDate, ProcessedDate, [Status], SenderEmailAddress, ZipFileName, ExtractedFileLocation, ErrorJson, Archived)
VALUES (@SubmissionID, @LabName, @LabPOC, @LabPOCEmail, @SubmissionType, @ReceivedDate, SYSUTCDATETIME(), @Status, @SenderEmailAddress, @ZipFileName, @ExtractedFileLocation, @ErrorJson, 0)

SELECT CAST(SCOPE_IDENTITY() AS int) AS SubmissionLogID



