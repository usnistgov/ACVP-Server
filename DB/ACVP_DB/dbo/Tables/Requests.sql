CREATE TABLE [dbo].[Requests] (
    [RequestId]          BIGINT NOT NULL,
    [WorkflowItemId] BIGINT NOT NULL,
    [ACVPUserId]     BIGINT NOT NULL,
    CONSTRAINT [PK_Requests] PRIMARY KEY CLUSTERED ([RequestId] ASC),
    CONSTRAINT [FK_Requests_ACVPUsers] FOREIGN KEY ([ACVPUserId]) REFERENCES [dbo].[ACVPUsers] ([ACVPUserId]),
    CONSTRAINT [FK_Requests_WorkflowItems] FOREIGN KEY ([WorkflowItemId]) REFERENCES [dbo].[WorkflowItems] ([WorkflowItemId])
);

