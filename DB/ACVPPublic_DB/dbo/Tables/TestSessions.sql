CREATE TABLE [dbo].[TestSessions] (
    [TestSessionId]       BIGINT        NOT NULL,
    [CreatedOn]           DATETIME2 (7) NOT NULL,
    [ACVVersionId]        INT           NOT NULL,
    [Generator]           NVARCHAR (32) NOT NULL,
    [IsSample]            BIT           NOT NULL,
    [ACVPUserId]          BIGINT        NULL,
    [TestSessionStatusId] TINYINT       NULL,
    [LastTouched]         DATETIME2 (7) NULL,
    CONSTRAINT [PK_TestSessions] PRIMARY KEY CLUSTERED ([TestSessionId] ASC)
);

