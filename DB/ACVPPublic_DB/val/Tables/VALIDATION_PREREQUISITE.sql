CREATE TABLE [val].[VALIDATION_PREREQUISITE] (
    [id]                    BIGINT          NOT NULL,
    [scenario_algorithm_id] BIGINT          NOT NULL,
    [record_id]             BIGINT          NOT NULL,
    [requirement]           NVARCHAR (2048) NOT NULL
);

