CREATE TABLE [dbo].[MessageQueue] (
    [MessageId]     UNIQUEIDENTIFIER CONSTRAINT [DF_MessageQueue_MessageId] DEFAULT (newsequentialid()) ROWGUIDCOL NOT NULL,
    [APIActionId]   INT              NOT NULL,
    [MessageStatus] INT              CONSTRAINT [DF_MessageQueue_MessageStatus] DEFAULT ((0)) NOT NULL,
    [ACVPUserId]    BIGINT           NOT NULL,
    [CreatedOn]     DATETIME2 (7)    CONSTRAINT [DF_MessageQueue_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [Payload]       VARBINARY (MAX)  NOT NULL,
    CONSTRAINT [PK_MessageQueue] PRIMARY KEY CLUSTERED ([MessageId] ASC) WITH (ALLOW_PAGE_LOCKS = OFF)
);







