﻿CREATE TABLE [acvp].[REQUEST] (
    [id]          BIGINT NOT NULL,
    [action_id]   INT    NOT NULL,
    [workflow_id] BIGINT NOT NULL,
    [user_id]     BIGINT NOT NULL,
    CONSTRAINT [PK_REQUEST] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_REQUEST_ACTION_ID] FOREIGN KEY ([action_id]) REFERENCES [acvp].[REQUEST_ACTION] ([id]),
    CONSTRAINT [FK_REQUEST_USER_ID] FOREIGN KEY ([user_id]) REFERENCES [acvp].[ACVP_USER] ([id]),
    CONSTRAINT [FK_REQUEST_WORKFLOW_ID] FOREIGN KEY ([workflow_id]) REFERENCES [val].[WORKFLOW] ([id])
);
