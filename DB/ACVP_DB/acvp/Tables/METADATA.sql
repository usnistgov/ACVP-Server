CREATE TABLE [acvp].[METADATA] (
    [id]                     BIGINT         IDENTITY (1, 1) NOT NULL,
    [test_session_id]        BIGINT         NOT NULL,
    [metadata]               NVARCHAR (MAX) NOT NULL,
    [validation_scenario_id] BIGINT         NULL,
    CONSTRAINT [PK_METADATA] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_METADATA_TEST_SESSION_ID] FOREIGN KEY ([test_session_id]) REFERENCES [acvp].[TEST_SESSION] ([id]),
    CONSTRAINT [FK_METADATA_VALIDATION_SCENARIO_ID] FOREIGN KEY ([validation_scenario_id]) REFERENCES [val].[VALIDATION_SCENARIO] ([id]),
    CONSTRAINT [UQ_TEST_SESSION_ID] UNIQUE NONCLUSTERED ([test_session_id] ASC)
);

