CREATE TABLE [acvp].[VECTOR_SET_EXPECTED_RESULTS] (
    [vector_set_id]       BIGINT          NOT NULL,
    [capabilities]        VARBINARY (MAX) NOT NULL,
    [prompt]              VARBINARY (MAX) NULL,
    [expected_results]    VARBINARY (MAX) NULL,
    [submitted_results]   VARBINARY (MAX) NULL,
    [validation_results]  VARBINARY (MAX) NULL,
    [internal_projection] VARBINARY (MAX) NULL,
    CONSTRAINT [PK_VECTOR_SET_EXPECTED_RESULTS] PRIMARY KEY CLUSTERED ([vector_set_id] ASC),
    CONSTRAINT [FK_VECTOR_SET_EXPECTED_RESULTS_VECTOR_SET_ID] FOREIGN KEY ([vector_set_id]) REFERENCES [acvp].[VECTOR_SET] ([id])
);

