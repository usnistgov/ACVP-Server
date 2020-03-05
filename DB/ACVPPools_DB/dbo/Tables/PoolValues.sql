CREATE TABLE [dbo].[PoolValues] (
    [id]             BIGINT        IDENTITY (1, 1) NOT NULL,
    [poolId]         BIGINT        NOT NULL,
    [isStagingValue] BIT           CONSTRAINT [DF_PoolValues_isStagingValue] DEFAULT ((0)) NOT NULL,
    [dateCreated]    DATETIME2 (7) CONSTRAINT [DF_PoolValues_dateCreated] DEFAULT (getdate()) NOT NULL,
    [dateLastUsed]   DATETIME2 (7) NULL,
    [timesUsed]      BIGINT        CONSTRAINT [DF_PoolValues_timesUsed] DEFAULT ((0)) NOT NULL,
    [value]          VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_PoolValues] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_PoolValues_PoolInformation] FOREIGN KEY ([poolId]) REFERENCES [dbo].[PoolInformation] ([id])
);




GO
CREATE NONCLUSTERED INDEX [NonClusteredIndex-PoolId-Staging]
    ON [dbo].[PoolValues]([poolId] ASC, [isStagingValue] ASC);

