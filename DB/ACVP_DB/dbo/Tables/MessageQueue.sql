CREATE TABLE [dbo].[MessageQueue] (
    [MessageId]     UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [APIActionId]   INT              NOT NULL,
    [Payload]       VARBINARY (MAX)  NOT NULL,
    [MessageStatus] INT              DEFAULT ((0)) NOT NULL,
    [CreatedOn]     DATETIME2 (7)    DEFAULT (getdate()) NOT NULL,
    [rowguid]       UNIQUEIDENTIFIER CONSTRAINT [MSmerge_df_rowguid_06AD8A425B1B495CA8B7DAC631228CE5] DEFAULT (newsequentialid()) ROWGUIDCOL NOT NULL,
    [ACVPUserId]    BIGINT           CONSTRAINT [DF_MESSAGE_QUEUE_userId] DEFAULT ((-1)) NOT NULL,
    CONSTRAINT [PK_MESSAGE_QUEUE] PRIMARY KEY CLUSTERED ([MessageId] ASC) WITH (ALLOW_PAGE_LOCKS = OFF)
);







