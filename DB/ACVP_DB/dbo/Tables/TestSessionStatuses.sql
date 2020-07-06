CREATE TABLE [dbo].[TestSessionStatuses]
(
	[TestSessionStatusId] TINYINT NOT NULL , 
    [StatusName] VARCHAR(100) NOT NULL,
	CONSTRAINT	[PK_TestSessionStatuses] PRIMARY KEY CLUSTERED ([TestSessionStatusId] ASC)
)
