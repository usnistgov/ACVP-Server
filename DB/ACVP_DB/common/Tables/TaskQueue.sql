CREATE TABLE [common].[TaskQueue] (
       [id]            BIGINT         IDENTITY (1, 1)            NOT NULL,
       [task_type]     NVARCHAR (128)                            NOT NULL,
       [is_sample]     INT            DEFAULT ((0))              NOT NULL,
       [show_expected] INT            DEFAULT ((0))              NOT NULL,
       [vs_id]         INT                                       NOT NULL,
       [status]        INT            DEFAULT ((0))              NOT NULL,
       [created_on]    DATETIME2 (7)  DEFAULT (sysutcdatetime()) NOT NULL,
       CONSTRAINT [PK_TaskQueue] PRIMARY KEY CLUSTERED ([id] ASC) WITH (ALLOW_PAGE_LOCKS = OFF)
);

