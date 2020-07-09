CREATE TABLE [dbo].[RequestWorkflow] (
    [RequestId]        BIGINT        NOT NULL,
    [ACVPUserId]       BIGINT        NOT NULL,
    [CreatedOn]        DATETIME2 (7) NOT NULL,
    [WorkflowStatusId] INT           NOT NULL,
    [AcceptId]         BIGINT        NULL,
    [APIActionId]      INT           NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [PK_RequestWorkflow]
    ON [dbo].[RequestWorkflow]([RequestId] ASC);

