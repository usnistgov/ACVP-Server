﻿CREATE TABLE [val].[VALIDATION_NOTE] (
    [id]           BIGINT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [record_id]    BIGINT          NOT NULL,
    [algorithm_id] BIGINT          NULL,
    [internal]     BIT             NOT NULL,
    [note]         NVARCHAR (3000) NOT NULL,
    [updated_on]   DATETIME2 (7)   NOT NULL,
    CONSTRAINT [PK_VALIDATION_NOTE] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [UQ_VALIDATION_NOTE_RECORD_ALGORITHM] UNIQUE NONCLUSTERED ([record_id] ASC, [algorithm_id] ASC, [internal] ASC)
);
