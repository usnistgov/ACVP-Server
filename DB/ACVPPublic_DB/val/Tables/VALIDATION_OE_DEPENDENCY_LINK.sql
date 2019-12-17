CREATE TABLE [val].[VALIDATION_OE_DEPENDENCY_LINK] (
    [validation_oe_id] BIGINT NOT NULL,
    [dependency_id]    BIGINT NOT NULL,
    CONSTRAINT [PK_VALIDATION_OE_DEPENDENCY_LINK] PRIMARY KEY CLUSTERED ([validation_oe_id] ASC, [dependency_id] ASC)
);

