﻿CREATE TABLE [common].[MESSAGE_QUEUE] (
    [id]              UNIQUEIDENTIFIER CONSTRAINT [DF__MESSAGE_QUEU__id__4E88ABD4] DEFAULT (newid()) NOT NULL,
    [message_type]    INT              NOT NULL,
    [message_payload] VARBINARY (MAX)  NOT NULL,
    [message_status]  INT              CONSTRAINT [DF__MESSAGE_Q__messa__4F7CD00D] DEFAULT ((0)) NOT NULL,
    [created_on]      DATETIME2 (7)    CONSTRAINT [DF__MESSAGE_Q__creat__5070F446] DEFAULT (sysutcdatetime()) NOT NULL,
    [rowguid]         UNIQUEIDENTIFIER CONSTRAINT [MSmerge_df_rowguid_06AD8A425B1B495CA8B7DAC631228CE5] DEFAULT (newsequentialid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_MESSAGE_QUEUE] PRIMARY KEY CLUSTERED ([id] ASC) WITH (ALLOW_PAGE_LOCKS = OFF)
);
