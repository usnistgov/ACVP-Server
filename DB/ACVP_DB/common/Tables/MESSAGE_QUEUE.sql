CREATE TABLE [common].[MESSAGE_QUEUE] (
    [id]              UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [message_type]    INT              NOT NULL,
    [message_payload] VARBINARY (MAX)  NOT NULL,
    [message_status]  INT              DEFAULT ((0)) NOT NULL,
    [created_on]      DATETIME2 (7)    DEFAULT (sysutcdatetime()) NOT NULL,
    [rowguid]         UNIQUEIDENTIFIER CONSTRAINT [MSmerge_df_rowguid_06AD8A425B1B495CA8B7DAC631228CE5] DEFAULT (newsequentialid()) ROWGUIDCOL NOT NULL,
    [userId]          BIGINT           CONSTRAINT [DF_MESSAGE_QUEUE_userId] DEFAULT ((-1)) NOT NULL,
    CONSTRAINT [PK_MESSAGE_QUEUE] PRIMARY KEY CLUSTERED ([id] ASC) WITH (ALLOW_PAGE_LOCKS = OFF)
);





