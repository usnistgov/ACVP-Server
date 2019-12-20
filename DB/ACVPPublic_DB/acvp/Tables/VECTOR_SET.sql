CREATE TABLE [acvp].[VECTOR_SET] (
    [id]                   BIGINT         NOT NULL,
    [test_session_id]      BIGINT         NOT NULL,
    [generator_version]    NVARCHAR (10)  NOT NULL,
    [algorithm_id]         BIGINT         NOT NULL,
    [status]               INT            NOT NULL,
    [vector_error_message] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_VECTOR_SET] PRIMARY KEY CLUSTERED ([id] ASC)
);

