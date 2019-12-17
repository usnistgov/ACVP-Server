CREATE TABLE [val].[VALIDATION_OE_DEPENDENCY_ATTRIBUTE] (
    [id]                          BIGINT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [validation_oe_dependency_id] BIGINT          NOT NULL,
    [name]                        NVARCHAR (1024) NOT NULL,
    [value]                       NVARCHAR (1024) NOT NULL,
    CONSTRAINT [PK_VALIDATION_OE_DEPENDENCY_ATTRIBUTE] PRIMARY KEY CLUSTERED ([id] ASC)
);

