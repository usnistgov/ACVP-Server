CREATE TABLE [acvp].[TEST_SESSION] (
    [id]             BIGINT        NOT NULL,
    [created_on]     DATETIME2 (7) NOT NULL,
    [acv_version_id] INT           NOT NULL,
    [generator]      NVARCHAR (32) NOT NULL,
    [disposition]    BIT           NULL,
    [passed_date]    DATETIME2 (7) NULL,
    [published]      BIT           DEFAULT ((0)) NOT NULL,
    [sample]         BIT           DEFAULT ((0)) NOT NULL,
    [publishable]    BIT           DEFAULT ((1)) NOT NULL,
    [user_id]        BIGINT        NULL,
    CONSTRAINT [PK_TEST_SESSION] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_TEST_SESSION_ACV_VERSION_ID] FOREIGN KEY ([acv_version_id]) REFERENCES [ref].[ACV_VERSION] ([id])
);

