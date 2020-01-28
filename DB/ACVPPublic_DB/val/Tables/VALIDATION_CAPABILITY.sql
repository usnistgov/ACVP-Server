CREATE TABLE [val].[VALIDATION_CAPABILITY] (
    [id]                    BIGINT         NOT NULL,
    [scenario_algorithm_id] BIGINT         NOT NULL,
    [algorithm_property]    BIGINT         NOT NULL,
    [historical]            BIT            NOT NULL,
    [root]                  BIGINT         NULL,
    [level]                 INT            NOT NULL,
    [parent]                BIGINT         NULL,
    [type]                  INT            NOT NULL,
    [order_index]           INT            NULL,
    [string_value]          NVARCHAR (512) NULL,
    [number_value]          BIGINT         NULL,
    [boolean_value]         BIT            NULL
);

