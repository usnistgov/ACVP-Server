﻿CREATE TABLE [external].[TEST_SESSION] (
    [id]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [acv_version] NVARCHAR (10) NOT NULL,
    CONSTRAINT [PK_TEST_SESSION] PRIMARY KEY CLUSTERED ([id] ASC)
);
