CREATE TABLE [dbo].[PoolInformation] (
    [id]       BIGINT        IDENTITY (1, 1) NOT NULL,
    [poolName] VARCHAR (512) NOT NULL,
    CONSTRAINT [PK_PoolInformation] PRIMARY KEY CLUSTERED ([id] ASC)
);

