CREATE TABLE [common].[EVENT_LOG_TYPE] (
    [id]                     INT             IDENTITY (1, 1) NOT NULL,
    [event_type_name]        NVARCHAR (128)  NOT NULL,
    [event_type_description] NVARCHAR (2048) NOT NULL,
    CONSTRAINT [PK_EVENT_LOG_TYPE] PRIMARY KEY CLUSTERED ([id] ASC)
);

