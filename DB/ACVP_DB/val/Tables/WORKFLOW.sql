CREATE TABLE [val].[WORKFLOW] (
    [id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [created_on]  DATETIME2 (7)  NOT NULL,
    [status]      INT            NOT NULL,
    [accept_id]   BIGINT         NULL,
    [json_blob]   NVARCHAR (MAX) NOT NULL,
    [lab_name]    NVARCHAR (100) NULL,
    [lab_contact] NVARCHAR (100) NULL,
    [lab_email]   NVARCHAR (100) NULL,
    [APIActionID] INT NULL, 
    [RequestingUserId] BIGINT NULL, 
    [LastUpdatedDate] DATETIME2 NULL, 
    CONSTRAINT [PK_WORKFLOW] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_WORKFLOW_STATUS] FOREIGN KEY ([status]) REFERENCES [val].[WORKFLOW_STATUS] ([id]),
    CONSTRAINT [FK_APIAction] FOREIGN KEY ([APIActionID]) REFERENCES [acvp].[APIActions] ([APIActionID]),
    CONSTRAINT [FK_ACVPUser] FOREIGN KEY ([RequestingUserId]) REFERENCES [acvp].[ACVP_USER] ([id]),
);

