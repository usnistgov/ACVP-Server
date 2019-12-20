CREATE TABLE [common].[EVENT_LOG] (
    [id]                BIGINT          IDENTITY (1, 1) NOT NULL,
    [event_date]        DATETIME2 (7)   NOT NULL,
    [subject_name]      NVARCHAR (1024) NOT NULL,
    [event_log_type_id] INT             NOT NULL,
    [event_information] NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_EVENT_LOG] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_EVENT_LOG_EVENT_LOG_TYPE_ID] FOREIGN KEY ([event_log_type_id]) REFERENCES [common].[EVENT_LOG_TYPE] ([id])
);

