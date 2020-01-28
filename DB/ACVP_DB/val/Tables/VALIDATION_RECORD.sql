CREATE TABLE [val].[VALIDATION_RECORD] (
    [id]                     BIGINT        IDENTITY (1, 1) NOT NULL,
    [product_information_id] BIGINT        NOT NULL,
    [source_id]              INT           NOT NULL,
    [validation_id]          BIGINT        NOT NULL,
    [created_on]             DATETIME2 (7) NOT NULL,
    [updated_on]             DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK_VALIDATION_RECORD] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_VALIDATION_RECORD_PRODUCT_INFORMATION_ID] FOREIGN KEY ([product_information_id]) REFERENCES [val].[PRODUCT_INFORMATION] ([id]),
    CONSTRAINT [FK_VALIDATION_RECORD_SOURCE_ID] FOREIGN KEY ([source_id]) REFERENCES [val].[VALIDATION_SOURCE] ([id]),
    CONSTRAINT [UQ_VALIDATION_RECORD_VALIDATION_ID] UNIQUE NONCLUSTERED ([validation_id] ASC, [source_id] ASC)
);

