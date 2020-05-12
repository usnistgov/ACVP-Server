CREATE TABLE [acvp].[TEST_SESSION] (
    [id]             BIGINT        NOT NULL,
    [created_on]     DATETIME2 (7) NOT NULL,
    [acv_version_id] INT           NOT NULL,
    [generator]      NVARCHAR (32) NOT NULL,
    [sample]         BIT           DEFAULT ((0)) NOT NULL,
    [user_id]        BIGINT        NULL,
    [TestSessionStatusId] TINYINT NULL, 
    CONSTRAINT [PK_TEST_SESSION] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_TEST_SESSION_ACV_VERSION_ID] FOREIGN KEY ([acv_version_id]) REFERENCES [ref].[ACV_VERSION] ([id]),
    CONSTRAINT [FK_TEST_SESSION_TestSessionStatusId] FOREIGN KEY ([TestSessionStatusId]) REFERENCES [acvp].[TestSessionStatus] ([TestSessionStatusId])
);

