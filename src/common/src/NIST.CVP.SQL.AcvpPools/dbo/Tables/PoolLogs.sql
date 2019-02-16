CREATE TABLE [dbo].[PoolLogs] (
    [id]        BIGINT        IDENTITY (1, 1) NOT NULL,
    [logType]   BIGINT        NOT NULL,
    [poolId]    BIGINT        NULL,
    [dateStart] DATETIME2 (7) NOT NULL,
    [dateEnd]   DATETIME2 (7) NULL,
    [msg]       VARCHAR (MAX) NULL,
    CONSTRAINT [PK_PoolLogs] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_PoolLogs_PoolInformation] FOREIGN KEY ([poolId]) REFERENCES [dbo].[PoolInformation] ([id]),
    CONSTRAINT [FK_PoolLogs_PoolLogTypes] FOREIGN KEY ([logType]) REFERENCES [dbo].[PoolLogTypes] ([id])
);

