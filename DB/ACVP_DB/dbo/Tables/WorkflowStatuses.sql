CREATE TABLE [dbo].[WorkflowStatuses] (
    [WorkflowStatusId]     INT            NOT NULL,
    [Status] NVARCHAR (256) NULL,
    CONSTRAINT [PK_WorkflowStatuses] PRIMARY KEY CLUSTERED ([WorkflowStatusId] ASC)
);

