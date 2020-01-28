CREATE TABLE [acvp].[PRODUCT_CAPABILITY] (
    [id]                 BIGINT         IDENTITY (1, 1) NOT NULL,
    [test_session_id]    BIGINT         NOT NULL,
    [algorithm_property] BIGINT         NOT NULL,
    [root]               BIGINT         NULL,
    [level]              INT            NOT NULL,
    [parent]             BIGINT         NULL,
    [type]               INT            NOT NULL,
    [order_index]        INT            NULL,
    [string_value]       NVARCHAR (512) NULL,
    [number_value]       INT            NULL,
    CONSTRAINT [PK_PRODUCT_CAPABILITY] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_PRODUCT_CAPABILITY_CRYPTO_ALGORITHM_PROPERTY_ID] FOREIGN KEY ([algorithm_property]) REFERENCES [ref].[CRYPTO_ALGORITHM_PROPERTY] ([id]),
    CONSTRAINT [FK_PRODUCT_CAPABILITY_PARENT_ID] FOREIGN KEY ([parent]) REFERENCES [acvp].[PRODUCT_CAPABILITY] ([id]),
    CONSTRAINT [FK_PRODUCT_CAPABILITY_PRODUCT_INFORMATION_ID] FOREIGN KEY ([test_session_id]) REFERENCES [acvp].[TEST_SESSION] ([id]),
    CONSTRAINT [FK_PRODUCT_CAPABILITY_ROOT_ID] FOREIGN KEY ([root]) REFERENCES [acvp].[PRODUCT_CAPABILITY] ([id]),
    CONSTRAINT [FK_PRODUCT_CAPABILITY_TYPE_ID] FOREIGN KEY ([type]) REFERENCES [ref].[PROPERTY_TYPE] ([id])
);

