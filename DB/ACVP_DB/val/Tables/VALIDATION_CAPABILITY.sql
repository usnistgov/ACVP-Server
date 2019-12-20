CREATE TABLE [val].[VALIDATION_CAPABILITY] (
    [id]                    BIGINT         IDENTITY (1, 1) NOT NULL,
    [scenario_algorithm_id] BIGINT         NOT NULL,
    [algorithm_property]    BIGINT         NOT NULL,
    [historical]            BIT            DEFAULT ((0)) NOT NULL,
    [root]                  BIGINT         NULL,
    [level]                 INT            NOT NULL,
    [parent]                BIGINT         NULL,
    [type]                  INT            NOT NULL,
    [order_index]           INT            NULL,
    [string_value]          NVARCHAR (512) NULL,
    [number_value]          BIGINT         NULL,
    [boolean_value]         BIT            NULL,
    CONSTRAINT [PK_VALIDATION_CAPABILITY] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_VALIDATION_CAPABILITY_CRYPTO_ALGORITHM_PROPERTY_ID] FOREIGN KEY ([algorithm_property]) REFERENCES [ref].[CRYPTO_ALGORITHM_PROPERTY] ([id]),
    CONSTRAINT [FK_VALIDATION_CAPABILITY_PARENT_ID] FOREIGN KEY ([parent]) REFERENCES [val].[VALIDATION_CAPABILITY] ([id]),
    CONSTRAINT [FK_VALIDATION_CAPABILITY_ROOT_ID] FOREIGN KEY ([root]) REFERENCES [val].[VALIDATION_CAPABILITY] ([id]),
    CONSTRAINT [FK_VALIDATION_CAPABILITY_TYPE_ID] FOREIGN KEY ([type]) REFERENCES [ref].[PROPERTY_TYPE] ([id]),
    CONSTRAINT [FK_VALIDATION_CAPABILITY_VALIDATION_ID] FOREIGN KEY ([scenario_algorithm_id]) REFERENCES [val].[VALIDATION_SCENARIO_ALGORITHM] ([id])
);

