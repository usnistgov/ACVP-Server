CREATE TABLE [val].[SUBMISSION] (
    [id]          BIGINT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [created_on]  DATETIME2 (7)   NULL,
    [module_name] NVARCHAR (1024) NULL,
    [status]      INT             NULL,
    [json_blob]   NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_SUBMISSION] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_SUBMISSION_TYPE] FOREIGN KEY ([status]) REFERENCES [val].[SUBMISSION_STATUS] ([id])
);

