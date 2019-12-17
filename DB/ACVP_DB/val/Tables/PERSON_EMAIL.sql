CREATE TABLE [val].[PERSON_EMAIL] (
    [person_id]     BIGINT        NOT NULL,
    [order_index]   INT           NOT NULL,
    [email_address] VARCHAR (512) NOT NULL,
    CONSTRAINT [PK_PERSON_EMAIL] PRIMARY KEY CLUSTERED ([person_id] ASC, [email_address] ASC),
    CONSTRAINT [FK_PERSON_EMAIL_PERSON_ID] FOREIGN KEY ([person_id]) REFERENCES [val].[PERSON] ([id]),
    CONSTRAINT [UQ_PERSON_EMAIL] UNIQUE NONCLUSTERED ([person_id] ASC, [order_index] ASC)
);

