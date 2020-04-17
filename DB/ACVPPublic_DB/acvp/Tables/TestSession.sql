CREATE TABLE [acvp].[TestSession] (
    [id]          BIGINT        NOT NULL,
    [created_on]  DATETIME2 (7) NOT NULL,
    [disposition] BIT           NULL,
    [passed_date] DATETIME2 (7) NULL,
    [published]   BIT           NOT NULL,
    [sample]      BIT           NOT NULL,
    [publishable] BIT           NOT NULL,
    [user_id]     BIGINT        NULL
);

