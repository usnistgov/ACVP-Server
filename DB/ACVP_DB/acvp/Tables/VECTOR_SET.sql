CREATE TABLE [acvp].[VECTOR_SET] (
    [id]                   BIGINT         NOT NULL,
    [test_session_id]      BIGINT         NOT NULL,
    [generator_version]    NVARCHAR (10)  NOT NULL,
    [algorithm_id]         BIGINT         NOT NULL,
    [status]               INT            DEFAULT ((0)) NOT NULL,
    [vector_error_message] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_VECTOR_SET] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_VECTOR_SET_CRYPTO_ALGORITHM_ID] FOREIGN KEY ([algorithm_id]) REFERENCES [ref].[CRYPTO_ALGORITHM] ([id]),
    CONSTRAINT [FK_VECTOR_SET_TEST_SESSION_ID] FOREIGN KEY ([test_session_id]) REFERENCES [acvp].[TEST_SESSION] ([id])
);

