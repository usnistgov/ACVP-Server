CREATE TABLE [common].[TASK_QUEUE] (
    [id]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [task_type]    NVARCHAR (128) NOT NULL,
    [task_payload] NVARCHAR (MAX) NOT NULL,
    [task_status]  INT            DEFAULT ((0)) NOT NULL,
    [task_worker]  NVARCHAR (128) NULL,
    [task_message] NVARCHAR (MAX) NULL,
    [created_on]   DATETIME2 (7)  DEFAULT (sysutcdatetime()) NOT NULL,
    CONSTRAINT [PK_TASK_QUEUE] PRIMARY KEY CLUSTERED ([id] ASC) WITH (ALLOW_PAGE_LOCKS = OFF)
);

