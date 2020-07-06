CREATE TABLE [dbo].[WorkflowItems] (
    [WorkflowItemId]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [CreatedOn]  DATETIME2 (7)  NOT NULL,
    [WorkflowStatusId]      INT            NOT NULL,
    [AcceptId]   BIGINT         NULL,
    [JsonBlob]   NVARCHAR (MAX) NOT NULL,
    [LabName]    NVARCHAR (100) NULL,
    [LabContact] NVARCHAR (100) NULL,
    [LabEmail]   NVARCHAR (100) NULL,
    [APIActionID] INT NULL, 
    [RequestingUserId] BIGINT NULL, 
    [LastUpdatedDate] DATETIME2 NULL, 
    CONSTRAINT [PK_WorkflowItems] PRIMARY KEY CLUSTERED ([WorkflowItemId] ASC),
    CONSTRAINT [FK_WorkflowItems_WorkflowStatuses] FOREIGN KEY ([WorkflowStatusId]) REFERENCES [dbo].[WorkflowStatuses] ([WorkflowStatusId]),
    CONSTRAINT [FK_WorkflowItems_APIActions] FOREIGN KEY ([APIActionID]) REFERENCES [dbo].[APIActions] ([APIActionId]),
    CONSTRAINT [FK_WorkflowItems_ACVPUsers] FOREIGN KEY ([RequestingUserId]) REFERENCES [dbo].[ACVPUsers] ([ACVPUserId]),
);

