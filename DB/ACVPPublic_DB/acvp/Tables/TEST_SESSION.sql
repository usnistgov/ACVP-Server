CREATE TABLE [acvp].[TEST_SESSION] (
    [id]                  BIGINT        NOT NULL,
    [created_on]          DATETIME2 (7) NOT NULL,
    [acv_version_id]      INT           NOT NULL,
    [generator]           NVARCHAR (32) NOT NULL,
    [sample]              BIT           NOT NULL,
    [user_id]             BIGINT        NULL,
    [TestSessionStatusId] TINYINT       NULL,
    CONSTRAINT [PK_TEST_SESSION] PRIMARY KEY CLUSTERED ([id] ASC)
);



