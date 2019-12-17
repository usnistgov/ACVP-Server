CREATE TABLE [val].[PERSON_PHONE] (
    [id]                BIGINT        IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [person_id]         BIGINT        NOT NULL,
    [order_index]       INT           NOT NULL,
    [phone_number]      NVARCHAR (64) NOT NULL,
    [phone_number_type] NVARCHAR (32) NOT NULL,
    CONSTRAINT [PK_PERSON_PHONE] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_PERSON_PHONE_PERSON_ID] FOREIGN KEY ([person_id]) REFERENCES [val].[PERSON] ([id]),
    CONSTRAINT [UQ_PERSON_PHONE] UNIQUE NONCLUSTERED ([person_id] ASC, [order_index] ASC)
);

