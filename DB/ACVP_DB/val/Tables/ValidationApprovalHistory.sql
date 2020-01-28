CREATE TABLE [val].[ValidationApprovalHistory]
(
	[ValidationApprovalHistoryId] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[ValidationId] BIGINT NOT NULL, 
    [TestSessionId] BIGINT NOT NULL, 
    [OperatingEnvironmentId] BIGINT NOT NULL, 
    [ApprovedDate] DATETIME2 NOT NULL
)
