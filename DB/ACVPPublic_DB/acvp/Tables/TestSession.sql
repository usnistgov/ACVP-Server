CREATE TABLE [acvp].[TestSession] (
    [id]          BIGINT        NOT NULL,
    [created_on]  DATETIME2 (7) NOT NULL,
    [sample]      BIT           NOT NULL,
    [TestSessionStatusId] TINYINT NOT NULL,
    [user_id]     BIGINT        NULL
);

