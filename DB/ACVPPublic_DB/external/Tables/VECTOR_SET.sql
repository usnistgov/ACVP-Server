CREATE TABLE [external].[VECTOR_SET] (
    [id]              BIGINT         IDENTITY (1, 1) NOT NULL,
    [test_session_id] BIGINT         NOT NULL,
    [token]           NVARCHAR (128) NULL,
    CONSTRAINT [PK_VECTOR_SET] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_VECTOR_SET_TEST_SESSION_ID] FOREIGN KEY ([test_session_id]) REFERENCES [external].[TEST_SESSION] ([id])
);

