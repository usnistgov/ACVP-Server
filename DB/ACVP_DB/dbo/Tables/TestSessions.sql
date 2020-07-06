CREATE TABLE [dbo].[TestSessions] (
    [TestSessionId]             BIGINT        NOT NULL,
    [CreatedOn]     DATETIME2 (7) NOT NULL,
    [ACVVersionId] INT           NOT NULL,
    [Generator]      NVARCHAR (32) NOT NULL,
    [IsSample]         BIT           CONSTRAINT [DF_TestSessions_IsSample] DEFAULT ((0)) NOT NULL,
    [ACVPUserId]        BIGINT        NULL,
    [TestSessionStatusId] TINYINT NULL, 
    [LastTouched] DATETIME2 NULL, 
    CONSTRAINT [PK_TestSessions] PRIMARY KEY CLUSTERED ([TestSessionId] ASC),
    CONSTRAINT [FK_TestSessions_ACVVersions] FOREIGN KEY ([ACVVersionId]) REFERENCES [dbo].[ACVVersions] ([ACVVersionId]),
    CONSTRAINT [FK_TestSessions_ACVPUsers] FOREIGN KEY ([ACVPUserId]) REFERENCES [dbo].[ACVPUsers] ([ACVPUserId]),
    CONSTRAINT [FK_TestSessions_TestSessionStatuses] FOREIGN KEY ([TestSessionStatusId]) REFERENCES [dbo].[TestSessionStatuses] ([TestSessionStatusId])
);

